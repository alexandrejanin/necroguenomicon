using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Unit {
    public List<Spell> spells = new List<Spell>();

    public override IEnumerator PlayMovementPhase(GameController controller) {
        yield return FindObjectOfType<PanCamera>().MoveTo(Position);
        var movementTiles = GetMovementTiles();

        controller.TileDrawer.DrawTiles(movementTiles.Keys);

        Vector2Int targetedTile;
        do {
            targetedTile = controller.GetTargetedTile();
            if (movementTiles.ContainsKey(targetedTile))
                controller.TileDrawer.DrawTiles(movementTiles[targetedTile]);
            else
                controller.TileDrawer.DrawTiles(movementTiles.Keys);
            yield return null;
        } while (!Input.GetMouseButtonDown(0) || !movementTiles.Keys.Contains(targetedTile));

        controller.TileDrawer.Clear();

        yield return Move(movementTiles[targetedTile]);
    }

    public override IEnumerator PlayAttackPhase(GameController controller) {
        if (spells == null || spells.Count == 0)
            yield break;

        yield return FindObjectOfType<PanCamera>().MoveTo(Position);

        foreach (var spell in spells) {
            controller.PhaseText.text = spell.FullName;
            var validTargets = spell.GetValidTargets(this);
            controller.TileDrawer.DrawTiles(validTargets);

            Vector2Int targetedTile;
            do {
                targetedTile = controller.GetTargetedTile();
                yield return null;
            } while (!Input.GetMouseButtonDown(0) || !validTargets.Contains(targetedTile));

            controller.TileDrawer.Clear();

            yield return spell.Apply(this, targetedTile);
        }
    }
}
