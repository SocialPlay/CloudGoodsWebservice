using UnityEngine;
using System;
using System.Collections;
using CloudGoods.Enums;
using CloudGoods.SDK.Models;
using CloudGoods.Services.WebCommunication;


namespace CloudGoods.Services
{
    public class AccountServices
    {
        public static CloudGoodsUser ActiveUser { get { return _ActiveUser; } }

        private static CloudGoodsUser _ActiveUser = null;


        public static void Logout()
        {
            _ActiveUser = null;
          
        }

        /// <summary>
        /// Log a user into the cloudgoods system.
        /// Only usable for SP Users (Use LoginByPlatform for external login)
        /// </summary>
        public static void Login(string userEmail, string password, Action<CloudGoodsUser> callback)
        {
            CallHandler.Instance.Login(userEmail, password, user =>
            {
                _ActiveUser = user;
                Debug.Log("User Set" +"\nSession: "+_ActiveUser.SessionId);
               
                callback(user);
            });
        }

        public static void Register(string appId, string userName, string userEmail, string password, Action<RegisteredUser> callback)
        {
            CallHandler.Instance.Register(appId, userName, userEmail, password, callback);
        }

        public static void ForgotPassword(string userEmail, Action<StatusMessageResponse> callback)
        {
            CallHandler.Instance.ForgotPassword(userEmail, callback);
        }

        public static void ResendVerificationEmail(string email, Action<StatusMessageResponse> callback)
        {
            CallHandler.Instance.ResendVerificationEmail(email, callback);
        }


        /// <summary>
        /// Log a user into the cloudgoods system.
        /// Only usable for external Users (Use Login for SP Users) 
        /// </summary> 
        public static void LoginByPlatform(string userName, CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, Action<CloudGoodsUser> callback)
        {
            CallHandler.Instance.LoginByPlatform(userName, cloudGoodsPlatform, platformUserID, user =>
            {
                _ActiveUser = user;
                callback(user);
            });
        }

    }
}
