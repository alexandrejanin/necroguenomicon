using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {
    [SerializeField] private Text text;
    [SerializeField] private Image healthBar;

    private GameController gameController;

    private void Awake() {
        gameController = FindObjectOfType<GameController>();
    }

    private void Update() {
        if (!gameController.Player)
            return;

        var stats = gameController.Player.Stats;

        healthBar.fillAmount = stats.health / (float) stats.maxHealth;

        text.text = $"{stats.health} / {stats.maxHealth}";
    }
}
