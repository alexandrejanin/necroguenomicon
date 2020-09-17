using UnityEngine;
using UnityEngine.UI;

public class SpellCard : MonoBehaviour {
    [SerializeField]
    private SpellCard secondaryCard;

    [SerializeField]
    private Image background, sprite;

    [SerializeField]
    private Text titleText, primaryText, secondaryText, rangeText;

    private Spell spell;
    public Spell Spell {
        get => spell;
        set {
            spell = value;
            gameObject.SetActive(spell != null);
            if (value != null)
                UpdateAppearance();
        }
    }

    private void UpdateAppearance() {
        titleText.text = spell.name;
        rangeText.text = spell.Range.ToString();
        background.color = spell.Color;
        sprite.sprite = spell.Sprite;

        if (secondaryCard != null)
            secondaryCard.Spell = spell.Secondary;
    }
}
