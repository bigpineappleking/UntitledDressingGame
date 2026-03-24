using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingItemManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _clothesItemList;

    [SerializeField]
    private GameObject _clothesItemPrefab;

    public void LoadNewClothesType(ClothingCollection currClothesCollection)
    {
        DestroyExistingClothes();
        foreach(ClothingItem c in currClothesCollection.items)
        {
            var clothingItem = Instantiate(_clothesItemPrefab, transform);
            _clothesItemList.Add(clothingItem);
            var icon = clothingItem.GetComponent<UnityEngine.UI.Image>();
            icon.sprite = c.icon;
        }

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
