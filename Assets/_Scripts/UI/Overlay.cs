using System;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    [SerializeField]
    public GameObject content;

    [SerializeField]
    public bool closeWithBackButton = true;


    [SerializeField]
    public OverlayAnimationData animationData = new OverlayAnimationData();


    private bool isClosing = false;

    //public void Open()
    //{
    //    if (animationData.useOpenAnimation)
    //        StartCoroutine(OpenAnimation());
    //}

    public virtual void Close()
    {
        if (isClosing)
        {
            Debug.Log("multiple close");
            return;
        }

        isClosing = true;

        ClientCoordinator.Instance.CloseOverlay(this);
    }

    public virtual void Close(Action onClosed)
    {
        if (isClosing)
        {
            Debug.Log("multiple close");
            return;
        }

        isClosing = true;

        ClientCoordinator.Instance.CloseOverlay(this, onClosed);
    }

}
