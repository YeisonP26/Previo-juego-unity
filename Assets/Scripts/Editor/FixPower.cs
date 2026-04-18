using UnityEngine;
using UnityEditor;

public class FixPower
{
    public static void Execute()
    {
        // Fix ShootingController - MUCH more power
        GameObject sp = GameObject.Find("ShootPoint");
        var sc = sp.GetComponent<ShootingController>();
        sc.maxForce = 50f;
        sc.chargeSpeed = 18f;
        sc.minAngle = 5f;
        sc.maxAngle = 75f;
        EditorUtility.SetDirty(sp);

        // Fix Arrow prefab - almost no gravity
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var rb = prefab.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.3f;  // was 0.5 - much less drop
        
        var ac = prefab.GetComponent<ArrowController>();
        ac.bullseyeRadius = 0.6f; // slightly more generous

        EditorUtility.SetDirty(prefab);
        AssetDatabase.SaveAssets();

        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("Power fixed! maxForce=40, chargeSpeed=25, gravity=0.3");
    }
}
