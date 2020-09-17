using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Spell/Fireball")]
public class Fireball : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryDamage, secondaryDuration;

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
        foreach (var target in targets)
            target.AddTickerEffect(new TickerEffect(
                "En feu !",
                secondaryDuration.GetAmount(isSecondarySpell),
                onTick: unit => Damage(unit, secondaryDamage.GetAmount(isSecondarySpell), caster, "En feu !")
            ));

        return targets;
    }
}
