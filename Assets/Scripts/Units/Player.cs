using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Unit {
    private List<Spell> spells = new List<Spell>();
    public List<Spell> Spells => spells;
    
    public override IEnumerator PlayMovementPhase(GameController controller) {
        var movementTiles = GetMovementTiles();

        controller.TileDrawer.DrawTiles(movementTiles.Keys);

        Vector2Int targetedTile;
        do {
            targetedTile = controller.GetTargetedTile();
            yield return null;
        } while (!Input.GetMouseButtonDown(0) || !movementTiles.Keys.Contains(targetedTile));

        controller.TileDrawer.Clear();

        yield return StartCoroutine(Move(movementTiles[targetedTile]));
    }

    public override IEnumerator PlayAttackPhase(GameController controller) {
        foreach (var spell in spells) {
            controller.PhaseText.text = spell.Name;
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

}