using UnityEngine;
using System.Collections;


namespace CloudGoods.SDK.Models
{

    public class InstancedItemInformation
    {
        public string StackLocationId;
        public int Amount;
        public int Location;
        public ItemInformation Information;

        public InstancedItemInformation()
        {
            Information = new ItemInformation();
        }
    }

}