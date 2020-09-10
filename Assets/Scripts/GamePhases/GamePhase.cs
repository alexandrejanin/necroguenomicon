using System.Collections;

public abstract class GamePhase {
    protected GameController controller;

    protected GamePhase(GameController controller) {
        this.controller = controller;
    }

    public abstract IEnumerator Run();
}
