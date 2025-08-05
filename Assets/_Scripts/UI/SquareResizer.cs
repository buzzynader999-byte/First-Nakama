using UnityEngine;

namespace _Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SquareResizer : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_parentRectTransform == null) return;

            Vector2 parentSize = _parentRectTransform.rect.size;
            float minDimension = Mathf.Min(parentSize.x, parentSize.y);
            _rectTransform.sizeDelta = new Vector2(minDimension, minDimension);
        }
    }
}