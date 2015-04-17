using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudGoods.SDK.Models
{
    /// <summary>
    /// Response object for user values
    /// <para>RequestObject:"<see cref="LoginRequest"/></para>
    /// <para>RequestObject:"<see cref="LoginWithPlatformRequest"/></para>
    /// </summary>
    public class CloudGoodsUser
    {
        public string UserID = "";
        public bool IsNewUserToWorld = false;
        public string UserName = "";
        public string UserEmail = "";
        public string SessionId;
    }
}
