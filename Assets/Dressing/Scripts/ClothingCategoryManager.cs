using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public interface IClothingUIManager
{
    void OnIconTabSwitch(IconTabController currIcon);
    void LoadUI();
}
public class ClothingCategoryManager : MonoBehaviour, IClothingUIManager
{
    // Start is called before the first frame update
    [SerializeField]
    private List<Sprite> _spriteIcons;

    [SerializeField]
    private ClothingCollection[] _clotheCategoryCollection;

    [SerializeField]
    private GameObject _clotheIconUIPrefab;

    [SerializeField]
    private ClothesType _activeClothesType = ClothesType.Bottom;
    private IconTabController _activeiconTab;

    [SerializeField]
    private ClothingItemManager _clothingItemManager;

    void Awake()
    {
        LoadUI();
    }

    public void LoadUI()
    {
        for(int i = 0; i < _spriteIcons.Count; i++)
        {
            var clotheIconUIPrefab = Instantiate(_clotheIconUIPrefab, transform);
            var icon = clotheIconUIPrefab.GetComponent<UnityEngine.UI.Image>();
            icon.sprite = _spriteIcons[i];

            IconTabController iconTab = clotheIconUIPrefab.GetComponent<IconTabController>();
            iconTab.SetCategoryManager(this);
            iconTab.SetAssets(_clotheCategoryCollection[i]);
            if(_clotheCategoryCollection[i].categoryType == _activeClothesType)
            {   
                OnIconTabSwitch(iconTab);
            }
        }
    }

    public void OnIconTabSwitch(IconTabController currIcon)
    {
        if(currIcon == _activeiconTab) return;
        currIcon.EnableTab();

        if(!_activeiconTab.IsUnityNull()) _activeiconTab.DisableTab();
        _activeiconTab = currIcon;

        //Now load single clothes icon from new active clothes type
        //Warning: this is not robust. consider fixing.
        _clothingItemManager.SetCurrClothesCollection((ClothingCollection)currIcon.uiAssets);
        _clothingItemManager.LoadUI();
    }
}
