using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using CloudGoods.Enums;
using CloudGoods.Models;
using CloudGoods.Utilities;

namespace CloudGoods.Login
{
    public class UnityUICloudGoodsLogin : MonoBehaviour
    {

        #region Login variables
        public GameObject loginTab;
        public InputField loginUserEmail;
        public InputField loginUserPassword;
        public Text loginErrorLabel;

        public Toggle autoLoginToggle;

        public GameObject resendVerificationTextObject;


        private InputFieldValidation loginUserEmailValidator;
        private InputFieldValidation loginUserPasswordValidator;


        #endregion

        #region Register variables
        public GameObject registerTab;
        public InputField registerUserEmail;
        public InputField registerUserPassword;
        public InputField registerUserPasswordConfirm;
        public InputField registerUserName;
        public Text registerErrorLabel;

        private InputFieldValidation registerUserEmailValidator;
        private InputFieldValidation registerUserPasswordValidator;
        private InputFieldValidation registerUserPasswordConfirmValidator;
        #endregion

        #region Confirmations Variables
        public GameObject confirmationTab;
        public Text confirmationStatus;
        #endregion

        #region ResendVerification Variables

        public GameObject ResendVerificationWindow;

        #endregion

        public bool IsKeptActiveOnAllPlatforms;

        void OnEnable()
        {
            CallHandler.OnUserAuthorized += RecivedUserGuid;
            CallHandler.OnForgotPassword += ForgotPasswordResponce;
            CallHandler.OnVerificationSent += ResentVerificationResponce;
            CallHandler.onLogout += OnLogout;
            CallHandler.IsError += CallHandler_onErrorEvent;
        }

        void OnDisable()
        {
            CallHandler.OnUserAuthorized -= RecivedUserGuid;
            CallHandler.OnForgotPassword -= ForgotPasswordResponce;
            CallHandler.OnVerificationSent -= ResentVerificationResponce;
            CallHandler.onLogout -= OnLogout;
            CallHandler.IsError -= CallHandler_onErrorEvent;
        }

        void Start()
        {
            if (!RemoveIfNeeded())
            {
                return;
            }

            registerErrorLabel.text = "";
            registerTab.SetActive(false);
            confirmationTab.SetActive(false);

            loginUserEmailValidator = loginUserEmail.GetComponent<InputFieldValidation>();
            loginUserPasswordValidator = loginUserPassword.GetComponent<InputFieldValidation>();

            registerUserEmailValidator = registerUserEmail.GetComponent<InputFieldValidation>();
            registerUserPasswordValidator = registerUserPassword.GetComponent<InputFieldValidation>(); ;
            registerUserPasswordConfirmValidator = registerUserPasswordConfirm.GetComponent<InputFieldValidation>();
            resendVerificationTextObject.SetActive(false);
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SocialPlay_Login_UserEmail")))
            {
                loginUserEmail.text = PlayerPrefs.GetString("SocialPlay_Login_UserEmail");
            }
            else
            {
                loginUserEmail.text = "";
            }

            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SocialPlay_UserGuid")))
            {
                CloudGoodsUser userInfo = new CloudGoodsUser()
                {
                    UserID = PlayerPrefs.GetString("SocialPlay_UserGuid"), 
                    UserName = PlayerPrefs.GetString("SocialPlay_UserName"),
                    UserEmail = PlayerPrefs.GetString("SocialPlay_UserEmail")
                };

                CallHandler.Login(CloudGoodsPlatform.SocialPlay, userInfo.UserID, userInfo.UserEmail, "", RecivedUserGuid);

                RecivedUserGuid(userInfo);
            }

        }

        #region webservice responce events

        void RecivedUserGuid(CloudGoodsUser obj)
        {
            if (autoLoginToggle != null && autoLoginToggle.isOn == true)
            {
                PlayerPrefs.SetString("SocialPlay_UserGuid", obj.UserID.ToString());
                PlayerPrefs.SetString("SocialPlay_UserName", obj.UserName);
                PlayerPrefs.SetString("SocialPlay_UserEmail", obj.UserEmail);
            }

            resendVerificationTextObject.SetActive(false);
            loginErrorLabel.text = "User logged in";

            CloseAllTabsOnLogin();
        }

        void ResentVerificationResponce(UserResponse responce)
        {
            resendVerificationTextObject.SetActive(false);
            loginErrorLabel.text = responce.message;
        }

        void ForgotPasswordResponce(UserResponse responce)
        {
            confirmationStatus.text = responce.message;
        }

        void LoginSuccess(Guid userID)
        {
            loginErrorLabel.text = userID.ToString();
        }
        #endregion

        #region button functions

        public void DisplayLoginPanel()
        {
            loginTab.SetActive(true);
        }

        public void SwitchToRegister()
        {
            registerErrorLabel.text = "";
            loginTab.SetActive(false);
            registerTab.SetActive(true);
            confirmationTab.SetActive(false);
        }

