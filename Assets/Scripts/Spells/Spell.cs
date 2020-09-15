using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class Spell {
    public Spell secondary;
    protected abstract string OwnName { get; }
    protected abstract Element Element { get; }

    public string Name => secondary == null ? OwnName : $"{OwnName} + {secondary.OwnName}"; 


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
        target.Damage(damage, Element, caster, name ?? OwnName);
    }
}
