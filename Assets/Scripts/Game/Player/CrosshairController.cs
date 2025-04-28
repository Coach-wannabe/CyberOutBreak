using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        Cursor.visible = false; // Hide default system cursor
    }

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 clampedPosition = ClampToScreen(mousePosition);
        transform.position = clampedPosition;
    }

    private Vector2 ClampToScreen(Vector2 position)
    {
        Vector3 viewportPosition = _camera.WorldToViewportPoint(position);

        viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
        viewportPosition.y = Mathf.Clamp01(viewportPosition.y);

        Vector2 clampedWorldPosition = _camera.ViewportToWorldPoint(viewportPosition);
        return clampedWorldPosition;
    }
}
