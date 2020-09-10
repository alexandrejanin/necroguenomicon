using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour {
    [SerializeField] private Image image;

    public Unit unit;

    private void Update() {
        if (unit == null || !unit) {
            Destroy(gameObject);
            return;
        }

        transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + new Vector3(0, -0.25f));
        image.fillAmount = unit.health / (float) unit.maxHealth;
    }
}
