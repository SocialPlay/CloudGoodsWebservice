using UnityEngine;
using System.Collections;

namespace CloudGoods.Webservice
{
    public interface HashCreator
    {

        string CreateHash(params string[] values);
    }
}
