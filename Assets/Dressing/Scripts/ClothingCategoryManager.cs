using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClothingCategoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Sprite[] _clotheCategoryIcons;

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
        for(int i = 0; i < _clotheCategoryIcons.Length; i++)
        {
            var clotheIconUIPrefab = Instantiate(_clotheIconUIPrefab, transform);
            var icon = clotheIconUIPrefab.GetComponent<UnityEngine.UI.Image>();
            icon.sprite = _clotheCategoryIcons[i];

            IconTabController iconTab = clotheIconUIPrefab.GetComponent<IconTabController>();
            iconTab.SetCategoryManager(this);
            iconTab.SetClothesType(_clotheCategoryCollection[i].categoryType);
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
        _activeClothesType = currIcon._clothesType;

        //Now load single clothes icon from new active clothes type
        _clothingItemManager.LoadNewClothesType(FindCollectionByType(_activeClothesType));
    }

    private ClothingCollection FindCollectionByType(ClothesType type)
    {
        foreach(ClothingCollection c in _clotheCategoryCollection)
        {
            if(c.categoryType == type) return c;
        }
        Debug.LogError("Cannot find clothes collection by type. Check UI setup.");
        return null;
    }


    public void SetCurrentClothesType(ClothesType type)
    {
        _activeClothesType = type;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
