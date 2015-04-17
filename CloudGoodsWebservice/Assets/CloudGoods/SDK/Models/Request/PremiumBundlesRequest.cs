using UnityEngine;
using System.Collections;


namespace CloudGoods.SDK.Models
{
    public class PremiumBundlesRequest : IRequestClass
    {
        public int PlatformId;

        public string ToHashable()
        {
            return PlatformId.ToString();
        }

        public PremiumBundlesRequest(int platformId)
        {
            PlatformId = platformId;
        }
    }
}
