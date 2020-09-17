using System.Collections.Generic;

[System.Serializable]
public class UnitStats {
    public int maxHealth;
    public int health;
    public int movementPoints;
    public Dictionary<Element, int> resistances, affinities;

    public int GetResistance(Element e) => resistances != null && resistances.ContainsKey(e) ? resistances[e] : 0;

    public int GetAffinity(Element e) => affinities != null && affinities.ContainsKey(e) ? affinities[e] : 0;

    public void AddResistance(Element e, int r) => resistances[e] = GetResistance(e) + r;

    public void AddAffinity(Element e, int r) => affinities[e] = GetAffinity(e) + r;

    public UnitStats(UnitStats stats) {
        maxHealth = stats.maxHealth;
        health = stats.health;
        movementPoints = stats.movementPoints;
        if (stats.resistances != null)
            resistances = new Dictionary<Element, int>(stats.resistances);
        if (stats.affinities != null)
            affinities = new Dictionary<Element, int>(stats.affinities);
    }
}
