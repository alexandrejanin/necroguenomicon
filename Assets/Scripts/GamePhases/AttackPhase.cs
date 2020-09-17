using System.Collections;
using UnityEngine;

public class AttackPhase : GamePhase {
    public AttackPhase(GameController controller) : base(controller) { }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase d'attaque";

        foreach (var unit in controller.Environment.units) {
            if (unit.Stats.health <= 0)
                continue;

            unit.StartOfTurn();
            
            if (unit.Stats.health <= 0)
                continue;
            
            yield return unit.StartCoroutine(unit.PlayAttackPhase(controller));

            unit.EndOfTurn();
        }

        foreach (var unit in controller.Environment.units)
            if (unit.Stats.health <= 0)
                Object.Destroy(unit);

        controller.Environment.units.RemoveAll(u => u.Stats.health <= 0);

        controller.Phase = new PreparationPhase(controller);
    }
}
