using System.Collections;
using UnityEngine;

public class Enemy : Unit {
    public override IEnumerator PlayMovementPhase(GameController controller) {
        var path = controller.Environment.GetPath(Position, controller.Player.Position);

        if (path == null || path.Count == 0)
            yield break;

        path.RemoveRange(Stats.movementPoints, path.Count - Stats.movementPoints);

        if (path.Count == 0)
            yield break;

        yield return FindObjectOfType<PanCamera>().MoveTo(Position);

        yield return Move(path);
    }

    public override IEnumerator PlayAttackPhase(GameController controller) {
        if (Util.ManhattanDistance(Position, controller.Player.Position) <= 1) {
            // yield return Object.FindObjectOfType<PanCamera>().MoveTo(Position);
            controller.Player.Damage(10, Element.Physical, this, "Coup de poing");
        }

        yield break;
    }
}
