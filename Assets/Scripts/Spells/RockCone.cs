using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rock Cone", menuName = "Spell/Rock Cone")]
public class RockCone : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryKnockback, secondaryDamage;

    public override HashSet<Vector2Int> GetValidTargets(Unit caster) {
        return caster.environment.ManhattanRange(caster.Position, Range);
    }

    public override IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, HashSet<Unit> targets) {
        var target = caster.environment.GetUnit(position);
        if (target == null)
            yield break;

        targets?.Add(target);

        Damage(target, primaryDamage.GetAmount(isPrimarySpell), caster);
    }

    public override IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, HashSet<Unit> secondaryTargets) {
        if (targets == null)
            yield break;
        
        foreach (var target in targets)
            target.KnockBack(caster.Position, secondaryKnockback.GetAmount(isSecondarySpell));
    }
}
