[System.Serializable]
public struct SpellStat {
    public int baseAmount;
    public int bonusAmount;

    public int GetAmount(bool useBonus) => useBonus ? baseAmount + bonusAmount : baseAmount;
}
