public class TickerEffect : Effect {
    private System.Action<Unit> onTick;

    public TickerEffect(string name, int turns, System.Action<Unit> onTick) : base(name, turns) {
        this.onTick = onTick;
    }

    public override bool Tick(Unit unit) {
        onTick(unit);
        turns--;
        return turns <= 0;
    }
}
