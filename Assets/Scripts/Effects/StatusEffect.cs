﻿using System;

public class StatsEffect : Effect{
    public readonly System.Action<UnitStats> apply;

    public StatsEffect(string name, int turns, Action<UnitStats> apply) : base(name, turns) {
        this.apply = apply;
    }
}
