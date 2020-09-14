using System.Collections.Generic;

[System.Serializable]
public class ElementMap {
    private Dictionary<Element, int> values = new Dictionary<Element, int>();

    private SortedSet<StatusEffect> statusEffects = new SortedSet<StatusEffect>();

    public ElementMap() {}

    public ElementMap(Dictionary<Element, int> values, SortedSet<StatusEffect> statusEffects) {
        this.values = values;
        this.statusEffects = statusEffects;
    }

    public int GetBase(Element element) => values.ContainsKey(element) ? values[element] : 0;
    
    public int Get(Element element) {
        var value = values.ContainsKey(element) ? values[element] : 0;
        foreach (var statusEffect in statusEffects)
            if (statusEffect.element == element)
                value += statusEffect.value;

        return value;
    }

    public void Tick(Unit unit) {
        foreach (var statusEffect in statusEffects)
            statusEffect.Tick(unit);
    }

    public int this[Element key] {
        get => Get(key);
        set => values[key] = value;
    } 
}