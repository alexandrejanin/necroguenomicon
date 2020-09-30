using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice Shard", menuName = "Spell/Ice Shard")]
public class IceShard : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryMpReduction;

    public override HashSet<Vector2Int> GetValidTargets(Unit caster) {
        return caster.environment.ManhattanRange(caster.Position, Range);
    }

    public override HashSet<Unit> PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell) {
        var target = caster.environment.GetUnit(position);
        if (target == null)
            return null;

        Damage(target, primaryDamage.GetAmount(isPrimarySpell), caster);

        return new HashSet<Unit> {target};
    }

    public override HashSet<Unit> SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell) {
        if (targets == null)
            return null;
        
        foreach (var target in targets)
            target.AddStatsEffect(new StatsEffect(
                "Gelé",
                2,
                stats => stats.movementPoints -= secondaryMpReduction.GetAmount(isSecondarySpell)
            ));

        return targets;
    }
}
