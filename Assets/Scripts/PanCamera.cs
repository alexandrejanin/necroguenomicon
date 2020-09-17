using UnityEngine;

public class PanCamera : MonoBehaviour {
    [SerializeField, Min(0)] private float speed = 5f;

    private Vector3 startingPosition;
    private Vector3 offset;

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
            startingPosition = transform.position;
            offset = new Vector3();
        }

        if (Input.GetMouseButton(2)) {
            var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            offset -= mousePos;
            transform.position = startingPosition + offset;
        }
    }
}
