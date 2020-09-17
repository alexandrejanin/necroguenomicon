using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Spell : ScriptableObject {
    private Spell secondary;
    public Spell Secondary => secondary;

    public string FullName => secondary == null ? name : $"{name} + {secondary.name}";

    [SerializeField] private Element element;
    public Element Element => element;

    [SerializeField] private Color color;
    public Color Color => color;

    [SerializeField, Min(0)] private int range;
    public int Range => range;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    public abstract HashSet<Vector2Int> GetValidTargets(Unit caster);

    public abstract HashSet<Unit> PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell);
    public abstract HashSet<Unit> SecondaryEffect(Unit caster, HashSet<Unit> primaryTargets, bool isSecondarySpell);

    public virtual void Apply(Unit caster, Vector2Int position) {
        if (secondary == null) {
            var targets = PrimaryEffect(caster, position, false);
            SecondaryEffect(caster, targets, false);
            return;
        }

        var primaryTargets = PrimaryEffect(caster, position, true);
        if (primaryTargets == null || primaryTargets.Count == 0)
            return;
        var secondaryTargets = secondary.SecondaryEffect(caster, primaryTargets, true);
        SecondaryEffect(caster, secondaryTargets, false);
    }

    protected void Damage(Unit target, int damage, Unit caster, string sourceName = null) {
        target.Damage(damage, element, caster, sourceName ?? name);
    }

    public Spell WithSecondary(Spell spell) {
        if (spell == null)
            return this;
        var newSpell = Instantiate(this);
        newSpell.secondary = spell;
        return newSpell;
    }
}
