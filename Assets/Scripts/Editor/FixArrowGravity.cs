using UnityEngine;
using UnityEditor;

public class FixArrowGravity
{
    public static void Execute()
    {
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null) return;

        var rb = prefab.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0.5f; // Less gravity for more direct shots
        }

        EditorUtility.SetDirty(prefab);
        AssetDatabase.SaveAssets();
        Debug.Log("Arrow gravity fixed to 0.5!");
    }
}
