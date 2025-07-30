using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Weapoons
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] float bulletSpeed;
        [SerializeField] private Transform firePoint;
        [SerializeField] AssetReferenceGameObject bulletPrefab;

        private AssetLoader<AssetReferenceGameObject, GameObject> _bulletLoader;
        public Transform FirePoint => firePoint;
        public float BulletSpeed => bulletSpeed;

        public float FireRate
        {
            get => fireRate;
            private set => fireRate = value;
        }

        public AssetReferenceGameObject BulletPrefab => bulletPrefab;

        async void Start()
        {
            if (BulletPrefab == null)
            {
                Debug.Log("BulletPrefab is null ...");
            }

            _bulletLoader = new AssetLoader<AssetReferenceGameObject, GameObject>(bulletPrefab);
            await _bulletLoader.LoadAsync();
        }

        public virtual void Attack()
        {
            var newBullet = Instantiate(_bulletLoader.LoadedAsset, firePoint.position, Quaternion.identity);
            newBullet.GetComponent<Bullet>().Rigidbody2D.linearVelocity = transform.right * BulletSpeed;
        }
    }
}