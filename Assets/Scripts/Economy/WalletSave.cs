using UnityEngine;

public static class WalletSave
{
    const string CoinsKey = "wallet.coins";
    const string LevelKey = "wallet.level";

    public static void Save(Wallet wallet)
    {
        PlayerPrefs.SetInt(CoinsKey, wallet.Coins);
        PlayerPrefs.SetInt(LevelKey, wallet.Level);
        PlayerPrefs.Save();
    }

    public static void Load(Wallet wallet)
    {
        wallet.Restore(
            PlayerPrefs.GetInt(CoinsKey, 0),
            PlayerPrefs.GetInt(LevelKey, 0)
        );
    }
}