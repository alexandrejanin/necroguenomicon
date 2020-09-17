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

        book.gameObject.SetActive(true);
        book.StartTurn();

        yield return new WaitForSeconds(2f);

        controller.Phase = new MovementPhase(controller);
    }
}
