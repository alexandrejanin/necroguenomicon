using UnityEngine;

public class AimCursor : MonoBehaviour {
    private void Update() {
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(
            Mathf.Floor(mousePositionInWorld.x) + 0.5f,
            Mathf.Floor(mousePositionInWorld.y) + 0.5f,
            1
        );
    }
}
