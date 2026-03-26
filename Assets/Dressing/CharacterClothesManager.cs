using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClothesManager : MonoBehaviour
{
    public static CharacterClothesManager instance;
    Dictionary<ClothesType, ClothingItem> equippedClothes;
    public List<ClothingItem> equippedAccessories{private set; get;}
    void Awake()
    {
        instance = this;

        equippedClothes = new Dictionary<ClothesType, ClothingItem>();
        equippedAccessories = new List<ClothingItem>();
        foreach(ClothesType type in System.Enum.GetValues(typeof(ClothesType)))
        {
            if(type == ClothesType.Accessories) continue;
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

    public void SetEquippedClothesByType(ClothesType type, ClothingItem clothes)
    {
        if(type == ClothesType.Accessories)
        {
            if(equippedAccessories.Count < 5)
            {
                equippedAccessories.Add(clothes);
            }
            else
            {
                //TODO:Warning cannot have more than 5 accessories
            }
        }else{
            if(type == ClothesType.Dress)
            {
                equippedClothes[ClothesType.Top] = null;
                equippedClothes[ClothesType.Bottom] = null;
            }else if(type == ClothesType.Top || type == ClothesType.Bottom)
            {
                equippedClothes[ClothesType.Dress] = null;
            }
            equippedClothes[type] = clothes;
        }
    }
}
