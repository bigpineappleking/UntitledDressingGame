using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClothingItemManager : MonoBehaviour, IClothingUIManager
{
    [SerializeField]
    private List<GameObject> _clothesItemList;

    [SerializeField]
    private GameObject _clothesItemPrefab;

    private ClothingCollection _currClothesCollection;
    private IconTabController _activeclothes;

    //Ui change, set selected clothes at manager
    public void OnIconTabSwitch(IconTabController currIcon)
    {
        if(currIcon == _activeclothes) return;
        currIcon.EnableTab();

        if(!_activeclothes.IsUnityNull()) _activeclothes.DisableTab();
        _activeclothes = currIcon;

        CharacterClothesManager.instance.SetEquippedClothesByType(_currClothesCollection.categoryType, (ClothingItem)currIcon.uiAssets);
    }

    //load clothes to list
    public void LoadUI()
    {
        if(_currClothesCollection == null)
        {
            Debug.LogError("Clothes colleciton cannot find");
            return;
        }
        DestroyExistingClothes();
        foreach(ClothingItem c in _currClothesCollection.items)
        {
            var clothingItem = Instantiate(_clothesItemPrefab, transform);
            IconTabController item = clothingItem.GetComponent<IconTabController>();
            item.SetCategoryManager(this);
            item.SetAssets(c);

            _clothesItemList.Add(clothingItem);
            var icon = clothingItem.GetComponent<UnityEngine.UI.Image>();
            icon.sprite = c.icon;
        }
        GetEquippedClothes();
    }

    // set current clothes collection
    public void SetCurrClothesCollection(ClothingCollection currClothesCollection)
    {
        _currClothesCollection = currClothesCollection;
    }


    //get equipped clothes from character manager and load active in UI
    void GetEquippedClothes()
    {
        if(_currClothesCollection.categoryType != ClothesType.Accessories)
        {
            var equippedClothes = CharacterClothesManager.instance.GetEquippedClothesByType(_currClothesCollection.categoryType);
            IconTabController tab;
            if (equippedClothes.IsUnityNull())
            {
                tab =_clothesItemList[0].GetComponent<IconTabController>();
            }
            else
            {
                tab = FindEquippedClothesTab(equippedClothes);
            }
            tab.EnableTab();
            _activeclothes = tab;
        }
        else
        {
            //TODO: Get equipped accessories
        }
    }

    IconTabController FindEquippedClothesTab(ClothingItem item)
    {
        foreach(GameObject g in _clothesItemList)
        {
            var tab = g.GetComponent<IconTabController>();
            if(tab.uiAssets == item)
            {
                return tab;
            }
        }
        return null;
    }

    void DestroyExistingClothes()
    {
        foreach(GameObject g in _clothesItemList)
        {
            Destroy(g);
        }
        _clothesItemList.Clear();
    }
}
