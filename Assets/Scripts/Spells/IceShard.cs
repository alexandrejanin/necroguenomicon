﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice Shard", menuName = "Spell/Ice Shard")]
public class IceShard : Spell {
    [SerializeField]
    private SpellStat primaryDamage, secondaryMpReduction;

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

    public override HashSet<Unit> SecondaryEffect(Unit caster, HashSet<Unit> primaryTargets, bool isSecondarySpell) {
        foreach (var target in primaryTargets)
            target.AddStatsEffect(new StatsEffect(
                "Gelé",
                2,
                stats => stats.movementPoints -= secondaryMpReduction.GetAmount(isSecondarySpell)
            ));

        return primaryTargets;
    }

    private HashSet<Unit> GetLightningTargets(Unit primaryTarget, int maxRange, int maxTargets, HashSet<Unit> excluded) {
        var currentTargets = new HashSet<Unit>{primaryTarget};

        var newTargets = new HashSet<Unit>();
        do {
            newTargets.Clear();
            foreach (var target in currentTargets) {
                foreach (var unit in primaryTarget.environment.units) {
                    if (
                        !currentTargets.Contains(unit)
                        && (excluded == null || !excluded.Contains(unit))
                        && Util.ManhattanDistance(target.Position, unit.Position) <= maxRange
                        && currentTargets.Count + newTargets.Count < maxTargets
                    )
                        newTargets.Add(unit);
                }
            }
            currentTargets.UnionWith(newTargets);
        } while(newTargets.Count > 0);

        return currentTargets;
    }
}
