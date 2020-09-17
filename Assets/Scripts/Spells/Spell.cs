using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class Spell : ScriptableObject {
    private Spell secondary;
    public Spell Secondary => secondary;

    [SerializeField] private string ownName;
    public string OwnName => ownName;
    public string FullName => secondary == null ? ownName : $"{ownName} + {secondary.ownName}";

    [SerializeField] private Element element;
    public Element Element => element;

    [SerializeField] private Color color;
    public Color Color => color;

    [SerializeField, Min(0)] private int range;
    public int Range => range;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    public abstract HashSet<Vector2Int> GetValidTargets(Unit caster);

    public abstract HashSet<Unit> PrimaryEffect(Unit caster, Vector2Int position);
    public abstract HashSet<Unit> SecondaryEffect(Unit caster, HashSet<Unit> primaryTargets);

    public virtual void Apply(Unit caster, Vector2Int position) {
        if (secondary == null) {
            PrimaryEffect(caster, position);
            return;
        }

        var primaryTargets = PrimaryEffect(caster, position);
        if (primaryTargets == null || primaryTargets.Count == 0)
            return;
        var secondaryTargets = secondary.SecondaryEffect(caster, primaryTargets);
        SecondaryEffect(caster, new HashSet<Unit>(secondaryTargets.Except(primaryTargets)));
    }

    protected void Damage(Unit target, int damage, Unit caster, string name = null) {
        target.Damage(damage, element, caster, name ?? ownName);
    }

    public Spell WithSecondary(Spell spell) {
        if (spell == null)
            return this;
        var newSpell = Instantiate(this);
        newSpell.secondary = spell;
        return newSpell;
    }
}
