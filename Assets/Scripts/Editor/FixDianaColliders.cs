using UnityEngine;
using UnityEditor;

public class FixDianaColliders
{
    public static void Execute()
    {
        GameObject diana = GameObject.Find("Diana");
        if (diana == null) { Debug.LogError("Diana not found!"); return; }

        // Remove ALL colliders from ALL children
        foreach (var col in diana.GetComponentsInChildren<Collider2D>())
        {
            Object.DestroyImmediate(col);
        }

        // Remove all tags from children
        foreach (Transform child in diana.transform)
        {
            child.gameObject.tag = "Untagged";
        }

        // Add ONE single CircleCollider2D on the Diana parent covering the full target
        var dianaCol = diana.AddComponent<CircleCollider2D>();
        dianaCol.isTrigger = true;
        dianaCol.radius = 1.65f; // covers the full diana (ring4 scale 3.3 / 2)
        diana.tag = "Target";

        EditorUtility.SetDirty(diana);

        // Also update the arrow prefab to set bullseyeRadius
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab != null)
        {
            var ac = prefab.GetComponent<ArrowController>();
            if (ac != null)
            {
                ac.bullseyeRadius = 0.5f; // generous bullseye hit zone
                EditorUtility.SetDirty(prefab);
            }
        }

        AssetDatabase.SaveAssets();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("Diana colliders fixed! Single collider on Diana parent, distance-based scoring.");
    }
}
