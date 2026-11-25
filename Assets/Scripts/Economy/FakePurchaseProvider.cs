using UnityEngine;
using System.Threading;

public class FakePurchaseProvider : IPurchaseProvider
{
    public async Awaitable<bool> PurchaseAsync(string productId, CancellationToken token)
    {
        Debug.Log($"[iap-stub] purchase {productId}");

        await Awaitable.WaitForSecondsAsync(0.5f, token);

        return true;
    }
}