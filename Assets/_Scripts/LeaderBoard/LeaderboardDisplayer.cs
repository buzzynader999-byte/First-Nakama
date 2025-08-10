using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using Nakama;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts.LeaderBoard
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
        //[SerializeField] WhoAmI whoAmI = WhoAmI.First;
        [SerializeField] private AssetReferenceGameObject leaderboardRecordPrefab;
        [SerializeField] Sprite[] specialRanks;
        [SerializeField] Sprite regularRank;
        [SerializeField] private List<LeaderBoardRecord> topThree;
        [SerializeField] private RectTransform recordsHolder;
        [SerializeField] private RectTransform recordsViewArea;
        [SerializeField] private RectTransform playerRecordUpPlace;
        [SerializeField] private RectTransform playerRecordDownPlace;
        [SerializeField] private TextMeshProUGUI expireTimeText;
        AsyncOperationHandle<GameObject> leaderboardRecordAsyncOperation;
        private IApiLeaderboardRecord _playerRecord;
        private string myUsername;
        private string _leaderBoardExpiretime;
        private RectTransform _playerRecordHolder;
        private RectTransform _playerEmptyPlaceInRecords;
        private bool _recordsCreated;

        private NakamaConnection _connection => GameManager.Instance.NakamaConnection;

        private async void OnEnable()
        {
            /*try
            {*/
            await GetLeaderboardRemainingTimeAsync("attack");
            if (transform.childCount > 0)
                foreach (Transform child in recordsHolder)
                    Destroy(child.gameObject);

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
            /*}
            catch (Exception e)
            {
                Debug.Log(e);
            }*/
        }

        private void OnDisable()
        {
            //todo: use object pooling
            if (transform.childCount > 0)
            {
                foreach (Transform child in recordsHolder)
                {
                    Destroy(child.gameObject);
                }
            }

            _recordsCreated = false;
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
            var usersInLeaderBoard = await LeaderBoardInterface.GetUsersFromUserId(_connection, records);

            if (records == null || records.Count == 0) return;
            var playerRecord = await ServiceLocator.Instance.Get<ScoreManager>().GetPlayerRank("attack");
/*            if (whoAmI == WhoAmI.Original)
            {
                _playerRecord = playerRecord;
                myUsername = _connection.UserName;
            }
            else
            {
                myUsername = GetUsername(records);
            }
*/
            _playerRecord = playerRecord;
            myUsername = _connection.UserName;
            SetUpTopThree(topThree, records);

            // display other records
            if (records.Count > 3)
            {
                for (int i = specialRanks.Length; i < records.Count; i++)
                    CreateNewRecord(obj.Result, records[i], regularRank);
            }

            if (!_playerEmptyPlaceInRecords)
            {
                if (playerRecord == null) return;
                _playerEmptyPlaceInRecords = CreateEmptyRecord(obj.Result);
                _playerRecordHolder = CreatePlayerRecord(obj.Result,
                    int.Parse(_playerRecord.Rank) <= 3
                        ? specialRanks[int.Parse(_playerRecord.Rank) - 1]
                        : regularRank);
                /*}
                else if (whoAmI == WhoAmI.OutOfRanks)
                {
                    _playerRecordHolder = MockPlayerRecord(obj.Result, "fakePlayer", "1",
                        100 <= 3
                            ? specialRanks[100]
                            : regularRank, "100");
                }*/
            }

            void SetUpTopThree(List<LeaderBoardRecord> topThree, List<IApiLeaderboardRecord> records)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (records.Count >= i + 1 && records[i] != null)
                    {
                        var targetRecord = records[i];
                        var displayName = usersInLeaderBoard[i].DisplayName;
                        if (targetRecord != null)
                            topThree[i].SetUp(targetRecord.Score, displayName ?? targetRecord.Username,
                                targetRecord.Rank,
                                specialRanks[i]);
                        /*if (targetRecord.Username == myUsername)
                        {
                            //record.SetColor(Color.limeGreen);
                            record.SetName("You");
                        }*/
                    }
                    else
                    {
                        topThree[i].gameObject.SetActive(false);
                    }
                }
            }
        }


        /*RectTransform MockPlayerRecord(GameObject prefab, string userName, string score, Sprite sprite, string rank)
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
            //rect.sizeDelta = Vector2.up * 300;
            return rect;
        }*/

        void CreateNewRecord(GameObject prefab, IApiLeaderboardRecord record, Sprite rankSprite)
        {
            GameObject newRecord;
            if (record.Username == myUsername)
            {
                _playerEmptyPlaceInRecords = CreateEmptyRecord(prefab);
                //if (whoAmI != WhoAmI.Original)
                //    _playerRecord = record;
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
            target.SetUp(_playerRecord.Score, _playerRecord.Username, _playerRecord.Rank, rankSprite);
            target.SetColor(Color.limeGreen);
            var rect = target.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.right;
            //rect.sizeDelta = Vector2.up * 300;
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

            if (Time.frameCount % 20 == 0)
                UpdateExpireTime(_leaderBoardExpiretime);
        }

        void UpdateExpireTime(string targetDateString)
        {
            if (DateTimeOffset.TryParse(targetDateString, out DateTimeOffset targetDate))
            {
                DateTimeOffset currentDate = DateTimeOffset.UtcNow;
                TimeSpan timeRemaining = targetDate - currentDate;
                if (timeRemaining.TotalSeconds > 0)
                {
                    int days = timeRemaining.Days;
                    int hours = timeRemaining.Hours;
                    int minutes = timeRemaining.Minutes;
                    int seconds = timeRemaining.Seconds;
                    expireTimeText.text = days + " : " + hours + " : " + minutes + " : " + seconds;
                }
                else
                    Debug.Log("time expired!");
            }
            else
            {
                Debug.Log("can not convert date string!");
            }
        }

        /*string GetUsername(List<IApiLeaderboardRecord> records)
        {
            return whoAmI switch
            {
                WhoAmI.First => records[0].Username,
                WhoAmI.Forth => records[3].Username,
                WhoAmI.Last => records[^1].Username,
                _ => "You"
            };
        }*/

        private async Task GetLeaderboardRemainingTimeAsync(string leaderboardId)
        {
            var records = await LeaderBoardInterface.GetRecords(_connection, leaderboardId, null, null, 1);
            if (records?.Count >= 1)
            {
                _leaderBoardExpiretime = records[0].ExpiryTime;
            }
        }
    }
}