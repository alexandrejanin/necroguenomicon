using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPhase : GamePhase {
    private Spell spell = new Fireball();

    public override string Text(GameController controller) {
        var targetedTile = controller.GetTargetedTile();
        
        return $"Phase d'attaque : {spell.Name} ({targetedTile.x}, {targetedTile.y})";
    }

    public override void Start(GameController controller) {
        foreach (var unit in controller.Environment.units) {
            unit.StartOfTurn();
        }
    }

    public override GamePhase Update(GameController controller) {
        var targetedTile = controller.GetTargetedTile();

        if (Input.GetMouseButtonDown(0)) {
            var units = spell.Apply(controller.Player, targetedTile);
            return new MovementPhase();
        }

        return this;
    }
}
