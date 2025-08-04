using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using Nakama;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts
{
    public enum WhoAmI
    {
        First,
        Forth,
        Last,
        OutOfRanks,
        Original
    }

    public class LeaderboardDisplayer : MonoBehaviour
    {
        AsyncOperationHandle<GameObject> leaderboardRecordAsyncOperation;
        [SerializeField] WhoAmI whoAmI = WhoAmI.First;
        [SerializeField] private AssetReferenceGameObject leaderboardRecordPrefab;
        [SerializeField] Sprite[] specialRanks;
        [SerializeField] Sprite regularRank;
        [SerializeField] private RectTransform recordsHolder;
        [SerializeField] private RectTransform playerOutOfRankPlace;
        [SerializeField] private RectTransform recordsViewArea;
        private bool _recordsCreated;
        private string myUsername;
        private GameObject _playerRecordHolder;
        private RectTransform _playerEmptyPlaceInRecords;
        private IApiLeaderboardRecord playerRecord;

        private async void OnEnable()
        {
            //Addressables.Release(leaderboardRecordAsyncOperation);
            if (!_recordsCreated)
            {
                if (!leaderboardRecordAsyncOperation.IsValid())
                {
                    leaderboardRecordAsyncOperation = leaderboardRecordPrefab.LoadAssetAsync();
                    await leaderboardRecordAsyncOperation.Task;
                }

                await LeaderboardRecordAsyncOperationOnCompleted(leaderboardRecordAsyncOperation);
                _recordsCreated = true;
            }
        }

        private void OnDisable()
        {
            //todo: use object pooling
            if (transform.childCount > 0)
            {
                foreach (Transform child in recordsHolder)
                {
                    print("Destroying " + child.name);
                    Destroy(child.gameObject);
                }

                _recordsCreated = false;
            }
        }

        private void OnDestroy()
        {
            if (leaderboardRecordAsyncOperation.IsValid())
                Addressables.Release(leaderboardRecordAsyncOperation);
        }

        private async Task LeaderboardRecordAsyncOperationOnCompleted(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Load failed: " + obj.OperationException);
                return;
            }

            var records = await ServiceLocator.Instance.Get<ScoreManager>().GetRecords();
            if (records == null || records.Count == 0) return;

            myUsername = whoAmI != WhoAmI.Original
                ? GetUsername(records)
                : GameManager.Instance.NakamaConnection.UserName;
            int specialCount = Mathf.Min(specialRanks.Length, records.Count);
            for (int i = 0; i < specialCount; i++)
                CreateNewRecord(obj.Result, records[i], specialRanks[i]);

            if (records.Count > specialRanks.Length)
            {
                for (int i = specialRanks.Length; i < records.Count; i++)
                    CreateNewRecord(obj.Result, records[i], regularRank);
            }

            //recordsViewArea.offsetMin = new Vector2(10, 150);
        }

        void CreateNewRecord(GameObject prefab, IApiLeaderboardRecord record, Sprite rankSprite)
        {
            GameObject newRecord;
            if (record.Username == myUsername)
            {
                newRecord = new GameObject("Empty Player");
                newRecord.gameObject.AddComponent<RectTransform>();
                _playerEmptyPlaceInRecords = newRecord.GetComponent<RectTransform>();
                playerRecord = record;
                _playerRecordHolder = CreatePlayerRecord(prefab, playerRecord, specialRanks[0]);
            }
            else
            {
                newRecord = Instantiate(prefab, recordsHolder);
                var target = newRecord.GetComponent<LeaderBoardRecord>();
                target.SetUp(record.Score, record.Username, record.Rank, rankSprite);
                // target.transform.localScale = Vector3.one;
            }
        }

        GameObject CreatePlayerRecord(GameObject prefab, IApiLeaderboardRecord record, Sprite rankSprite)
        {
            var playerReord = Instantiate(prefab, recordsViewArea);
            var target = playerReord.GetComponent<LeaderBoardRecord>();
            target.SetUp(record.Score, record.Username, record.Rank, rankSprite);
            return target.gameObject;
        }

        private void Update()
        {
            if (_playerRecordHolder)
            {
                var pos = new Vector2(_playerEmptyPlaceInRecords.position.x,
                    Mathf.Clamp(_playerEmptyPlaceInRecords.position.y, recordsViewArea.offsetMin.y,
                        recordsViewArea.offsetMax.y));
                _playerRecordHolder.transform.position = pos;
            }
        }

        string GetUsername(List<IApiLeaderboardRecord> records)
        {
            switch (whoAmI)
            {
                case WhoAmI.First:
                    return records[0].Username;
                    break;
                case WhoAmI.Forth:
                    return records[3].Username;
                    break;
                case WhoAmI.Last:
                    return records[^1].Username;
                    break;
                default:
                case WhoAmI.OutOfRanks:
                    return "You";
            }
        }
    }
}