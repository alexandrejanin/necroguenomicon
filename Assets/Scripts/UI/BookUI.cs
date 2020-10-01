using System.Linq;
using UnityEngine;

public class BookUI : MonoBehaviour {
    [SerializeField] private Spell[] spellFragments;

    [SerializeField] private SpellSlot[] spellFragmentSlots, spellSlots;

    [SerializeField] private SpellSlot primarySpellSlot, secondarySpellSlot, resultSpellSlot;

    public bool Finished { get; private set; }

    public void StartTurn() {
        Finished = false;

        foreach (var slot in spellFragmentSlots)
            slot.Spell = spellFragments[Random.Range(0, spellFragments.Length)];

        foreach (var slot in spellSlots)
            slot.Spell = null;

        primarySpellSlot.Spell = null;
        secondarySpellSlot.Spell = null;
        resultSpellSlot.Spell = null;
    }

    public void Validate() {
        FindObjectOfType<GameController>().Player.spells =
            spellSlots.Where(slot => slot.Spell != null).Select(slot => slot.Spell).ToList();
        gameObject.SetActive(false);
        Finished = true;
    }

    public void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void AddToCombinationSlot(SpellSlot spellSlot) {
        if (!spellSlot.Spell)
            return;
        if (primarySpellSlot.Spell == null) {
            primarySpellSlot.Spell = spellSlot.Spell;
            spellSlot.Spell = null;
        } else if (secondarySpellSlot.Spell == null) {
            secondarySpellSlot.Spell = spellSlot.Spell;
            spellSlot.Spell = null;
        }

        UpdateCombination();
    }

    private void UpdateCombination() =>
        resultSpellSlot.Spell = primarySpellSlot.Spell && secondarySpellSlot.Spell
            ? primarySpellSlot.Spell.WithSecondary(secondarySpellSlot.Spell)
            : null;

    public void RemoveFromCombineSlot(SpellSlot spellSlot) {
        if (!spellSlot.Spell)
            return;
        foreach (var fragmentSlot in spellFragmentSlots) {
            if (fragmentSlot.Spell == null) {
                fragmentSlot.Spell = spellSlot.Spell;
                spellSlot.Spell = null;
                UpdateCombination();
                return;
            }
        }
    }

    public void Equip(SpellSlot spellSlot) {
        if (!spellSlot.Spell)
            return;
        foreach (var slot in spellSlots) {
            if (slot.Spell == null) {
                slot.Spell = spellSlot.Spell;
                spellSlot.Spell = null;
                if (spellSlot == resultSpellSlot) {
                    primarySpellSlot.Spell = null;
                    secondarySpellSlot.Spell = null;
                }
                return;
            }
        }
    }

    public void Unequip(SpellSlot spellSlot) {
        if (!spellSlot.Spell)
            return;
        foreach (var slot in spellFragmentSlots) {
            if (slot.Spell == null) {
                if (spellSlot.Spell.Secondary != null) {
                    slot.Spell = spellSlot.Spell.Secondary;
                    spellSlot.Spell.secondary = null;
                } else {
                    slot.Spell = spellSlot.Spell;
                    spellSlot.Spell = null;
                    return;
                }
            }
        }
    }
}
