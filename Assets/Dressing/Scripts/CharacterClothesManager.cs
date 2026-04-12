using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterClothesManager : MonoBehaviour
{
    public static CharacterClothesManager instance;
    Dictionary<ClothesType, ClothingItem> equippedClothes;
    public List<ClothingItem> equippedAccessories {private set; get;}
    [SerializeField]
    private ClothingItem _defaultClothes;
    [SerializeField]
    private GameObject _clothesContainer;
    [SerializeField]
    private GameObject _clothesPrefab;
    [SerializeField]
    private GameObject _characterSprite;
    [SerializeField]
    private GameObject _characterBg;
    void Awake()
    {
        instance = this;
        Hide();

        equippedClothes = new Dictionary<ClothesType, ClothingItem>();
        equippedAccessories = new List<ClothingItem>();
        foreach(ClothesType type in System.Enum.GetValues(typeof(ClothesType)))
        {
            if(type == ClothesType.Accessories) continue;
            if(type == ClothesType.Bottom)
            {
                PutDefaultclothesOn();
                continue;
            }

            equippedClothes[type] = null;
        }
    }

    public ClothingItem GetEquippedClothesByType(ClothesType type)
    {
        return equippedClothes[type];
    }
    
    public List<ClothingItem> GetEquippedAccessories()
    {
        return equippedAccessories;
    }


    public void SetEquippedClothesByType(ClothesType clothesType, ClothingItem clothes)
    {
        if(clothesType == ClothesType.Accessories)
        {
            //If select equipped accessories will take it off
            if (equippedAccessories.Contains(clothes))
            {
                TakeAccessoriesOffCharacter(clothes);
                equippedAccessories.Remove(clothes);
            }else{
                if(equippedAccessories.Count < 5)
                {
                    equippedAccessories.Add(clothes);
                    PutClothesOnCharacter(clothes);
                }
                else
                {
                    Debug.LogWarning("Cannot wear more than 5, TODO: add dialogue notification");
                    //TODO:Warning cannot have more than 5 accessories
                }
            }
        }else{
            if(clothesType == ClothesType.Dress)
            {
                TakeClothesOffCharacter(ClothesType.Top);
                equippedClothes[ClothesType.Top] = null;
                TakeClothesOffCharacter(ClothesType.Bottom);
                equippedClothes[ClothesType.Bottom] = null;
            }else if(clothesType == ClothesType.Top || clothesType == ClothesType.Bottom)
            {
                TakeClothesOffCharacter(ClothesType.Dress);
                equippedClothes[ClothesType.Dress] = null;

                //put pants on if switch from dress to top
                //if(clothesType == ClothesType.Top) PutDefaultclothesOn();
            }

            TakeClothesOffCharacter(clothesType);         
            equippedClothes[clothesType] = clothes;
            PutClothesOnCharacter(equippedClothes[clothesType], clothesType == ClothesType.Dress);
        }
    }
    void PutClothesOnCharacter(ClothingItem clothes, bool isDress = false)
    {
        if(clothes.isDefault){
            //If take off dress, put default pants on
            //if(isDress) PutDefaultclothesOn();
            return;
        }
        var newClothes = Instantiate(_clothesPrefab, _clothesContainer.transform);
        var newClothesOrder = newClothes.GetComponent<SpriteRenderer>();
        newClothesOrder.sortingOrder = clothes.renderOrder;
        newClothesOrder.sprite = clothes.icon;
        clothes.associatedClothes = newClothes;
    }

    void TakeClothesOffCharacter(ClothesType type)
    {
        if(equippedClothes[type].IsUnityNull()) return;
        var oldClothes = equippedClothes[type].associatedClothes;
        Destroy(oldClothes);
        equippedClothes[type].associatedClothes = null;
    }

    void TakeAccessoriesOffCharacter(ClothingItem clothes)
    {
        var oldClothes = clothes.associatedClothes;
        Destroy(oldClothes);
        clothes.associatedClothes = null;
    }

    void PutDefaultclothesOn()
    {
        equippedClothes[ClothesType.Bottom] = _defaultClothes;
        PutClothesOnCharacter(_defaultClothes);
    }

    public void Show()
    {
        _characterBg.SetActive(true);
        _characterSprite.SetActive(true);
    }

    public void Hide()
    {
        _characterBg.SetActive(false);
        _characterSprite.SetActive(false);
    }
}
