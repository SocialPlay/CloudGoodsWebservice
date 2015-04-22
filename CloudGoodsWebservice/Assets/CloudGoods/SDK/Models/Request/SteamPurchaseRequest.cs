using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;

public class SteamPurchaseRequest : IRequestClass {

	public string SteamUserId;
    public int bundleId;

    public string ToHashable()
    {
        return SteamUserId + bundleId;
    }
}
