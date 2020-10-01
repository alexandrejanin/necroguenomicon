using System.Collections;

public class MovementPhase : GamePhase {
    public MovementPhase(GameController controller) : base(controller) { }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase de déplacement";

        yield return controller.Player.PlayMovementPhase(controller);

        foreach (var enemy in controller.Environment.Enemies)
            yield return enemy.PlayMovementPhase(controller);

        controller.Phase = new AttackPhase(controller);
    }
}
