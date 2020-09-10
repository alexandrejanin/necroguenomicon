using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : Unit {
    private Spell spell = new Fireball();
    
    public override IEnumerator PlayMovementPhase(GameController controller) {
        var movementTiles = GetMovementTiles();

        controller.TileDrawer.DrawTiles(movementTiles.Keys.ToList());

        Vector2Int targetedTile;
        do {
            targetedTile = controller.GetTargetedTile();
            yield return null;
        } while (!Input.GetMouseButtonDown(0) || !movementTiles.Keys.Contains(targetedTile));

        controller.TileDrawer.Clear();

        yield return StartCoroutine(Move(movementTiles[targetedTile]));
    }

    public override IEnumerator PlayAttackPhase(GameController controller) {
        var validTargets = spell.GetValidTargets(this);
        controller.TileDrawer.DrawTiles(validTargets);

        Vector2Int targetedTile;
        do {
            targetedTile = controller.GetTargetedTile();
            yield return null;
        } while (!Input.GetMouseButtonDown(0) || !validTargets.Contains(targetedTile));

        controller.TileDrawer.Clear();

        spell.Apply(this, targetedTile);
    }

}