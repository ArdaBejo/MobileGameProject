using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Run choices are intentionally scene-local; wallet purchases retain their existing save rules.
public sealed class RunProgression : MonoBehaviour
{
    public static RunProgression Instance { get; private set; }
    public bool Choosing { get; private set; }
    public int Level { get; private set; } = 1;
    public int NextLevelKills { get; private set; } = 5;
    public int PistolLevel { get; private set; } = 1;
    public int SwordLevel { get; private set; }
    public int RocketLevel { get; private set; }
    public int DamageLevel { get; private set; }
    public int SpeedLevel { get; private set; }
    public int ShieldLevel { get; private set; }
    public int ShieldCharges { get; private set; }
    public float SpeedMultiplier => 1 + SpeedLevel * 0.12f;
    public int BonusDamage => DamageLevel; // Might rank; each rank adds 20% damage.

    enum Upgrade { Pistol, Sword, Rocket, Damage, Speed, Shield }
    readonly Upgrade[] candidates = new Upgrade[6];
    readonly Upgrade[] choices = new Upgrade[3];
    readonly Button[] buttons = new Button[3];
    readonly TMP_Text[] labels = new TMP_Text[3];
    GreyboxLane lane;
    GameObject panel;
    float nextShield;

    void Awake()
    {
        Instance = this;
        lane = GetComponent<GreyboxLane>();
        var safe = GreyboxUI.Canvas("LevelUpCanvas", 115);
        safe.parent.SetParent(transform, false);
        panel = safe.gameObject;
        var backdrop = panel.AddComponent<Image>();
        backdrop.color = new Color(0.015f, 0.025f, 0.04f, 1f);
        var center = Vector2.one * 0.5f;
        GreyboxUI.Label(safe, "Title", "CHOOSE AN UPGRADE", center,
            new Vector2(0, 290), new Vector2(660, 75), 34);
        GreyboxUI.Label(safe, "Hint", "One choice for this run", center,
            new Vector2(0, 222), new Vector2(620, 45), 23);
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i] = GreyboxUI.Button(safe, "Upgrade " + i, center,
                new Vector2(0, 115 - i * 155), () => Choose(index), 25);
            var rect = (RectTransform)buttons[i].transform;
            rect.sizeDelta = new Vector2(620, 130);
            labels[i] = buttons[i].GetComponentInChildren<TMP_Text>();
            labels[i].rectTransform.sizeDelta = new Vector2(590, 118);
            buttons[i].GetComponent<Image>().color = new Color(0.09f, 0.21f, 0.25f);
        }
        panel.SetActive(false);
    }

    void Update()
    {
        if (lane == null || !lane.Playing) return;
        if (ShieldLevel > 0 && ShieldCharges == 0 && Time.time >= nextShield)
            ShieldCharges = 1;
        if (lane.Kills >= NextLevelKills && HasUpgrades()) OpenChoices();
    }

    bool HasUpgrades() => PistolLevel < 5 || SwordLevel < 5 || RocketLevel < 5
        || DamageLevel < 3 || SpeedLevel < 3 || ShieldLevel < 3;

    void OpenChoices()
    {
        int count = 0;
        if (SwordLevel < 5) candidates[count++] = Upgrade.Sword;
        if (RocketLevel < 5) candidates[count++] = Upgrade.Rocket;
        if (PistolLevel < 5) candidates[count++] = Upgrade.Pistol;
        if (DamageLevel < 3) candidates[count++] = Upgrade.Damage;
        if (SpeedLevel < 3) candidates[count++] = Upgrade.Speed;
        if (ShieldLevel < 3) candidates[count++] = Upgrade.Shield;

        // Offer both missing weapons first, then rotate the remaining eligible upgrades.
        int slots = 0;
        if (SwordLevel == 0) choices[slots++] = Upgrade.Sword;
        if (RocketLevel == 0) choices[slots++] = Upgrade.Rocket;
        for (int i = count - 1; i > 0; i--)
        {
            int other = Random.Range(0, i + 1);
            (candidates[i], candidates[other]) = (candidates[other], candidates[i]);
        }
        for (int i = 0; i < count && slots < choices.Length; i++)
        {
            bool duplicate = false;
            for (int j = 0; j < slots; j++) duplicate |= choices[j] == candidates[i];
            if (!duplicate) choices[slots++] = candidates[i];
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(i < slots);
            if (i < slots) labels[i].text = Describe(choices[i]);
        }
        Choosing = true;
        panel.SetActive(true);
        LifecycleGuard.SetGlobalPaused(true);
        Telemetry.Log("level_up", ("level", Level + 1), ("kills", lane.Kills));
    }

    string Describe(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case Upgrade.Pistol: return $"Pistol Lv {PistolLevel + 1}\nMore damage and faster shots";
            case Upgrade.Sword: return SwordLevel == 0 ? "Laser Sword\nUnlock a close-range energy sweep"
                : $"Laser Sword Lv {SwordLevel + 1}\nMore damage and a wider sweep";
            case Upgrade.Rocket: return RocketLevel == 0 ? "Rocket Launcher\nUnlock explosive automatic shots"
                : $"Rocket Launcher Lv {RocketLevel + 1}\nMore damage and a larger blast";
            case Upgrade.Damage: return $"Damage Up {DamageLevel + 1}/3\n+20% base damage to every weapon";
            case Upgrade.Speed: return $"Speed Up {SpeedLevel + 1}/3\n+12% movement speed";
            default: return $"Shield {ShieldLevel + 1}/3\nBlock one hit, recharge faster";
        }
    }

    void Choose(int index)
    {
        if (!Choosing || index < 0 || index >= choices.Length || !buttons[index].gameObject.activeSelf) return;
        var selected = choices[index];
        switch (selected)
        {
            case Upgrade.Pistol: PistolLevel++; break;
            case Upgrade.Sword: SwordLevel++; break;
            case Upgrade.Rocket: RocketLevel++; break;
            case Upgrade.Damage: DamageLevel++; break;
            case Upgrade.Speed: SpeedLevel++; break;
            case Upgrade.Shield:
                ShieldLevel++;
                ShieldCharges = 1;
                break;
        }
        Level++;
        NextLevelKills += 5 + (Level - 1) * 3;
        Choosing = false;
        panel.SetActive(false);
        Telemetry.Log("upgrade_selected", ("upgrade_id", selected.ToString()), ("run_level", Level),
            ("next_kills", NextLevelKills));
        LifecycleGuard.SetGlobalPaused(false);
    }

    public bool TryBlockDamage()
    {
        if (ShieldCharges == 0) return false;
        ShieldCharges = 0;
        nextShield = Time.time + 18 - ShieldLevel * 3;
        return true;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
