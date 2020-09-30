using UnityEngine;

public class PanCamera : MonoBehaviour {
    [SerializeField, Min(0)] private float speed = 5f;
    [SerializeField, Min(0)] private float minSize, maxSize, sizeSensitivity;

    private Vector2 lastMousePosition;

    private new Camera camera;

    private void Awake() => camera = GetComponent<Camera>();

    private void Update() {
        var mousePosition = Input.mousePosition;

        if (Input.GetMouseButton(1)) 
            transform.position += camera.ScreenToWorldPoint(lastMousePosition) -
                                  camera.ScreenToWorldPoint(mousePosition);
 
        lastMousePosition = mousePosition;

        var scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - scroll * sizeSensitivity, minSize, maxSize);
    }
}
