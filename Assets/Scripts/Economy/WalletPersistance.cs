using UnityEngine;

public class WalletPersistence : MonoBehaviour
{
    [SerializeField] Wallet wallet;

    void Awake()
    {
        if (wallet == null)
            wallet = GetComponent<Wallet>();

        WalletSave.Load(wallet);
        wallet.Changed += SaveWallet;
    }

    void OnDestroy()
    {
        if (wallet != null)
            wallet.Changed -= SaveWallet;
    }

    void SaveWallet()
    {
        WalletSave.Save(wallet);
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
            WalletSave.Save(wallet);
    }
}