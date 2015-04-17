using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class WebserviceError
    {
        public int ErrorCode;
        public string Message;

        public WebserviceError(int newErrorCode, string newErrorMessage)
        {
            ErrorCode = newErrorCode;
            Message = newErrorMessage;
        }
    }
}
