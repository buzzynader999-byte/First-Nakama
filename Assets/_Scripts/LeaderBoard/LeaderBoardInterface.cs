using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace _Scripts.LeaderBoard
{
    public static class LeaderBoardInterface
    {
        public static async Task<List<IApiLeaderboardRecord>> GetRecords(NakamaConnection connection,
            string leaderBoardId, IEnumerable<string> ownerIds = null, long? expiry = null, int limit = 1)
        {
            try
            {
                var leaderBoardScore =
                    await connection.Client.ListLeaderboardRecordsAsync(connection.Session, leaderBoardId, ownerIds,
                        expiry,
                        limit);
                var records = new List<IApiLeaderboardRecord>(leaderBoardScore.Records.ToList());
                return records;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }

        public static async Task<List<IApiUser>> GetUsersFromUserId(NakamaConnection connection, List<IApiLeaderboardRecord> records)
        {
            var userIds = records.Select(record => record.OwnerId).ToList();
            var users = await connection.Client.GetUsersAsync(connection.Session, userIds);
            return users.Users.ToList();
        }

        public static async Task<int> GetScoreOfThisUser(NakamaConnection connection, string leaderBoardId)
        {
            try
            {
                var records = await GetRecords(connection, leaderBoardId, null, null, 1);
                if (records.Count >= 1)
                    if (!String.IsNullOrEmpty(records[0]?.Score))
                        return int.Parse(records[0].Score);
                return 0;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }
    }
}