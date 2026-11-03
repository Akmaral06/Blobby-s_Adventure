using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform rect;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        Apply();
    }

    void Update()
    {
        if (Screen.safeArea != lastSafeArea)
            Apply();
    }

    void Apply()
    {
        lastSafeArea = Screen.safeArea;

        Vector2 anchorMin = lastSafeArea.position;
        Vector2 anchorMax = lastSafeArea.position + lastSafeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
    }
}