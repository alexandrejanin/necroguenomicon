using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Spell {
    public abstract string Name { get; }

    public Spell secondary;

    public abstract List<Vector2Int> GetValidTargets(Unit caster);

    public abstract List<Unit> Apply(Unit caster, Vector2Int position);
}
