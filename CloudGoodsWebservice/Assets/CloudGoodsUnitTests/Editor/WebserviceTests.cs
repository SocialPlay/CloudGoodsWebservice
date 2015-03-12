using UnityEngine;
using System.Collections;
using LitJson;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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

        #region Data Validation
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

        [Test]
        public void TimestampDeltaCalculation()
        {
            
        }
        #endregion

        #region User Management
        [Test]
        public void WebserviceLoginCloudGoodsUserGenerated()
        {
            string mockJsonstring = "{\"SessionId\":\"ae3af4a3-de7e-4d59-818e-10b11030df1e\",\"UserId\":\"01\",\"Username\":\"Test User\",\"Email\":\"test@user.com\",\"Signature\":\"123456\"}";

            CloudGoodsUser loginInfo = responseCreator.CreateLoginResponse(mockJsonstring);

            Assert.AreEqual("Test User", loginInfo.userName);
            Assert.AreEqual("01", loginInfo.userID);
            Assert.AreEqual("test@user.com", loginInfo.userEmail);
            Assert.AreEqual("ae3af4a3-de7e-4d59-818e-10b11030df1e", loginInfo.sessionID);
        }

        #endregion

        #region Item Management

        [Test]
        public void GetUserItemsCreatesListOfItemDatas()
        {
            string mockJsonstring = "[{\"Id\":1,\"collectionId\":2,\"stackLocationId\":\"1234567\",\"classId\":3,\"name\":\"Test Item\",\"amount\":4,\"location\":5,\"detail\":\"Some details here\",\"energy\":6,\"behaviours\":[{\"name\":\"0\",\"Id\":0},{\"name\":\"1\",\"Id\":1}],\"tags\":[{\"name\":\"0\",\"Id\":0},{\"name\":\"1\",\"Id\":1},{\"name\":\"2\",\"Id\":2}]},{\"Id\":1,\"collectionId\":2,\"stackLocationId\":\"1234567\",\"classId\":3,\"name\":\"Test Item\",\"amount\":4,\"location\":5,\"detail\":\"Some details here\",\"energy\":6,\"behaviours\":[{\"name\":\"0\",\"Id\":0},{\"name\":\"1\",\"Id\":1}],\"tags\":[{\"name\":\"0\",\"Id\":0},{\"name\":\"1\",\"Id\":1},{\"name\":\"2\",\"Id\":2}]}]";

            List<ItemData> items = responseCreator.CreateItemDataListResponse(mockJsonstring);

            ItemData itemData = items[0];

            Assert.AreEqual(1, itemData.Id);
            Assert.AreEqual(2, itemData.CollectionId);
            Assert.AreEqual("1234567", itemData.StackLocationId);
            Assert.AreEqual(3, itemData.ClassId);
            Assert.AreEqual("Test Item", itemData.Name);
            Assert.AreEqual(4, itemData.Amount);
            Assert.AreEqual(5, itemData.Location);
            Assert.AreEqual("Some details here", itemData.Detail);
            Assert.AreEqual(6, itemData.Energy);

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(itemData.tags[i].Id, i);
                Assert.AreEqual(itemData.tags[i].name, i.ToString());
            }

            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(itemData.behaviours[i].Id, i);
                Assert.AreEqual(itemData.behaviours[i].name, i.ToString());
            }

        }


        #endregion
    }
}
