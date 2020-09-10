using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {
    public override IEnumerator PlayMovementPhase(GameController controller) {
        var tiles = GetMovementTiles();

        if (tiles.Count <= 0)
            yield break;

        var minDist = 10000;
        List<Vector2Int> minPath = null;

        foreach (var kvp in tiles) {
            var dist = Util.ManhattanDistance(kvp.Key, controller.Player.Position); 
            if (dist < minDist) {
                minDist = dist;
                minPath = kvp.Value;
            }
        }

        yield return StartCoroutine(Move(minPath));
    }

    public override IEnumerator PlayAttackPhase(GameController controller) {
        if (Util.ManhattanDistance(Position, controller.Player.Position) <= 1) {
            controller.Player.Damage(Element.Physical, 10);
        }
        yield break;
    }

}