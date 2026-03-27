using UnityEngine;

public enum ClothesType
{
    Hair,
    Top,
    Bottom,
    Shoe,
    Sock,
    Dress,
    Accessories
}

[CreateAssetMenu(fileName = "NewClothingItem", menuName = "Dressing/Clothing Item")]
public class ClothingItem : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    [TextArea]
    public string itemDescription;

    [Header("Style Scores")]
    public int sexyScore;
    public int elegantScore;

    public int renderOrder;
    public Vector3 uiOffset;
    [HideInInspector]
    public GameObject associatedClothes;
    public bool isDefault;
}
