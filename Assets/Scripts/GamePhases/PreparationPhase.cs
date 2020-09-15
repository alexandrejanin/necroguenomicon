using System.Collections;
using UnityEngine;

public class PreparationPhase : GamePhase {
    public PreparationPhase(GameController controller) : base(controller) { }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase de préparation";

        controller.Player.Spells.Clear();
        controller.Player.Spells.Add(new Fireball() {secondary = new LightningArc()});
        controller.Player.Spells.Add(new LightningArc() {secondary = new Fireball()});

        yield return new WaitForSeconds(2f);

        controller.Phase = new MovementPhase(controller);
    }
}
