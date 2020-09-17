using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    public Unit unit;

    private void Update() {
        if (unit == null || !unit || unit.Stats.health <= 0) {
            Destroy(gameObject);
            return;
        }

        transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + new Vector3(0, 0.5f));
        image.fillAmount = unit.Stats.health / (float) unit.Stats.maxHealth;
        text.text = $"{unit.Stats.health}/{unit.Stats.maxHealth}";
    }
}
