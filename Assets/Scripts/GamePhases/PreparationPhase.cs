using System.Collections;
using UnityEngine;

public class PreparationPhase : GamePhase {
    private BookUI book;

    public PreparationPhase(GameController controller) : base(controller) {
        book = Object.FindObjectOfType<BookUI>(true);
    }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase de préparation";

        controller.Player.spells.Clear();

        yield return Object.FindObjectOfType<PanCamera>().MoveTo(controller.Player.Position);

        book.gameObject.SetActive(true);
        book.StartTurn();

        while (!book.Finished)
            yield return null;

        controller.Phase = new MovementPhase(controller);
    }
}
