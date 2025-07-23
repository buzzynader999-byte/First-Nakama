using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;

namespace _Scripts
{
    public static class MatchDataJson
    {
        public static string Died(Vector3 position)
        {
            var values = new Dictionary<string, string>
            {
                { "position.x", position.x.ToString() },
                { "position.y", position.y.ToString() }
            };

            return values.ToJson();
        }
    }
}