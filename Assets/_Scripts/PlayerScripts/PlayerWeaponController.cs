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
        Animator gunAnimator;
        private bool _canAttack = false;

        private Weapon _gun;

        private void Awake()
        {
            _weaponLoader = new AssetLoader<AssetReferenceGameObject, GameObject>(targetWeapon);
        }

        async void Start()
        {
            await _weaponLoader.LoadAsync();
            _gun = Instantiate(_weaponLoader.LoadedAsset, player.GunPlace, false).GetComponent<Weapon>();
            gunAnimator = _gun.GetComponent<Animator>();
        }

        public void Attack()
        {
            gunAnimator.Play("Fire");
            if (_canAttack)
            {
                _gun.Attack();
                _canAttack = false;
                Invoke(nameof(ResetGunState), 0.5f);
            }
        }

        void ResetGunState()
        {
            _canAttack = true;
        }
    }
}