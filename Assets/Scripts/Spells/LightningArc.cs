using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Arc", menuName = "Spell/Lightning Arc")]
public class LightningArc : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryDamage;

    public override IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, System.Func<HashSet<Unit>, IEnumerator> then) {
        var target = caster.environment.GetUnit(position);
        if (target == null)
            yield break;

        var targets = GetLightningTargets(target, 2, 3, null);

        foreach (var lightningTarget in targets)
            Damage(lightningTarget, primaryDamage.GetAmount(isPrimarySpell), caster);

        then(targets);
    }

    public override IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, System.Func<HashSet<Unit>, IEnumerator> then = null) {
        var newTargets = new HashSet<Unit>(targets);

        foreach (var primaryTarget in targets) {
            var lightningTargets = GetLightningTargets(primaryTarget, 2, 3, newTargets);
            newTargets.UnionWith(lightningTargets);
        }

        foreach (var target in newTargets)
            Damage(target, secondaryDamage.GetAmount(isSecondarySpell), caster);

        if (then != null)
            yield return then(targets);
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
