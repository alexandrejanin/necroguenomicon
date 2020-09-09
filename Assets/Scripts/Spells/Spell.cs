using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Spell {
    public string Name { get; }
    public Spell secondary;

    public abstract SortedSet<Unit> Apply(Unit caster, Vector2Int position);
}
