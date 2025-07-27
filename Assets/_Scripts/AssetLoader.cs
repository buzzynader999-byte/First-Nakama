using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

namespace _Scripts
{
    public class AssetLoader<TReference, TAsset>
        where TAsset : Object
        where TReference : AssetReference
    {
        private TReference _assetReference;
        private TAsset _loadedAsset;
        private AsyncOperationHandle<TAsset> _loadHandle;

        public TAsset LoadedAsset => _loadedAsset;
        public bool IsLoaded => _loadedAsset != null;

        public AssetLoader(TReference assetReference)
        {
            _assetReference = assetReference;
        }

        public async Task<TAsset> LoadAsync()
        {
            if (!IsLoaded)
            {
                if (_loadHandle.IsValid())
                {
                    await _loadHandle.Task;
                }
                else
                {
                    _loadHandle = _assetReference.LoadAssetAsync<TAsset>();
                    await _loadHandle.Task;

                    if (_loadHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        _loadedAsset = _loadHandle.Result;
                    }
                    else
                    {
                        Debug.LogError($"Failed to load asset: {_assetReference}");
                        return null;
                    }
                }
            }

            return _loadedAsset;
        }

        public void Release()
        {
            if (_loadHandle.IsValid())
            {
                Addressables.Release(_loadHandle);
                _loadedAsset = null;
            }
        }
    }
}