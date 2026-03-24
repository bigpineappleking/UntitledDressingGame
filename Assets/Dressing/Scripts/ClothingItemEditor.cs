using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClothingItem))]
public class ClothingItemEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        ClothingItem item = target as ClothingItem;
        
        if (item == null || item.icon == null)
            return base.RenderStaticPreview(assetPath, subAssets, width, height);

        Texture2D preview = AssetPreview.GetAssetPreview(item.icon);
        if (preview == null)
            return base.RenderStaticPreview(assetPath, subAssets, width, height);

        Texture2D result = new Texture2D(width, height);
        EditorUtility.CopySerialized(preview, result);
        return result;
    }
}
