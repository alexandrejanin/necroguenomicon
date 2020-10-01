using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour {
    [SerializeField] private SpellCard spellCard;

    public Spell Spell {
        get => spellCard.Spell;
        set => spellCard.Spell = value;
    }

    private void Update () {
        foreach (Transform child in transform) 
            if (child.GetComponent<Button>())
                child.gameObject.SetActive(Spell != null);
        
    }
}
