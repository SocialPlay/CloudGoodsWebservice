using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class UserDataAllRequest : IRequestClass
    {
        public string ToHashable()
        {
            return "UserDataAllRequest";
        }
    }
}
