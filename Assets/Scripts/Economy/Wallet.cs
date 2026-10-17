using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    public int Coins { get; private set; }
    public int Level { get; private set; }

    public event System.Action Changed;
    public event System.Action<int> Earned;

    public int NextCost => (int)(100 * Mathf.Pow(1.5f, Level));

    void Awake()
    {
        Instance = this;
    }

    public void Earn(int n)
    {
        Coins += n;
        Earned?.Invoke(n);
        Changed?.Invoke();
    }

    public void Restore(int c, int l)
    {
        Coins = c;
        Level = l;
        Changed?.Invoke();
    }

    public bool TryUpgrade()
    {
        int cost = NextCost;
        if (Coins < cost)
            return false;

        Coins -= cost;
        Level++;
        Telemetry.Log("upgrade_purchased", ("upgrade_id", "damage"),
            ("level", Level), ("cost", cost), ("coins_left", Coins));
        Changed?.Invoke();

        return true;
    }
}
