using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect {
    private string name;
    private System.Action<Unit> apply;
    private int turns;

    public Effect(int turns, string name, System.Action<Unit> apply) {
        this.turns = turns;
        this.name = name;
        this.apply = apply;
    }

    public bool Apply(Unit unit) {
        apply(unit);
        turns--;
        return turns <= 0;
    }
}
