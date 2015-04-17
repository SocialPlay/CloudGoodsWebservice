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
        public static void UserItems(UserItemsRequest request, Action<List<InstancedItemInformation>> callback)
        {
            CallHandler.Instance.UserItems(request, callback);
        }

        public static void UserItem(OwnerItemRequest request, Action<SimpleItemInfo> callback)
        {
            CallHandler.Instance.UserItem(request, callback);
        }

        public static void MoveItem(MoveItemsRequest request, Action<UpdatedStacksResponse> callback)
        {

            CallHandler.Instance.MoveItems(request, callback);
        }

        public static void UpdateItemsByIds(UpdateItemsByIdsRequest request, Action<UpdatedStacksResponse> callback)
        {
            CallHandler.Instance.UpdateItemsByIds(request, callback);
        }

        public static void UpdateItemByStackIds(string stackId, int amount, int location, Action<UpdatedStacksResponse> callback, AlternateDestinationOwner otherOwner = null)
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

        public static void UpdateItemByStackIds(List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders, Action<UpdatedStacksResponse> callback, AlternateDestinationOwner destinationOwner = null)
        {
            CallHandler.Instance.UpdateItemByStackIds(orders, callback, destinationOwner);
        }

        public static void RedeemItemVouchers(RedeemItemVouchersRequest request, Action<UpdatedStacksResponse> callback)
        {
            CallHandler.Instance.RedeemItemVoucher(request, callback);
        }

        public static void CreateItemVouchers(CreateItemVouchersRequest request, Action<ItemVouchersResponse> callback)
        {
            CallHandler.Instance.CreateItemVouchers(request, callback);
        }

        public static void GetItemVoucher(ItemVoucherRequest request, Action<ItemVouchersResponse> callback)
        {
            CallHandler.Instance.ItemVoucher(request, callback);
        }

    }
}
