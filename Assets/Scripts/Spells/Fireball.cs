using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell {
    public override string Name => "Boule de feu";

    public override List<Vector2Int> GetValidTargets(Unit caster) {
        return caster.environment.ManhattanRange(caster.Position, 4);
    }

    public override List<Unit> Apply(Unit caster, Vector2Int position) {
        var target = caster.environment.GetUnit(position);
        if (target == null) return null;

        var list = new List<Unit> {target};

        target.Damage(Element.Fire, 10);

        target.Effects.Add(new Effect(
            2,
            "En feu !",
            (stats) => stats.Damage(Element.Fire, 5)
        ));

        secondary?.Apply(caster, position);

        return list;
    }
}
