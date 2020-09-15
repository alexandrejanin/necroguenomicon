using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningArc : Spell {

    protected override string OwnName => "Arc Électrique";
    protected override Element Element => Element.Lightning;

    public override HashSet<Vector2Int> GetValidTargets(Unit caster) {
        return caster.environment.ManhattanRange(caster.Position, 4);
    }

    public override HashSet<Unit> PrimaryEffect(Unit caster, Vector2Int position) {
        var target = caster.environment.GetUnit(position);
        if (target == null)
            return null;

        var targets = GetLightningTargets(target, 2, 3, null);

        foreach (var lightningTarget in targets)
            Damage(lightningTarget, 7, caster);

        return targets;
    }

    public override HashSet<Unit> SecondaryEffect(Unit caster, HashSet<Unit> primaryTargets) {
        var secondaryTargets = new HashSet<Unit>();
        
        foreach (var primaryTarget in primaryTargets) {
            var lightningTargets = GetLightningTargets(primaryTarget, 2, 3, new HashSet<Unit>(primaryTargets.Union(secondaryTargets)));
            secondaryTargets.UnionWith(lightningTargets);
        }

        foreach (var secondaryTarget in secondaryTargets)
            Damage(secondaryTarget, 2, caster);

        return secondaryTargets;
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
