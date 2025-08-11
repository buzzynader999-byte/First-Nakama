using UnityEngine;

namespace _Scripts
{
    public class CameraController : MonoBehaviour
    {
        GameObject _targetPlayer;
        [SerializeField] private float yOffset;
        public static CameraController Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_targetPlayer)
                transform.position = _targetPlayer.transform.position + Vector3.up * yOffset;
        }

        public void SetTarget(GameObject player)
        {
            _targetPlayer = player;
        }
    }
}