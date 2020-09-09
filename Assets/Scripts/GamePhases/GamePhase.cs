public abstract class GamePhase {
    public abstract string Text(GameController controller);

    public virtual void Start(GameController controller) { }

    public abstract GamePhase Update(GameController controller);
}
