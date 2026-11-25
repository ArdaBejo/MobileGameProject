using System;
using System.Threading;
using UnityEngine;

public class CoinShop : MonoBehaviour
{
    readonly IPurchaseProvider store = new FakePurchaseProvider();
    CancellationTokenSource purchase;
    void OnEnable() => LifecycleGuard.PausedChanged += OnPaused;
    void OnDisable()
    {
        LifecycleGuard.PausedChanged -= OnPaused;
        purchase?.Cancel();
    }
    void OnPaused(bool paused) { if (paused) purchase?.Cancel(); }
    public async void OnBuyPack()
    {
        if (purchase != null || Wallet.Instance == null || LifecycleGuard.IsPaused) return;
        using var pending = CancellationTokenSource.CreateLinkedTokenSource(
            destroyCancellationToken, Application.exitCancellationToken);
        purchase = pending;
        try
        {
            if (await store.PurchaseAsync("coins_500", pending.Token)) Wallet.Instance?.Earn(500);
        }
        catch (OperationCanceledException) { }
        finally { purchase = null; }
    }
}
