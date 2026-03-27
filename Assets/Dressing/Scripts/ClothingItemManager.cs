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
    private List<IconTabController> _activeAccessories;

    //Ui change, set selected clothes at manager
    public void OnIconTabSwitch(IconTabController currIcon)
    {
        if(_currClothesCollection.categoryType == ClothesType.Accessories){
            //turn off accessories when select repeat accessories
            if (!_activeAccessories.IsUnityNull() && _activeAccessories.Contains(currIcon))
            {
                currIcon.DisableTab();
                _activeAccessories.Remove(currIcon);
            }else{
                //select new accessories when have less than five
                if(_activeAccessories.Count < 5){
                    currIcon.EnableTab();
                    _activeAccessories.Add(currIcon);
                }
            }
        }
        else
        {
            //nothing happens when reselect selected clothes
            if(currIcon == _activeclothes) return;

            //turn off old clothes and turn on new clothes
            if(!_activeclothes.IsUnityNull()) _activeclothes.DisableTab();
            _activeclothes = currIcon;
            currIcon.EnableTab();
        }

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
            if(!_activeAccessories.IsUnityNull()) _activeAccessories.Clear();
            _activeAccessories = new List<IconTabController>();
            var equippedClothes = CharacterClothesManager.instance.equippedAccessories;
            if (!equippedClothes.IsUnityNull())
            {
                foreach(ClothingItem c in equippedClothes)
                {
                    var tab = FindEquippedClothesTab(c);
                    _activeAccessories.Add(tab);
                    tab.EnableTab();
                }
            }
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
