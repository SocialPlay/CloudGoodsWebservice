using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CloudGoods.SDK.Models
{
    public class CurrencyInfoRequest:IRequestClass
    {
        public string ToHashable()
        {
            return "CurrencyInfoRequest";
        }
    }
}
