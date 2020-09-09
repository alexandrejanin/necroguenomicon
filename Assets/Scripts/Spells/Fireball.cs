using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell {
    public string Name => "Boule de feu";
    public override SortedSet<Unit> Apply(Unit caster, Vector2Int position) {
        var target = caster.environment.GetUnit(position);
        if (target == null) return null;

        var set = new SortedSet<Unit>(){ target };

        target.Damage(Element.Fire, 10);

        target.Effects.Add(new Effect(
            2,
            "En feu !",
            (stats) => stats.Damage(Element.Fire, 5)
        ));

        secondary?.Apply(caster, position);

        return set;
    }
}
