using UnityEngine;

public class ViewportPositioning : MonoBehaviour
{
    public Vector2 viewportPosition = new Vector2(0.5f, 0.1f); // (X=0.5, Y=0.1) means center-bottom

    void Start()
    {
        Camera cam = Camera.main;
        Vector3 worldPosition = cam.ViewportToWorldPoint(new Vector3(viewportPosition.x, viewportPosition.y, cam.nearClipPlane + 1));
        transform.position = worldPosition;
    }
}
