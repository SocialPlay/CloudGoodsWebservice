using UnityEngine;
using System;
using System.Collections;
using CloudGoods.Services.WebCommunication;
using CloudGoods.SDK.Utilities;
using CloudGoods.Enums;
using CloudGoods.Services;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK
{
    public static class CurrencyManager
    {
        public class CurrencyDetails
        {
            public string Name;
            public Texture2D Icon;
        }

        private static CurrencyDetails PremiumInfo = null;
        private static int? PremiumAmount = null;
        private static CurrencyDetails StandardInfo = null;
        private static int? StandardAmount = null;


        private static bool IsGettingWolrdCurrency = false;

        private static Action<string, Texture2D> RecivedPremiumDetails;
        private static Action<int> RecivecdPremiumAmount;
        private static Action<string, Texture2D> RecivedStandardDetails;
        private static Action<int> RecivecdStandardAmount;
        private static int StandardCurrencyLoaction = 0;

        public static void GetPremiumCurrencyDetails(Action<string, Texture2D> callback)
        {
            if (PremiumInfo == null)
            {
                GetWolrdCurrencyInfo(StandardCurrencyLoaction);
                RecivedPremiumDetails += callback;
            }
            else
            {
                callback(PremiumInfo.Name, PremiumInfo.Icon);
            }
        }

        public static void GetPremiumCurrencyBalance(Action<int> callback, bool forceUpdate = true)
        {
            if (PremiumAmount == null)
            {
                GetWolrdCurrencyInfo(StandardCurrencyLoaction);
                RecivecdPremiumAmount += callback;
            }
            else if (forceUpdate)
            {
                ItemStoreServices.GetPremiumCurrencyBalance(balance =>
                {
                    callback(balance.Amount);
                });
            }
            else
            {
                callback(PremiumAmount.GetValueOrDefault(0));
            }
        }

        public static void GetStandardCurrencyBalance(int location, Action<int> callback, bool forceUpdate = true)
        {
            StandardCurrencyLoaction = location;
            if (StandardAmount == null)
            {
                GetWolrdCurrencyInfo(StandardCurrencyLoaction);
                RecivecdStandardAmount += callback;
            }
            else if (forceUpdate)
            {
                ItemStoreServices.GetStandardCurrencyBalance(new StandardCurrencyBalanceRequest(StandardCurrencyLoaction), item =>
                {
                    StandardAmount = item.Amount;
                    callback(StandardAmount.GetValueOrDefault(0));
                });
            }
            else
            {
                callback(StandardAmount.GetValueOrDefault(0));
            }

        }

        public static void GetStandardCurrencyDetails(int location, Action<string, Texture2D> callback)
        {
            StandardCurrencyLoaction = location;
            if (StandardInfo == null)
            {
                GetWolrdCurrencyInfo(StandardCurrencyLoaction);
                RecivedStandardDetails += callback;
            }
            else
            {
                callback(StandardInfo.Name, StandardInfo.Icon);
            }
        }


        private static void GetWolrdCurrencyInfo(int location)
        {
            if (!IsGettingWolrdCurrency)
            {
                IsGettingWolrdCurrency = true;
                ItemStoreServices.GetCurrencyInfo(worldCurrencyInfo =>
                {
                    ItemTextureCache.GetItemTexture(worldCurrencyInfo.PremiumCurrencyImage, icon =>
                    {
                        PremiumInfo = new CurrencyDetails()
                        {
                            Name = worldCurrencyInfo.PremiumCurrencyName,
                            Icon = icon
                        };
                        if (RecivedPremiumDetails != null)
                        {
                            RecivedPremiumDetails(PremiumInfo.Name, PremiumInfo.Icon);
                        }
                    });
                    ItemStoreServices.GetPremiumCurrencyBalance(premiumCurrencyResponse =>
                    {
                        PremiumAmount = premiumCurrencyResponse.Amount;
                        if (RecivecdPremiumAmount != null)
                        {
                            RecivecdPremiumAmount(premiumCurrencyResponse.Amount);
                        }
                    });

                    ItemTextureCache.GetItemTexture(worldCurrencyInfo.StandardCurrencyImage, icon =>
                    {
                        StandardInfo = new CurrencyDetails()
                        {
                            Name = worldCurrencyInfo.StandardCurrencyName,
                            Icon = icon
                        };
                        if (RecivedStandardDetails != null)
                        {
                            RecivedStandardDetails(StandardInfo.Name, StandardInfo.Icon);
                        }
                    });

                    ItemStoreServices.GetStandardCurrencyBalance(new StandardCurrencyBalanceRequest(StandardCurrencyLoaction), standardCurrencyItem =>
                    {
                        StandardAmount = standardCurrencyItem.Amount;
                        if (RecivecdStandardAmount != null)
                        {
                            RecivecdStandardAmount(standardCurrencyItem.Amount);
                        }
                    });
                });
            }
        }
    }
}
