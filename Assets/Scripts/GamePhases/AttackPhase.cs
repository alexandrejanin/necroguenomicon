using System.Collections;
using UnityEngine;

public class AttackPhase : GamePhase {
    public AttackPhase(GameController controller) : base(controller) { }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase d'attaque";

        foreach (var unit in controller.Environment.units) {
            if (unit.Health <= 0)
                continue;

            unit.StartOfTurn();
            
            if (unit.Health <= 0)
                continue;
            
            yield return unit.StartCoroutine(unit.PlayAttackPhase(controller));
        }

        foreach (var unit in controller.Environment.units)
            if (unit.Health <= 0)
                Object.Destroy(unit);

        controller.Environment.units.RemoveAll(u => u.Health <= 0);

        controller.Phase = new PreparationPhase(controller);
    }
}
