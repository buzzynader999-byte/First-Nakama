using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.UI
{
    public class TabManager : MonoBehaviour
    {

        [SerializeField] List<TabContainer> _tabContainers = new();
        [Tooltip("Set it to negative if you do not want to open by default")]
        [SerializeField] int defaultIndex = -1;
        
        List<GameObject> _tabs = new();
        private AssetLoader<AssetReferenceGameObject, GameObject> buttonAssetsLoader;
        private TabContainer currentActiveTab;

        private void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            for (int i = 0; i < _tabContainers.Count; i++)
            {
                _tabContainers[i].Tab.gameObject.SetActive(false);
                var index = i;
                _tabContainers[i].Button.onClick.AddListener(() => OpenTab(index));
                _tabs.Add(_tabContainers[i].Tab);
            }

            if (defaultIndex>=0)
            {
                OpenTab(Mathf.Clamp(defaultIndex,0,_tabContainers.Count-1));
            }
        }

        public void OpenTab(int index)
        {
            if (currentActiveTab != null) ChangeThsTabStateTo(currentActiveTab, false);
            var target = _tabContainers[index];
            ChangeThsTabStateTo(target, true);
        }

        void ChangeThsTabStateTo(TabContainer target, bool state)
        {
            if (state)
            {
                target.Tab.SetActive(true);
                target.Button.GetComponent<RectTransform>().localScale *= 1.5f;
                currentActiveTab = target;
                return;
            }
            target?.Tab.SetActive(false);
            target.Button.GetComponent<RectTransform>().localScale /= 1.5f;
        }
    }
}