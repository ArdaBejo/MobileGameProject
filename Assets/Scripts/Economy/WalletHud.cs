using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletHud : MonoBehaviour
{
    [SerializeField] TMP_Text coinsLabel;
    [SerializeField] TMP_Text upgradeLabel;
    [SerializeField] Button upgradeButton;

    Wallet wallet;
    TMP_Text damageLabel;

    void Start()
    {
        // Keep authored button references intact; replace their old layout at runtime.
        coinsLabel.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        var ui = GreyboxUI.Canvas("EconomyCanvas", -1);
        GreyboxUI.Band(ui, "UpgradeStrip", false, 132);
        coinsLabel = GreyboxUI.Label(ui, "Coins", "", new Vector2(1, 1), new Vector2(-110, -44), new Vector2(190, 46), 26);
        coinsLabel.color = GreyboxUI.Accent;
        var title = GreyboxUI.Label(ui, "WeaponLabel", "Pistol damage", new Vector2(0, 0), new Vector2(195, 86), new Vector2(330, 44), 26);
        title.alignment = TextAlignmentOptions.Left;
        damageLabel = GreyboxUI.Label(ui, "DamagePreview", "", Vector2.zero, new Vector2(195, 47), new Vector2(330, 38), 22);
        damageLabel.alignment = TextAlignmentOptions.Left;
        damageLabel.color = GreyboxUI.Muted;
        upgradeButton = GreyboxUI.Button(ui, "Upgrade", new Vector2(1, 0), new Vector2(-142, 66), OnUpgradePressed, 24);
        ((RectTransform)upgradeButton.transform).sizeDelta = new Vector2(236, 84);
        upgradeLabel = upgradeButton.GetComponentInChildren<TMP_Text>();
        upgradeLabel.rectTransform.sizeDelta = new Vector2(216, 80);
        BindWallet();
        Refresh();
    }

    void OnEnable()
    {
        BindWallet();
    }

    void BindWallet()
    {
        if (wallet != null) wallet.Changed -= Refresh;
        wallet = Wallet.Instance;

        if (wallet == null)
        {
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
        if (wallet == null) return;
        coinsLabel.SetText("{0} coins", wallet.Coins);
        upgradeLabel.SetText("Upgrade\n{0} coins", wallet.NextCost);
        if (damageLabel != null) damageLabel.SetText("{0} to {1} per hit", (float)(1 + wallet.Level), (float)(2 + wallet.Level));
        upgradeButton.interactable = wallet.Coins >= wallet.NextCost;
        upgradeButton.GetComponent<Image>().color = upgradeButton.interactable
            ? new Color(0.29f, 0.25f, 0.16f) : new Color(0.1f, 0.12f, 0.13f);
    }

    public void OnUpgradePressed()
    {
        if (wallet != null && (SceneLoader.Instance == null || !SceneLoader.Instance.IsLoading)) wallet.TryUpgrade();
    }
}
