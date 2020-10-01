using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rock Cone", menuName = "Spell/Rock Cone")]
public class RockCone : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryKnockback, secondaryDamage;

    public override IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, System.Func<HashSet<Unit>, IEnumerator> then) {
        // TODO make rock cone an AoE
        var target = caster.environment.GetUnit(position);
        if (target == null)
            yield break;

        Damage(target, primaryDamage.GetAmount(isPrimarySpell), caster);
        yield return then(new HashSet<Unit>{target});
    }

    public override IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, System.Func<HashSet<Unit>, IEnumerator> then = null) {
        var newTargets = new HashSet<Unit>(targets);
        foreach (var target in targets) {
            newTargets.UnionWith(target.KnockBack(caster.Position, secondaryKnockback.GetAmount(isSecondarySpell)));
            while (Vector3.Distance(target.transform.position, target.WorldPosition) > 0.1f)
                yield return null;
        }

        if (then != null)
            yield return then(targets);
    }
}
