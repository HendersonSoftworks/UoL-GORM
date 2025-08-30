using UnityEngine;

public class AnimateCloud : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField]
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.Translate(Vector2.right * Time.deltaTime * 4);
        if (rectTransform.anchoredPosition.x > 662)
        {
            rectTransform.anchoredPosition = new Vector2(-662, rectTransform.anchoredPosition.y);
        }
    }
}
