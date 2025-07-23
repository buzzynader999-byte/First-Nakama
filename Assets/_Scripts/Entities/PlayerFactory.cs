using UnityEngine;

namespace _Scripts.Entities
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private Entities.Player playerPrefab;
        [SerializeField] PlayerLocalNetwork playerLocalNetworkPrefab;
        [SerializeField] PlayerRemote playerRemoteNetworkPrefab;
        public static PlayerFactory Instance;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject GetNewPlayer(bool isLocalPlayer)
        {
            var targetPrefab = isLocalPlayer ? playerLocalNetworkPrefab.gameObject : playerRemoteNetworkPrefab.gameObject;
            var newPlayer = Instantiate(targetPrefab, Vector3.up * 10, Quaternion.identity);
            newPlayer.GetComponent<Player>().ChangeColor(isLocalPlayer);
            return newPlayer.gameObject;
        }
    }
}