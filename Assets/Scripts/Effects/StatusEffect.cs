public class StatusEffect : Effect{
    public Element element;
    public int value;

    public StatusEffect(string name, int turns, Element element, int value) : base(name, turns) {
        this.element = element;
        this.value = value;
    }
}
