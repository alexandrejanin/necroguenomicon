public abstract class Effect {
    private string name;
    public string Name => name;

    protected int turns;

    public Effect(string name, int turns) {
        this.name = name;
        this.turns = turns;
    }

    public virtual bool Tick(Unit unit) {
        turns--;
        return turns <= 0;
    }
}
