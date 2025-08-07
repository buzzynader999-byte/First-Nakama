using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.PlayerScripts
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] List<Transform> spawnPoints = new();
        [SerializeField] AssetReferenceGameObject playerLocalNetworkPrefab;
        [SerializeField] AssetReferenceGameObject playerRemoteNetworkPrefab;
        public static PlayerSpawner instance;
        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> Players => _players;

        GameObject _loadedLocalPlayer;
        GameObject _loadedRemotePlayer;
        private void Awake() => instance = this;

        public async Task<GameObject> SpawnPlayerAsync(IUserPresence targetUser, bool isLocalUser)
        {
            if (_players.ContainsKey(targetUser.SessionId))
                return null;
            var pos = spawnPoints[Random.Range(0, spawnPoints.Count - 1)].position;

            await CheckForLoadedPlayers();
            var newPlayer = Instantiate(isLocalUser ? _loadedLocalPlayer : _loadedRemotePlayer,
                pos,
                Quaternion.identity);
            if (isLocalUser) CameraController.Instance.SetTarget(newPlayer);
            _players.Add(targetUser.SessionId, newPlayer);
            return newPlayer;
        }

        Task LoadLocalPlayer()
        {
            var asyncOperationHandle = playerLocalNetworkPrefab.LoadAssetAsync<GameObject>();
            asyncOperationHandle.Completed += (a) => _loadedLocalPlayer = a.Result;
            return asyncOperationHandle.Task;
        }

        Task LoadRemotePlayer()
        {
            var asyncOperationHandle = playerRemoteNetworkPrefab.LoadAssetAsync<GameObject>();
            asyncOperationHandle.Completed += (a) => _loadedRemotePlayer = a.Result;
            return asyncOperationHandle.Task;
        }

        async Task CheckForLoadedPlayers()
        {
            if (!playerLocalNetworkPrefab.IsValid()) await LoadLocalPlayer();
            if (!playerRemoteNetworkPrefab.IsValid()) await LoadRemotePlayer();
        }

        public void Destroy(string leavedSessionId)
        {
            Addressables.ReleaseInstance(_players[leavedSessionId]);
            _players.Remove(leavedSessionId);
        }
    }
}