using System;
using _Scripts.Tools.Service_Locator;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts
{
    public class LeaderboardDisplayer : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject leaderboardRecordPrefab;
        AsyncOperationHandle leaderboardRecordAsyncOperation;

        private void OnEnable()
        {
            var records = ServiceLocator.Instance.Get<ScoreManager>().GetRecords();
            return;
            leaderboardRecordAsyncOperation = leaderboardRecordPrefab.LoadAssetAsync();
            leaderboardRecordAsyncOperation.Completed += (a) => { };
        }
    }
}