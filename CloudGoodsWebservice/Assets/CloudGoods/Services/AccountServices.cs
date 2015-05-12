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
        public static void Login(LoginRequest request, Action<CloudGoodsUser> callback)
        {
            CallHandler.Instance.Login(request, user =>
            {
                _ActiveUser = user;

                callback(user);
            });
        }

        public static void Register(RegisterUserRequest request, Action<RegisteredUser> callback)
        {
            CallHandler.Instance.Register(request, callback);
        }

        public static void ForgotPassword(ForgotPasswordRequest request, Action<StatusMessageResponse> callback)
        {
            CallHandler.Instance.ForgotPassword(request, callback);
        }

        public static void ResendVerificationEmail(ResendVerificationRequest request, Action<StatusMessageResponse> callback)
        {
            CallHandler.Instance.ResendVerificationEmail(request, callback);
        }


        /// <summary>
        /// Log a user into the cloudgoods system.
        /// Only usable for external Users (Use Login for SP Users) 
        /// </summary> 
        public static void LoginByPlatform(LoginByPlatformRequest request, Action<CloudGoodsUser> callback)
        {
            CallHandler.Instance.LoginByPlatform(request, user =>
            {
                _ActiveUser = user;
                callback(user);
            });
        }

    }
}
