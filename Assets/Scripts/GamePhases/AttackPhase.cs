using UnityEngine;

public class AttackPhase : GamePhase {
    private Spell spell = new Fireball();

    public override string Text(GameController controller) => "Phase d'attaque";

    public override void Start(GameController controller) {
        foreach (var unit in controller.Environment.units) {
            unit.StartOfTurn();
        }
    }

    public override GamePhase Update(GameController controller) {
        var targetedTile = controller.GetTargetedTile();

        controller.TileDrawer.DrawTiles(spell.GetValidTargets(controller.Player));

        if (Input.GetMouseButtonDown(0)) {
            if (spell.GetValidTargets(controller.Player).Contains(targetedTile)) {
                controller.TileDrawer.Clear();

                spell.Apply(controller.Player, targetedTile);
                return new MovementPhase();
            }
        }

        return this;
    }
}
