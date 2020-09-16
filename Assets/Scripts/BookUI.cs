using UnityEngine;

public class BookUI : MonoBehaviour {
    [SerializeField]
    private Spell[] spellFragments;

    [SerializeField]
    private SpellSlot[] spellFragmentSlots, spellSlots;

    [SerializeField]
    private SpellSlot primarySpellSlot, secondarySpellSlot, resultSpellSlot;

    public void StartTurn() {
        foreach (var slot in spellFragmentSlots)
            slot.Spell = spellFragments[Random.Range(0, spellFragments.Length)];
    }

    public void Validate() {

    }

    public void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnFragmentSlotClicked(SpellSlot spellSlot) {
        if (primarySpellSlot.Spell == null) {
            primarySpellSlot.Spell = spellSlot.Spell;
            spellSlot.Spell = null;
        } else if (secondarySpellSlot.Spell == null) {
            secondarySpellSlot.Spell = spellSlot.Spell;
            spellSlot.Spell = null;
        }

        resultSpellSlot.Spell = primarySpellSlot.Spell?.WithSecondary(secondarySpellSlot.Spell);
    }

    public void OnCombinationSlotClicked(SpellSlot spellSlot) {
        foreach (var fragmentSlot in spellFragmentSlots) {
            if (fragmentSlot.Spell == null) {
                fragmentSlot.Spell = spellSlot.Spell;
                spellSlot.Spell = null;
            }
        }
    }

    public void OnResultSlotClicked(SpellSlot spellSlot) {
        foreach (var slot in spellSlots) {
            if (slot.Spell == null) {
                slot.Spell = spellSlot.Spell;
                spellSlot.Spell = null;
                primarySpellSlot.Spell = null;
                secondarySpellSlot.Spell = null;
            }
        }
    }

    public void OnSpellSlotClicked(SpellSlot spellSlot) {
    }
}
