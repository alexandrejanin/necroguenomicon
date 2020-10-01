using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Spell : ScriptableObject {
    public Spell secondary;
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

    public virtual HashSet<Vector2Int> GetValidTargets(Unit caster) => Environment.ManhattanRange(caster.Position, Range);

    public virtual HashSet<Vector2Int> GetTargetedTiles(Unit caster, Vector2Int position) =>
        new HashSet<Vector2Int> {position};

    public abstract IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, System.Func<HashSet<Unit>, IEnumerator> then);
    public abstract IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, System.Func<HashSet<Unit>, IEnumerator> then = null);

    public IEnumerator Apply(Unit caster, Vector2Int position) {
        if (secondary == null) {
            yield return PrimaryEffect(caster, position, false, targets => SecondaryEffect(caster, targets, false));
        } else {
            yield return PrimaryEffect(caster, position, true,
                targets => secondary.SecondaryEffect(caster, targets, true,
                        secondaryTargets => SecondaryEffect(caster, secondaryTargets, false)
                )
            );
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
