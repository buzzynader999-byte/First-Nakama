using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Entities
{
    [Serializable]
    public class AssetReferencePlayer :AssetReferenceT<Player>
    {
        public AssetReferencePlayer(string guid) : base(guid)
        {
            //...
        }
    }
}