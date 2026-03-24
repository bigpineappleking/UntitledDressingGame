using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClothingCollection", menuName = "Dressing/Clothing Collection")]
public class ClothingCollection : ScriptableObject
{
    public ClothesType categoryType;
    public List<ClothingItem> items = new List<ClothingItem>();
}
