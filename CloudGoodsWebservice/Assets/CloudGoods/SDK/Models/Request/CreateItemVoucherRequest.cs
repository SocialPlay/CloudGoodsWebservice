using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{
    public class CreateItemVouchersRequest : IRequestClass
    {
        public int MinimumEnergy;
        public int MaximumEnergy;
        public int TotalEnergy;
        public TagSelection Tags = null;

        public string ToHashable()
        {
            return MinimumEnergy.ToString() + MaximumEnergy.ToString() + TotalEnergy.ToString() + (Tags != null ? Tags.ToHashable() : "");
        }

        public CreateItemVouchersRequest(int totalEnergy,int minimumEnergy,int maximumEnergy , TagSelection tags = null)
        {
            MinimumEnergy = minimumEnergy;
            MaximumEnergy = maximumEnergy;
            TotalEnergy = totalEnergy;
            Tags = tags;
        }
    }
}
