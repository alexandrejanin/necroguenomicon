using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    public Unit unit;

    private void Update() {
        if (unit == null || !unit || unit.Health <= 0) {
            Destroy(gameObject);
            return;
        }

        transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + new Vector3(0, -0.25f));
        image.fillAmount = unit.Health / (float) unit.MaxHealth;
        text.text = $"{unit.Health}/{unit.MaxHealth}";
    }
}
