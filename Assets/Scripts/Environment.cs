using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment {
    public List<Unit> units = new List<Unit>();

    public Unit AddUnit(Unit unit) {
        units.Add(unit);
        unit.environment = this;
        return unit;
    }

    public Unit GetUnit(Vector2Int position) {
        foreach (var unit in units) {
            if (unit.Position == position)
                return unit;
        }
        return null;
    }
}
