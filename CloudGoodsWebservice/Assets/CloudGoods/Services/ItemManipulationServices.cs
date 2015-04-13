using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CloudGoods.Services.WebCommunication;
using CloudGoods.SDK.Models;

namespace CloudGoods.Services
{
    public class ItemManipulationServices
    {
        public static void GetUserItems(int location, Action<List<InstancedItemInformation>> callback)
        {
            CallHandler.Instance.GetUserItems(location, callback);
        }

        public static void UserItem(int lookupItemId, int location, Action<SimpleItemInfo> callback)
        {
            CallHandler.Instance.UserItem(lookupItemId, location, callback);
        }

        public static void MoveItem(OwnedItemInformation item, int location, int amountToMove, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            List<MoveItemsRequest.MoveOrder> orders = new List<MoveItemsRequest.MoveOrder>(){
                new MoveItemsRequest.MoveOrder(){
                    StackId = item.StackLocationId,
                    Amount = amountToMove,
                    Location =location
                }
            };
            CallHandler.Instance.MoveItems(orders, callback);
        }

        public static void MoveItems(List<MoveItemsRequest.MoveOrder> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            CallHandler.Instance.MoveItems(orders, callback);
        }

        public static void UpdateItemById(int itemId, int amount, int location, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            List<UpdateItemByIdRequest.UpdateOrderByID> orders = new List<UpdateItemByIdRequest.UpdateOrderByID>(){
            new UpdateItemByIdRequest.UpdateOrderByID(){
                itemId = itemId,
                amount = amount,
                location = location
            }
        };
            CallHandler.Instance.UpdateItemsByIds(orders, callback, otherOwner);
        }

        public static void UpdateItemsByIds(List<UpdateItemByIdRequest.UpdateOrderByID> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            CallHandler.Instance.UpdateItemsByIds(orders, callback, otherOwner);
        }

        public static void UpdateItemByStackIds(string stackId, int amount, int location, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders = new List<UpdateItemsByStackIdRequest.UpdateOrderByStackId>(){
            new UpdateItemsByStackIdRequest.UpdateOrderByStackId(){
                stackId = stackId,
                amount = amount,
                location = location
            }
        };
            CallHandler.Instance.UpdateItemByStackIds(orders, callback, otherOwner);
        }

        public static void UpdateItemByStackIds(List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders, Action<UpdatedStacksResponse> callback, OtherOwner destinationOwner = null)
        {
            CallHandler.Instance.UpdateItemByStackIds(orders, callback, destinationOwner);
        }

        public static void RedeemItemVouchers(List<RedeemItemVouchersRequest.ItemVoucherSelection> selections, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null) //ToDo: Add callback
        {
            CallHandler.Instance.RedeemItemVoucher(selections, callback, otherOwner);
        }

        public static void CreateItemVouchers(int minEnergy, int total, Action<ItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
        {
            CallHandler.Instance.CreateItemVouchers(minEnergy, total, callback, andTags, orTags);
        }

        public static void GetItemVoucher(int voucherId, Action<ItemVouchersResponse> callback)
        {
            CallHandler.Instance.GetItemVoucher(voucherId, callback);
        }

    }
}
