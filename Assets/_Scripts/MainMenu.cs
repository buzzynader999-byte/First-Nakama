using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;

        public void DeActive()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        }
    }
}