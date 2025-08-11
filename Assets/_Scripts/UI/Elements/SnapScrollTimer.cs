using System.Collections;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class SnapScrollTimer : MonoBehaviour
    {
        [SerializeField] private SimpleScrollSnap scrollSnap;
        [SerializeField] private float waitingTime;
        private bool _enableTime;

        private void OnEnable()
        {
            _enableTime = true;
            StartCoroutine(NextItemInScroll());
        }

        private IEnumerator NextItemInScroll()
        {
            while (_enableTime)
            {
                yield return new WaitForSeconds(waitingTime);
                scrollSnap.GoToNextPanel();
            }
        }

        private void OnDisable()
        {
            _enableTime = false;
        }
    }
}