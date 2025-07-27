using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Weapoons
{
    public interface IWeapon
    {
        AssetReferenceGameObject BulletPrefab { get; }
        Transform FirePoint { get; }
        float BulletSpeed { get; }
        float FireRate { get; }
        void Attack();
    }
}