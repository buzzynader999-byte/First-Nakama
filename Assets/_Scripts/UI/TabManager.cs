using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.UI
{
    public class TabManager : MonoBehaviour
    {
        //[SerializeField] RectTransform contentPlace;
        //[SerializeField] RectTransform tabButtonsPlace;
        //[SerializeField] AssetReferenceGameObject tabButtonPrefab;

        [SerializeField] List<TabContainer> _tabContainers = new();
        [SerializeField] bool openFirstByDefault;
        
        List<GameObject> _tabs = new();
        List<GameObject> _tabsButtons = new();

        //private List<AssetLoader<AssetReferenceGameObject, GameObject>> assetsLoader;
        private AssetLoader<AssetReferenceGameObject, GameObject> buttonAssetsLoader;
        private TabContainer currentActiveTab;

        private void Start()
        {
            Initialize();
        }

        async void Initialize()
        {
            //await CreateTabs();
            //await CreateTabsButton();

            for (int i = 0; i < _tabContainers.Count; i++)
            {
                _tabContainers[i].Tab.gameObject.SetActive(false);
                var index = i;
                _tabContainers[i].Button.onClick.AddListener(() => OpenTab(index));
                _tabs.Add(_tabContainers[i].Tab);
            }

            if (openFirstByDefault)
            {
                OpenTab(0);
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
        
        /*async Task CreateTabs()
        {
            assetsLoader = new List<AssetLoader<AssetReferenceGameObject, GameObject>>();
            for (int i = 0; i < _tabContainers.Count; i++)
            {
                assetsLoader.Add(new AssetLoader<AssetReferenceGameObject, GameObject>(_tabContainers[i].Tab));
                var newLoaded = await assetsLoader[i].LoadAsync();
                var newTab = Instantiate(newLoaded, transform.position, Quaternion.identity);
                newTab.transform.SetParent(contentPlace);
                newTab.transform.localScale = Vector3.one;
                newTab.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                newTab.SetActive(false);

                _tabs.Add(newTab);
            }
        }*/

        /*async Task CreateTabsButton()
        {
            buttonAssetsLoader = new AssetLoader<AssetReferenceGameObject, GameObject>(tabButtonPrefab);
            var newLoaded = await buttonAssetsLoader.LoadAsync();

            for (int i = 0; i < _tabContainers.Count; i++)
            {
                var newButton = Instantiate(newLoaded, tabButtonsPlace.position, Quaternion.identity);
                newButton.transform.SetParent(tabButtonsPlace);
                newButton.transform.localScale = Vector3.one;
                newButton.transform.GetChild(0).GetComponent<Image>().sprite = _tabContainers[i].Icon;
                var index = i;
                newButton.GetComponent<Button>().onClick.AddListener(() => OpenTab(index));
                _tabsButtons.Add(newButton);
            }
        }*/
    }
}