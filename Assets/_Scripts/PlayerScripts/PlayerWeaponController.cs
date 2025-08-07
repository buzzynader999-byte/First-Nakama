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

        private Weapon gun;

        private void Awake()
        {
            _weaponLoader = new AssetLoader<AssetReferenceGameObject, GameObject>(targetWeapon);
        }

        async void Start()
        {
            await _weaponLoader.LoadAsync();
            gun = Instantiate(_weaponLoader.LoadedAsset, player.GunPlace, false).GetComponent<Weapon>();
        }

        public void Attack()
        {
            gun.Attack();
        }
    }
}