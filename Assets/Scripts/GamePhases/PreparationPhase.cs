using System.Collections;
using UnityEngine;

public class PreparationPhase : GamePhase {
    private BookUI book;

    public PreparationPhase(GameController controller) : base(controller) {
        book = GameController.FindObjectOfType<BookUI>();
     }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase de préparation";

        controller.Player.Spells.Clear();

        book.gameObject.SetActive(true);
        book.StartTurn();

        yield return new WaitForSeconds(2f);

        controller.Phase = new MovementPhase(controller);
    }
}
