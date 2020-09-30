using UnityEngine;

public class PanCamera : MonoBehaviour {
    [SerializeField, Min(0)] private float speed = 5f;
    [SerializeField, Min(0)] private float minSize, maxSize, sizeSensitivity;

    private Vector3 lastMousePosition;

    private new Camera camera;

    private void Awake() => camera = GetComponent<Camera>();

    private void Update() {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position += Time.deltaTime * speed * Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += Time.deltaTime * speed * Vector3.right;
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += Time.deltaTime * speed * Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow))
            transform.position += Time.deltaTime * speed * Vector3.down;

        if (Input.GetMouseButtonDown(2)) {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) {
            transform.position += camera.ScreenToWorldPoint(lastMousePosition) -
                                  camera.ScreenToWorldPoint(Input.mousePosition);
            lastMousePosition = Input.mousePosition;
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - scroll * sizeSensitivity, minSize, maxSize);
    }
}
