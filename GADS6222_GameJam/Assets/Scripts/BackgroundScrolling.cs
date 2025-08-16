using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public float speed = 100f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //move from right to left
        rectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        //reset background when its off-screen
        if (rectTransform.anchoredPosition.x <= -rectTransform.rect.width)
        {
            rectTransform.anchoredPosition = new Vector2(0, rectTransform.anchoredPosition.y);
        }
    }
}
