using UnityEngine;
using TMPro;

public class FollowEnemyName : MonoBehaviour
{
    public Transform enemy; // Gán enemy (hoặc empty object đặt trên đầu enemy)
    public Vector3 offset; // Điều chỉnh để label nằm trên đầu

    private Camera cam;
    private RectTransform rectTransform;

    void Start()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (enemy == null || cam == null) return;

        // Chuyển vị trí 3D sang tọa độ UI
        Vector3 screenPos = cam.WorldToScreenPoint(enemy.position + offset);
        rectTransform.position = screenPos;
    }
}
