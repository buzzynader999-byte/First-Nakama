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
        [SerializeField] private RectTransform recordsViewArea;
        [SerializeField] private RectTransform playerRecordUpPlace;
        [SerializeField] private RectTransform playerRecordDownPlace;
        private bool _recordsCreated;
        private string myUsername;
        private RectTransform _playerRecordHolder;
        private RectTransform _playerEmptyPlaceInRecords;
        private IApiLeaderboardRecord _playerRecord;

        private async void OnEnable()
        {
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

            if (_playerRecordHolder)
            {
                Destroy(_playerRecordHolder.gameObject);
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

            var records = await ServiceLocator.Instance.Get<ScoreManager>().GetRecords("attack", 100);
            if (records == null || records.Count == 0) return;
            var playerRecord = await ServiceLocator.Instance.Get<ScoreManager>().GetPlayerRank("attack");
            if (whoAmI == WhoAmI.Original)
            {
                _playerRecord = playerRecord;
                myUsername = GameManager.Instance.NakamaConnection.UserName;
            }
            else
            {
                myUsername = GetUsername(records);
            }

            // چند نفر اول
            for (int i = 0; i < Mathf.Min(specialRanks.Length, records.Count); i++)
                CreateNewRecord(obj.Result, records[i], specialRanks[i]);
            //بقیه رکوردها
            if (records.Count > specialRanks.Length)
            {
                for (int i = specialRanks.Length; i < records.Count; i++)
                    CreateNewRecord(obj.Result, records[i], regularRank);
            }

            if (!_playerEmptyPlaceInRecords)
            {
                _playerEmptyPlaceInRecords = CreateEmptyRecord(obj.Result);
                if (whoAmI == WhoAmI.Original)
                {
                    _playerRecordHolder = CreatePlayerRecord(obj.Result,
                        int.Parse(this._playerRecord.Rank) <= 3
                            ? specialRanks[int.Parse(this._playerRecord.Rank)]
                            : regularRank);
                }
                else if (whoAmI == WhoAmI.OutOfRanks)
                {
                    _playerRecordHolder = MockPlayerRecord(obj.Result, "fakePlayer", "1",
                        100 <= 3
                            ? specialRanks[100]
                            : regularRank, "100");
                }
            }
        }

        RectTransform MockPlayerRecord(GameObject prefab, string userName, string score, Sprite sprite, string rank)
        {
            var playerRecord = Instantiate(prefab, recordsViewArea);
            playerRecord.gameObject.name = "Player : " + userName;
            var target = playerRecord.GetComponent<LeaderBoardRecord>();
            target.transform.localScale = Vector3.one;
            target.SetUp(score, "You", rank, sprite);
            target.SetColor(Color.limeGreen);
            var rect = target.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.right;
            rect.sizeDelta = Vector2.up * 130;
            return rect;
        }

        void CreateNewRecord(GameObject prefab, IApiLeaderboardRecord record, Sprite rankSprite)
        {
            GameObject newRecord;
            if (record.Username == myUsername)
            {
                _playerEmptyPlaceInRecords = CreateEmptyRecord(prefab);
                if (whoAmI != WhoAmI.Original)
                    _playerRecord = record;
                _playerRecordHolder = CreatePlayerRecord(prefab, specialRanks[0]);
            }
            else
            {
                newRecord = Instantiate(prefab, recordsHolder);
                var target = newRecord.GetComponent<LeaderBoardRecord>();
                target.SetUp(record.Score, record.Username, record.Rank, rankSprite);
            }
        }

        RectTransform CreateEmptyRecord(GameObject basePrefab)
        {
            var newRecord = new GameObject("Empty Player");
            newRecord.gameObject.AddComponent<RectTransform>();
            var emptyRecor = newRecord.GetComponent<RectTransform>();
            emptyRecor.SetParent(recordsHolder);
            emptyRecor.sizeDelta = basePrefab.GetComponent<RectTransform>().sizeDelta;
            emptyRecor.localScale = Vector3.one;
            return emptyRecor;
        }

        RectTransform CreatePlayerRecord(GameObject prefab, Sprite rankSprite)
        {
            var playerRecord = Instantiate(prefab, recordsViewArea);
            playerRecord.gameObject.name = "Player : " + _playerRecord.Username;
            var target = playerRecord.GetComponent<LeaderBoardRecord>();
            target.transform.localScale = Vector3.one;
            target.SetUp(_playerRecord.Score, "You", _playerRecord.Rank, rankSprite);
            target.SetColor(Color.limeGreen);
            var rect = target.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.right;
            rect.sizeDelta = Vector2.up * 130;
            return rect;
        }

        private void Update()
        {
            if (_playerRecordHolder && _playerEmptyPlaceInRecords)
            {
                Vector2 emptyPos = _playerEmptyPlaceInRecords.position;
                float upPose = playerRecordUpPlace.transform.position.y;
                float downPose = playerRecordDownPlace.transform.position.y;
                Vector2 pos = new Vector2(emptyPos.x, Mathf.Clamp(emptyPos.y, downPose, upPose));
                _playerRecordHolder.position = pos;
            }
        }

        string GetUsername(List<IApiLeaderboardRecord> records)
        {
            return whoAmI switch
            {
                WhoAmI.First => records[0].Username,
                WhoAmI.Forth => records[3].Username,
                WhoAmI.Last => records[^1].Username,
                _ => "You"
            };
        }
    }
}