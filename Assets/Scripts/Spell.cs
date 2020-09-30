using System;
using System.Collections;
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
    public virtual HashSet<Vector2Int> GetTargetedTiles(Unit caster, Vector2Int position) => new HashSet<Vector2Int> {position};

    public abstract IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, HashSet<Unit> targets);
    public abstract IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, HashSet<Unit> secondaryTargets);

    public IEnumerator Apply(Unit caster, Vector2Int position) {
        if (secondary == null) {
            var targets = new HashSet<Unit>();
            yield return PrimaryEffect(caster, position, false, targets);
            yield return SecondaryEffect(caster, targets, false, null);
        } else {
            var targets = new HashSet<Unit>();
            yield return PrimaryEffect(caster, position, true, targets);
            if (targets.Count == 0)
                yield break;
            var secondaryTargets = new HashSet<Unit>(targets);
            yield return secondary.SecondaryEffect(caster, targets, true, secondaryTargets);
            yield return SecondaryEffect(caster, secondaryTargets, false, null);
        }
    }

    protected void Damage(Unit target, int damage, Unit caster, string sourceName = null) {
        target.Damage(damage, element, caster, sourceName ?? name);
    }

    public Spell WithSecondary(Spell spell) {
        if (spell == null)
            return this;
        var newSpell = Instantiate(this);
        newSpell.name = name;
        newSpell.secondary = spell;
        return newSpell;
    }
}
