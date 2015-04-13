using UnityEngine;
using System;
using System.Collections;
using CloudGoods.Services.WebCommunication;
using CloudGoods.SDK.Utilities;
using CloudGoods.Enums;
using CloudGoods.Services;

namespace CloudGoods.SDK
{
    public static class CurrencyManager
    {

        private static bool _IsInitilized = false;

        private static CurrencyInfo _PremiumcurrencyInfo = null;

        private static CurrencyInfo _StandardCurrencyInfo = null;

        public class CurrencyInfo
        {
            public string Name;
            public Texture2D Icon;
            public int Amount = -1;
        }

        private static bool isGettingWolrdCurrency = false;

        private static Action<CurrencyInfo> RecivedPremiumCurrency;
        private static Action<CurrencyInfo> RecivedStandardCurrency;

        public static void GetPremiumCurrency(int location, Action<CurrencyInfo> callback, bool dontUpdate = false)
        {
            if (!_IsInitilized)
            {
                GetWolrdCurrencyInfo(location);
                RecivedPremiumCurrency += callback;
            }
            else
            {
                if (dontUpdate) callback(_PremiumcurrencyInfo);
                else
                {
                    ItemStoreServices.GetPremiumCurrencyBalance(x =>
                    {
                        _PremiumcurrencyInfo.Amount = x.Amount;
                        callback(_PremiumcurrencyInfo);
                    });
                }
            }
        }

        public static void GetStandardCurrency(int location, Action<CurrencyInfo> callback, bool dontUpdate = false)
        {
            if (!_IsInitilized)
            {
                GetWolrdCurrencyInfo(location);
                RecivedStandardCurrency += callback;
            }
            else
            {
                if (dontUpdate) callback(_StandardCurrencyInfo);
                else
                {
                    ItemStoreServices.GetPremiumCurrencyBalance(x =>
                    {
                        _StandardCurrencyInfo.Amount = x.Amount;
                        callback(_StandardCurrencyInfo);
                    });
                }
            }
        }



        private static void GetWolrdCurrencyInfo(int location)
        {
            if (!isGettingWolrdCurrency)
                ItemStoreServices.GetCurrencyInfo(WorldCurrencyInfo =>
                {
                    _PremiumcurrencyInfo = new CurrencyInfo() { Name = WorldCurrencyInfo.PremiumCurrencyName };
                    ItemStoreServices.GetPremiumCurrencyBalance(premiumCurrencyResponse =>
                    {
                        _PremiumcurrencyInfo.Amount = premiumCurrencyResponse.Amount;
                        ItemTextureCache.GetItemTexture(WorldCurrencyInfo.PremiumCurrencyImage, icon =>
                        {
                            _PremiumcurrencyInfo.Icon = icon;
                            RecivedPremiumCurrency(_PremiumcurrencyInfo);
                            RecivedPremiumCurrency = null;
                        });
                    });

                    _StandardCurrencyInfo = new CurrencyInfo() { Name = WorldCurrencyInfo.StandardCurrencyName };
                    ItemStoreServices.GetStandardCurrencyBalance(location, standardCurrencyItem =>
                    {
                        _StandardCurrencyInfo.Amount = standardCurrencyItem.Amount;
                        ItemTextureCache.GetItemTexture(WorldCurrencyInfo.PremiumCurrencyImage, icon =>
                        {
                            _StandardCurrencyInfo.Icon = icon;
                            RecivedStandardCurrency(_StandardCurrencyInfo);
                            RecivedStandardCurrency = null;
                        });
                    });

                });
        }
    }
}
