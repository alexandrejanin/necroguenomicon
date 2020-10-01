using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice Shard", menuName = "Spell/Ice Shard")]
public class IceShard : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryMpReduction;

    public override IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, System.Func<HashSet<Unit>, IEnumerator> then) {
        var target = caster.environment.GetUnit(position);
        if (target == null)
            yield break;

        Damage(target, primaryDamage.GetAmount(isPrimarySpell), caster);

        then(new HashSet<Unit>{target});
    }

    public override IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, System.Func<HashSet<Unit>, IEnumerator> then = null) {
        foreach (var target in targets)
            target.AddStatsEffect(new StatsEffect(
                "Gelé",
                2,
                stats => stats.movementPoints -= secondaryMpReduction.GetAmount(isSecondarySpell)
            ));
        
        if (then != null)
            yield return then(targets);
    }
}
