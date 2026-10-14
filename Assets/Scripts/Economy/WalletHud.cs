using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletHud : MonoBehaviour
{
    [SerializeField] TMP_Text coinsLabel;
    [SerializeField] TMP_Text upgradeLabel;
    [SerializeField] Button upgradeButton;

    Wallet wallet;

    void OnEnable()
    {
        wallet = Wallet.Instance;

        if (wallet == null)
        {
            Debug.LogError("Wallet not found.");
            return;
        }

        wallet.Changed += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        if (wallet != null)
            wallet.Changed -= Refresh;
    }

    void Refresh()
    {
        coinsLabel.SetText("Coins: {0}", wallet.Coins);
        upgradeLabel.SetText("Upgrade ({0})", wallet.NextCost);
        upgradeButton.interactable = wallet.Coins >= wallet.NextCost;
    }

    public void OnUpgradePressed()
    {
        wallet.TryUpgrade();
    }
}