using System;
using System.Threading.Tasks;
using _Scripts.Tools.Service_Locator;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts
{
    public class LeaderboardDisplayer : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject leaderboardRecordPrefab;
        AsyncOperationHandle<GameObject> leaderboardRecordAsyncOperation;
        [SerializeField] Sprite[] specialRanks;
        [SerializeField] Sprite regularRank;
        [SerializeField] private RectTransform recordsHolder;
        private bool _recordsCreated;

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

            for (int i = 0; i < Mathf.Min(specialRanks.Length, records.Count); i++)
                CreateNewRecord(obj.Result, records[i].Score, records[i].Username, records[i].Rank, specialRanks[i]);

            if (records.Count > specialRanks.Length)
                for (int i = specialRanks.Length; i < records.Count; i++)
                    CreateNewRecord(obj.Result, records[i].Score, records[i].Username, records[i].Rank, regularRank);
            
            // for increase records in leaderboard for test
            if (records.Count > specialRanks.Length)
                for (int i = specialRanks.Length; i < records.Count; i++)
                    CreateNewRecord(obj.Result, records[i].Score, records[i].Username, records[i].Rank, regularRank);
            if (records.Count > specialRanks.Length)
                for (int i = specialRanks.Length; i < records.Count; i++)
                    CreateNewRecord(obj.Result, records[i].Score, records[i].Username, records[i].Rank, regularRank);
            if (records.Count > specialRanks.Length)
                for (int i = specialRanks.Length; i < records.Count; i++)
                    CreateNewRecord(obj.Result, records[i].Score, records[i].Username, records[i].Rank, regularRank);
        }

        void CreateNewRecord(GameObject prefab, string score, string userName, string rank, Sprite rankSprite)
        {
            var newOne = Instantiate(prefab, recordsHolder).GetComponent<LeaderBoardRecord>();
            newOne.transform.localScale = Vector3.one;
            newOne.SetUp(score, userName, rank, rankSprite);
            print("new record created");
            //return newOne;
        }
    }
}