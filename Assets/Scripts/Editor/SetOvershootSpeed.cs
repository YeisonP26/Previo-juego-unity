using UnityEngine;
using UnityEditor;

public class SetOvershootSpeed
{
    public static void Execute()
    {
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var ac = prefab.GetComponent<ArrowController>();
        ac.overshootSpeed = 25f; // arrows faster than this pass through diana
        ac.bullseyeRadius = 0.55f;
        EditorUtility.SetDirty(prefab);
        AssetDatabase.SaveAssets();
        Debug.Log("Arrow overshoot speed set to 25. Arrows going too fast will fly right through!");
    }
}
