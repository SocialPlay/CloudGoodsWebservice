using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;


namespace CloudGoods.SDK.Models
{
    public class StoreItem
    {

        public ItemInformation ItemInformation;
        public List<SalePrices> Sale;
        public int ItemId;
        public int CreditValue;
        public int CoinValue;
        public DateTime AddDate;

        public List<StoreItemDetail> ItemDetails
        {
            get
            {
                if (ParsedItemDetail != null)
                    return ParsedItemDetail;
                else
                {
                    ParsedItemDetail = new List<StoreItemDetail>();

                    JsonData data = JsonMapper.ToObject(ItemInformation.Detail);
                    for (int i = 0; i < data.Count; i++)
                    {
                        StoreItemDetail itemDetail = new StoreItemDetail()
                        {
                            Name = data[i]["Name"].ToString(),
                            Value = data[i]["Value"].ToString()
                        };
                        ParsedItemDetail.Add(itemDetail);
                    }
                    return ParsedItemDetail;
                }
            }
        }

        List<StoreItemDetail> ParsedItemDetail;

        public class StoreItemDetail
        {
            public string Name;
            public string Value;
            public bool InvertEnergy;
        }
    }
}