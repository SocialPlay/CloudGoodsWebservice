using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;


public class SteamOrderConfirmationRequest : IRequestClass
{
    public string AppId;
    public int OrderId;
    public int BundleId;

    public string ToHashable()
    {
        return OrderId + AppId;
    }
}
