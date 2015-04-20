using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{
    public class CreateItemVouchersRequest : IRequestClass
    {
        public int MinimumEnergy;
        public int TotalEnergy;
        public List<string> AndTags = null;
        public List<string> OrTags = null;

        public string ToHashable()
        {
            string hashable = MinimumEnergy.ToString() + TotalEnergy.ToString();
            if (AndTags != null)
                AndTags.ForEach(tag => { hashable += tag; });
            if (OrTags != null)
                OrTags.ForEach(tag => { hashable += tag; });
            return hashable;
        }

        public CreateItemVouchersRequest(int minimumEnergy, int totalEnergy, List<string> andTags = null, List<string> orTags = null)
        {
            MinimumEnergy = minimumEnergy;
            TotalEnergy = totalEnergy;
            AndTags = andTags;
            OrTags = orTags;
        }
    }
}
