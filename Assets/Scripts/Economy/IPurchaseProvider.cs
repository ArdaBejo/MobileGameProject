using UnityEngine;
using System.Threading;

public interface IPurchaseProvider
{
    Awaitable<bool> PurchaseAsync(string productId, CancellationToken token);
}