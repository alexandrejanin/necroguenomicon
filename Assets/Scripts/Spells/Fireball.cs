using System;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell {
    protected override string OwnName => "Boule de Feu";
    protected override Element Element => Element.Fire;

    public override HashSet<Vector2Int> GetValidTargets(Unit caster) {
        return caster.environment.ManhattanRange(caster.Position, 5);
    }

    public override HashSet<Unit> PrimaryEffect(Unit caster, Vector2Int position) {
        var target = caster.environment.GetUnit(position);
        if (target == null)
            return null;

        Damage(target, 10, caster);

        var targets = new HashSet<Unit>{target};

        SecondaryEffect(caster, targets);

        return targets;
    }

    public override HashSet<Unit> SecondaryEffect(Unit caster, HashSet<Unit> targets) {
        foreach (var target in targets)
            target.AddTickerEffect(new TickerEffect(
                "En feu !",
                2,
                onTick: unit => Damage(unit, 2, caster, "En feu !")
            ));

        return targets;
    }
}
