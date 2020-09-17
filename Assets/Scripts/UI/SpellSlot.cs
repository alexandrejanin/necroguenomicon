using UnityEngine;

public class SpellSlot : MonoBehaviour {
    [SerializeField] private SpellCard spellCard;

    public Spell Spell {
        get => spellCard.Spell;
        set => spellCard.Spell = value;
    }
}
