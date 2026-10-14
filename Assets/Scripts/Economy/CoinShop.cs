using UnityEngine;

public class CoinShop : MonoBehaviour
{
    IPurchaseProvider store = new FakePurchaseProvider();

    public async void OnBuyPack()
    {
        if (Wallet.Instance == null)
            return;

        if (await store.PurchaseAsync("coins_500"))
            Wallet.Instance.Earn(500);
    }
}