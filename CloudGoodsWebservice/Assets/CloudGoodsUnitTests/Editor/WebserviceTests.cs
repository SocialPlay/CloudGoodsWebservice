using UnityEngine;
using System.Collections;
using LitJson;
using NUnit.Framework;
using System;
using Moq;

namespace WebserviceTests
{
    [TestFixture]
    [Category("LitJsonResponseTests")]
    public class LitJsonTests
    {
        ResponseCreator responseCreator;

        [SetUp]
        public void Init()
        {
            responseCreator = new LitJsonResponseCreator();
        }

        #region DataValidationTests
        [Test]
        [ExpectedException(typeof(WebserviceException), ExpectedMessage = "Error 0: ErrorMessage")]
        public void WebserviceErrorThrowsException()
        {
            string mockJsonstring = "{\"errorCode\":0,\"message\":\"ErrorMessage\"}";

            responseCreator.IsWebserviceError(mockJsonstring);
        }

        [Test]
        public void NoWebserviceErrorReturnsFalse()
        {
            string mockJsonstring = "{\"SessionId\":\"ae3af4a3-de7e-4d59-818e-10b11030df1e\",\"UserId\":\"01\",\"Username\":\"Test User\",\"Email\":\"test@user.com\",\"Signature\":\"123456\"}";

            Assert.IsFalse(responseCreator.IsWebserviceError(mockJsonstring));
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Invalid Data received from webservice")]
        public void InvalidWebserviceDataThrowsException()
        {
            responseCreator.IsValidData("Some invalid data");
        }

        [Test]
        public void ValidWebserviceDataReturnsTrue()
        {
             string mockJsonstring = "{\"SessionId\":\"ae3af4a3-de7e-4d59-818e-10b11030df1e\",\"UserId\":\"01\",\"Username\":\"Test User\",\"Email\":\"test@user.com\",\"Signature\":\"123456\"}";

             Assert.IsTrue(responseCreator.IsValidData(mockJsonstring));
        }
        #endregion


        #region UserManagementTests
        [Test]
        public void WebserviceLoginCloudGoodsUserGenerated()
        {
            string mockJsonstring = "{\"SessionId\":\"ae3af4a3-de7e-4d59-818e-10b11030df1e\",\"UserId\":\"01\",\"Username\":\"Test User\",\"Email\":\"test@user.com\",\"Signature\":\"123456\"}";

            CloudGoodsUser loginInfo = responseCreator.CreateLoginResponse(mockJsonstring);

            Assert.AreEqual("Test User", loginInfo.userName);
            Assert.AreEqual("01", loginInfo.UserID);
            Assert.AreEqual("test@user.com", loginInfo.userEmail);
            Assert.AreEqual("ae3af4a3-de7e-4d59-818e-10b11030df1e", loginInfo.sessionID);

        }

        #endregion
    }
}
