using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour {
    [SerializeField]
    private Image image;

    public Unit unit;

    private void Update () {
        if (!unit)
            Destroy(gameObject);

        transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + new Vector3(0, -0.25f));
        image.fillAmount = unit.health / (float) unit.maxHealth;
    }
}
