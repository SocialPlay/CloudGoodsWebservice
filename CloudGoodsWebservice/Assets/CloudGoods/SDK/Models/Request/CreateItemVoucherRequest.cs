using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{
    public class CreateItemVouchersRequest : IRequestClass
    {
        public int MinimumEnergy;
        public int TotalEnergy;
        public TagSelection Tags = null;

        public string ToHashable()
        {
            return MinimumEnergy.ToString() + TotalEnergy.ToString() + (Tags != null ? Tags.ToHashable() : "");
        }

        public CreateItemVouchersRequest(int minimumEnergy, int totalEnergy, TagSelection tags = null)
        {
            MinimumEnergy = minimumEnergy;
            TotalEnergy = totalEnergy;
            Tags = tags;
        }
    }
}
