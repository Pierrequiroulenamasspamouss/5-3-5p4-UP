using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class ForceHorizontalLayoutRuntime : MonoBehaviour
{
    private RectTransform rectTransform;
    private HorizontalLayoutGroup horizontalLayoutGroup;
    private ContentSizeFitter contentSizeFitter;
    private LayoutElement layoutElement;

    [SerializeField] private bool forceEveryFrame = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        layoutElement = GetComponent<LayoutElement>();
    }

    private void OnEnable()
    {
        ApplyLayout();
    }

    private void Update()
    {
        if (forceEveryFrame)
        {
            ApplyLayout();
        }
    }

    public void ApplyLayout()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (horizontalLayoutGroup == null)
            horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();

        if (horizontalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        if (contentSizeFitter != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        if (layoutElement != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        Canvas.ForceUpdateCanvases();
    }

    public void ForceOnce()
    {
        ApplyLayout();
    }
}