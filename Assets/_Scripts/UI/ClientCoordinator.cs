using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class ClientCoordinator : MonoBehaviour
{
    #region Singleton

    private static ClientCoordinator instance = null;

    public static ClientCoordinator Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion // Singleton

    public List<string> ShownPopups { get; set; } = new List<string>();


    [SerializeField] Canvas panelCanvas;

    #region Overlay

    public List<Overlay> overlayList = new List<Overlay>();

    public T OpenOverlay<T>(bool hasCanvas = false, bool showOnTop = false) where T : Overlay
    {
        overlayList.RemoveAll((t) => !t);

        GameObject currentOverlayGameobject;
        if (hasCanvas)
        {
            currentOverlayGameobject = Addressables.InstantiateAsync(typeof(T).Name).WaitForCompletion();
        }
        else
        {
            currentOverlayGameobject =
                Addressables.InstantiateAsync(typeof(T).Name, panelCanvas.transform).WaitForCompletion();
        }

        Overlay overlay = currentOverlayGameobject.GetComponent<T>();

        if (overlayList.Count != 0 && !showOnTop)
        {
            if (overlay.content)
                overlay.content.SetActive(false);
            else
                currentOverlayGameobject.SetActive(false);
        }
        else
            StartCoroutine(OpenAnimation(overlay));

        if (showOnTop)
            overlayList.Insert(0, overlay);
        else
            overlayList.Add(overlay);

        return overlay as T;
    }

    public void CloseOverlay(Overlay overlay, Action onClosed = null)
    {
        if (!overlayList.Contains(overlay))
            return;

        overlayList.Remove(overlay);

        StartCoroutine(CloseAnimation(overlay, () =>
        {
            onClosed?.Invoke();

            Addressables.ReleaseInstance(overlay.gameObject);

            if (overlayList.Count != 0)
            {
                bool wasActive = (overlayList[0].content != null && overlayList[0].content.activeSelf) ||
                                 (overlayList[0].content == null && overlayList[0].gameObject.activeSelf);

                if (overlayList[0].content != null)
                    overlayList[0].content.SetActive(true);
                else
                    overlayList[0].gameObject.SetActive(true);

                if (!wasActive)
                    StartCoroutine(OpenAnimation(overlayList[0]));
            }
        }));
    }

    public void LoseFocus(Overlay overlay)
    {
        if (overlayList.Count <= 1)
            return;

        if (overlayList.Contains(overlay))
        {
            overlayList.Remove(overlay);
            overlayList.Add(overlay);


            if (overlay.content != null)
                overlay.content.SetActive(false);
            else
                overlay.gameObject.SetActive(false);

            bool wasActive = (overlayList[0].content != null && overlayList[0].content.activeSelf) ||
                             (overlayList[0].content == null && overlayList[0].gameObject.activeSelf);

            if (overlayList[0].content != null)
                overlayList[0].content.SetActive(true);
            else
                overlayList[0].gameObject.SetActive(true);

            if (!wasActive)
                StartCoroutine(OpenAnimation(overlayList[0]));

            Debug.LogError("Moved");
        }
    }

    public Overlay GetFocusedOverlay()
    {
        return overlayList[0];
    }

    public int GetOverlayCount()
    {
        return overlayList.Count;
    }

    public void Clear()
    {
        foreach (Overlay overlay in overlayList)
        {
            try
            {
                Addressables.ReleaseInstance(overlay.gameObject);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        overlayList = new List<Overlay>();
    }

    #endregion


    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (overlayList.Count > 0)
            {
                if (overlayList[0].closeWithBackButton)
                    overlayList[0].Close();
                return;
            }
        }
    }

    #region animations

    private IEnumerator OpenAnimation(Overlay overlay, Action onComplete = null)
    {
        OverlayAnimationData animationData = overlay.animationData;
        if (!animationData.useOpenAnimation)
        {
            onComplete?.Invoke();
            yield break;
        }

        if (animationData.openElements != null)
        {
            for (int i = 0; i < animationData.openElements.Length; i++)
            {
                animationData.openElements[i].transform.localScale = Vector3.zero;
            }

            for (int i = 0; i < animationData.openElements.Length; i++)
            {
                LeanTween.scale(animationData.openElements[i], Vector3.one,
                        animationData.openDurations[Mathf.Min(i, animationData.openDurations.Length - 1)])
                    .setEase(animationData.openEaseTypes[Mathf.Min(i, animationData.openEaseTypes.Length - 1)]);
                yield return new WaitForSeconds(animationData.openDelay);
            }
        }

        onComplete?.Invoke();
    }

    private IEnumerator CloseAnimation(Overlay overlay, Action onComplete)
    {
        OverlayAnimationData animationData = overlay.animationData;

        if (!animationData.useCloseAnimation)
        {
            onComplete?.Invoke();
        }
        else
        {
            float additionalDelay = 0;

            if (animationData.closeElements != null)
                for (int i = 0; i < animationData.closeElements.Length; i++)
                {
                    LeanTween.scale(animationData.closeElements[i], Vector3.zero,
                            animationData.closeDurations[Mathf.Min(i, animationData.closeDurations.Length - 1)])
                        .setEase(animationData.closeEaseTypes[Mathf.Min(i, animationData.closeEaseTypes.Length - 1)]);

                    additionalDelay +=
                        animationData.closeDurations[Mathf.Min(i, animationData.closeDurations.Length - 1)];

                    yield return new WaitForSeconds(animationData.closeDelay);
                }

            yield return new WaitForSeconds(additionalDelay -
                                            (animationData.closeDelay * animationData.closeElements.Length));

            onComplete?.Invoke();
        }
    }

    #endregion

    public void CloseAllOverlays()
    {
        for (int i = 0; i < overlayList.Count - 1; i++)
        {
            CloseOverlay(overlayList[i]);
        }
    }
}


public static class SceneNames
{
    public static readonly string sLoading = "Loading";
    public static readonly string sMainMenu = "MainMenu";
    public static readonly string sGame = "Game";
}

public enum DeepLinkType
{
    None = -1,
    ZarinpalSuccess = 300,
    ZarinpalFail = 333
}