using UnityEngine;

// Shared by combat and the HUD so previews always match the damage actually dealt.
public static class CombatBalance
{
    public const int MaxPaidLevels = 5;
    public static int Damage(int baseDamage, int mightRank) =>
        Mathf.RoundToInt(baseDamage * (1 + Mathf.Clamp(mightRank, 0, 3) * 0.2f));
    public static int Pistol(int rank, int paidRank, int mightRank) =>
        Damage(14 + (Mathf.Clamp(rank, 1, 5) - 1) * 4
            + Mathf.Clamp(paidRank, 0, MaxPaidLevels) * 2, mightRank);
    public static int Sword(int rank, int mightRank) => Damage(26 + (rank - 1) * 6, mightRank);
    public static int Rocket(int rank, int mightRank) => Damage(42 + (rank - 1) * 10, mightRank);
    public static int Contact(int wave) => 8 + Mathf.Clamp(wave, 1, 3) * 4;
    public static int EnemyHealth(int baseHealth, int wave) =>
        Mathf.RoundToInt(baseHealth * (1 + (Mathf.Clamp(wave, 1, 3) - 1) * 0.5f));
}
