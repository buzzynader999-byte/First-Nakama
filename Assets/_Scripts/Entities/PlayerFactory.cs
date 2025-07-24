using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Entities
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField] PlayerLocalNetwork playerLocalNetworkPrefab;
        [SerializeField] PlayerRemote playerRemoteNetworkPrefab;
        public static PlayerFactory instance;

        private void Awake() => instance = this;

        public GameObject GetNewPlayer(bool isLocal)
        {
            var target = isLocal ? playerLocalNetworkPrefab.gameObject : playerRemoteNetworkPrefab.gameObject;
            var newPlayer = Instantiate(target, Vector3.up*7, Quaternion.identity);
            newPlayer.GetComponent<Player>().ChangeColor(isLocal);
            return newPlayer.gameObject;
        }
    }
}