        public void SwitchToLogin()
        {
            loginErrorLabel.text = "";
            registerTab.SetActive(false);
            loginTab.SetActive(true);
            confirmationTab.SetActive(false);
        }

        public void SwitchToConfirmation()
        {
            confirmationStatus.gameObject.SetActive(true);
            confirmationStatus.text = "Waiting ...";
            confirmationTab.SetActive(true);
            loginTab.SetActive(false);
            registerTab.SetActive(false);
        }

        public void CloseResendVerificationTab()
        {
            ResendVerificationWindow.SetActive(false);
        }

        void CloseAllTabsOnLogin()
        {
            confirmationTab.SetActive(false);
            loginTab.SetActive(false);
            registerTab.SetActive(false);
        }

        public void Login()
        {
            string ErrorMsg = "";
            if (!loginUserEmailValidator.IsValidCheck())
            {
                ErrorMsg = "-Invalid Email";
            }

            if (!loginUserPasswordValidator.IsValidCheck())
            {
                if (!string.IsNullOrEmpty(ErrorMsg)) ErrorMsg += "\n";
                ErrorMsg += "-Invalid Password";
            }
            loginErrorLabel.text = ErrorMsg;
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                Debug.Log("login email: " + loginUserEmail.text + " login password: " + loginUserPassword.text);

                PlayerPrefs.SetString("SocialPlay_Login_UserEmail", loginUserEmail.text);
                CallHandler.Login(CloudGoodsPlatform.SocialPlay, loginUserEmail.text.ToLower(),  loginUserEmail.text.ToLower(), loginUserPassword.text, RecivedUserGuid);
            }
        }

        public void Register()
        {
            string ErrorMsg = "";
            if (!registerUserEmailValidator.IsValidCheck())
            {
                if (!string.IsNullOrEmpty(ErrorMsg)) ErrorMsg += "\n";
                ErrorMsg += "-Invalid Email";
            }

            if (!registerUserPasswordValidator.IsValidCheck() || !registerUserPasswordConfirmValidator.IsValidCheck())
            {
                if (!string.IsNullOrEmpty(ErrorMsg)) ErrorMsg += "\n";
                ErrorMsg += "-Invalid Password";
            }
            registerErrorLabel.text = ErrorMsg;
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                SwitchToConfirmation();
                CallHandler.Register(CloudGoodsSettings.AppID, registerUserName.text, registerUserEmail.text, registerUserPassword.text, OnRegisteredUser);
            }
        }

        void OnRegisteredUser(RegisteredUser userResponse)
        {
            Debug.Log("User has been registered: " + userResponse.Active);

            confirmationStatus.text = "Verification Email has been sent to your Email";
        }

        public void ForgotPassword()
        {

            string ErrorMsg = "";
            if (!loginUserEmailValidator.IsValidCheck())
            {
                ErrorMsg = "Password reset requires valid E-mail";
            }
            loginErrorLabel.text = ErrorMsg;
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                SwitchToConfirmation();
                CallHandler.ForgotPassword(loginUserEmail.text, OnSentPassword);
            }
        }

        void OnSentPassword(StatusMessageResponse userResponse)
        {
            confirmationStatus.text = userResponse.message;
        }

        public void ResendVerificationEmail()
        {
            string ErrorMsg = "";
            if (!loginUserEmailValidator.IsValidCheck())
            {
                ErrorMsg = "Validation resend requires valid E-mail";
            }
            loginErrorLabel.text = ErrorMsg;
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                SwitchToConfirmation();
                CloseResendVerificationTab();
                CallHandler.ResendVerificationEmail(loginUserEmail.text, OnResentVerificationEmail);
            }

        }

        void OnResentVerificationEmail(StatusMessageResponse response)
        {
            confirmationStatus.text = response.message;
        }

        void OnLogout()
        {
            Debug.Log("User logged out");
            SwitchToLogin();
        }

        #endregion

        void CallHandler_onErrorEvent(WebserviceError obj)
        {
            Debug.Log("ErrorOccured: " + obj.ErrorCode + "   message: " + obj.Message);

            if(obj.ErrorCode == 1001 || obj.ErrorCode == 1000)
            {
                confirmationStatus.text = obj.Message;
            }
            
            if(obj.ErrorCode == 1003)
            {
                ResendVerificationWindow.SetActive(true);
            }
        }


        public bool RemoveIfNeeded()
        {
            if (IsKeptActiveOnAllPlatforms) return true;

            if (BuildPlatform.Platform == BuildPlatform.BuildPlatformType.Automatic)
            {
                BuildPlatform.OnBuildPlatformFound += platform => { RemoveIfNeeded(); };
                return false;
            }

            if (BuildPlatform.Platform == BuildPlatform.BuildPlatformType.Facebook || BuildPlatform.Platform == BuildPlatform.BuildPlatformType.Kongergate)
            {
                Destroy(loginTab);
                Destroy(registerTab);
                Destroy(confirmationTab);
                Destroy(this);
                return false;
            }
            return true;
        }
    }
}
