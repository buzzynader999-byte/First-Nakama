using System;
using _Scripts.Entities;
using _Scripts.Weapoons;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.PlayerScripts
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] AssetReferenceGameObject targetWeapon;
        [SerializeField] Player player;
        private AssetLoader<AssetReferenceGameObject, GameObject> _weaponLoader;

        private void Awake()
        {
            _weaponLoader = new AssetLoader<AssetReferenceGameObject, GameObject>(targetWeapon);
        }

        async void Start()
        {
            await _weaponLoader.LoadAsync();
            Instantiate(_weaponLoader.LoadedAsset, player.GunPlace, false);
        }
    }
}