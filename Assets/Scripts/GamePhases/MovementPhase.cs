using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementPhase : GamePhase {
    public MovementPhase(GameController controller) : base(controller) { }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase de déplacement";

        foreach (var unit in controller.Environment.units) {
            yield return unit.PlayMovementPhase(controller);
        }

        controller.Phase = new AttackPhase(controller);
    }
}
