using UnityEngine;
using System.Collections;

namespace CloudGoods.Services.Webservice
{
    public interface HashCreator
    {

        string CreateHash(params string[] values);
    }
}
