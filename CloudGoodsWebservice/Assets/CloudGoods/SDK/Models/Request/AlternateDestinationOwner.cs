using UnityEngine;
using System.Collections;


namespace CloudGoods.SDK.Models
{
    public class AlternateDestinationOwner : IRequestClass
    {
        public string Id = "Default";
        public string Type = "User";

        public AlternateDestinationOwner()
        {
            Id = "Default";
            Type = "User";
        }

        public AlternateDestinationOwner(string Id, string type)
        {
            this.Id = Id;
            this.Type = type;
        }

        public string ToHashable()
        {
            return Id + Type;
        }

        public static string ToHashable(AlternateDestinationOwner value)
        {
            return value == null ? "" : value.ToHashable();
        }
      
    }
}
