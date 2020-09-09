using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPhase : GamePhase {
    public override string Text(GameController controller) {
        var movementTiles = controller.Player.GetMovementTiles();
        var targetedTile = controller.GetTargetedTile();
        if(movementTiles.ContainsKey(targetedTile))
            return $"Phase de déplacement ({targetedTile.x}, {targetedTile.y}) {movementTiles[targetedTile].Count}PM";
        
        return $"Phase de déplacement ({targetedTile.x}, {targetedTile.y})";
    }

    public override GamePhase Update(GameController controller) {
        var movementTiles = controller.Player.GetMovementTiles();
        var targetedTile = controller.GetTargetedTile();

        if (Input.GetMouseButtonDown(0)) {
            if (movementTiles.ContainsKey(targetedTile)) {
                controller.Player.Move(movementTiles[targetedTile]);

                foreach (var unit in controller.Environment.units) {
                    if (unit == controller.Player) continue;
                    var tiles = unit.GetMovementTiles();
                    var paths = new List<List<Vector2Int>>(tiles.Values);
                    if (paths.Count > 0) {
                        var path = paths[Random.Range(0, paths.Count)];
                        unit.Move(path);
                    }
                }

                return new AttackPhase();
            }
        }

        return this;
    }
}
