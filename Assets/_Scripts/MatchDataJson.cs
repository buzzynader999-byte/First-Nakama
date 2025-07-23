using System.Collections.Generic;
using _Scripts.Entities;
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

        public static string VelocityAndPosition(Vector2 velocity, Vector3 position)
        {
            var values = new Dictionary<string, string>
            {
                { "velocity.x", velocity.x.ToString() },
                { "velocity.y", velocity.y.ToString() },
                { "position.x", position.x.ToString() },
                { "position.y", position.y.ToString() }
            };

            return values.ToJson();
        }

        public static string Input(PlayerInputControllerDetails inputDetails)
        {
            var values = new Dictionary<string, string>
            {
                { "horizontalInput", inputDetails.HorizontalInput.ToString() },
                { "jump", inputDetails.Jump.ToString() },
                { "jumpHeld", inputDetails.JumpHeld.ToString() },
                { "attack", inputDetails.Attack.ToString() }
            };

            return values.ToJson();
        }
    }
}