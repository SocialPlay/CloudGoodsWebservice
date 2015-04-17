using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class AppDataAllRequest : IRequestClass
    {
        public string ToHashable()
        {
            return "AppDataAllRequest";
        }
    }
}
