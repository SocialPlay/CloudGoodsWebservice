using UnityEngine;
using System.Collections;
using CloudGoods.Models;
using CloudGoods.Store;

namespace CloudGoods.CurrencyPurchase
{
    public class LoadStoreOnRegister : MonoBehaviour
    {
        public StoreInitializer displayStoreItems;

        void Start()
        {
            CallHandler.OnRegisteredUserToSession += UserRegistered;
        }

        void UserRegistered(string userID)
        {
            displayStoreItems.InitializeStore();
        }
    }
}
