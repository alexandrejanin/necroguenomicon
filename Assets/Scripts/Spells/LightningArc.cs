﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Arc", menuName = "Spell/Lightning Arc")]
public class LightningArc : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryDamage;

    public override HashSet<Vector2Int> GetValidTargets(Unit caster) {
        return caster.environment.ManhattanRange(caster.Position, Range);
    }

    public override HashSet<Unit> PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell) {
        var target = caster.environment.GetUnit(position);
        if (target == null)
            return null;

        var targets = GetLightningTargets(target, 2, 3, null);

        foreach (var lightningTarget in targets)
            Damage(lightningTarget, primaryDamage.GetAmount(isPrimarySpell), caster);

        return targets;
    }

    public override HashSet<Unit> SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell) {
        if (targets == null)
            return null;
        
        var secondaryTargets = new HashSet<Unit>();

        foreach (var primaryTarget in targets) {
            var lightningTargets = GetLightningTargets(primaryTarget, 2, 3,
                new HashSet<Unit>(targets.Union(secondaryTargets)));
            secondaryTargets.UnionWith(lightningTargets);
        }

        foreach (var secondaryTarget in secondaryTargets)
            Damage(secondaryTarget, secondaryDamage.GetAmount(isSecondarySpell), caster);

        return secondaryTargets;
    }

    private HashSet<Unit> GetLightningTargets(
        Unit primaryTarget, int maxRange, int maxTargets, HashSet<Unit> excluded
    ) {
        var currentTargets = new HashSet<Unit> {primaryTarget};

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
        } while (newTargets.Count > 0);

        return currentTargets;
    }
}
