using UnityEngine;
using UnityEditor;

public class FixOvershootBalance
{
    public static void Execute()
    {
        // ==========================================
        // PHYSICS BALANCE: max power MUST overshoot
        // ==========================================
        // ShootPoint at x=-5.8, Diana at x~6 (range 1-8)
        // Distance ~12 units horizontal
        // 
        // With maxForce=70, gravity=0.4, angle~15°:
        //   Medium power (~55%) -> hits diana center
        //   Full power (100%) -> arrow flies OVER/PAST diana
        //   Low power (~30%) -> falls short
        // ==========================================

        // 1. Update ShootingController values
        GameObject sp = GameObject.Find("ShootPoint");
        var sc = sp.GetComponent<ShootingController>();
        sc.maxForce = 70f;       // much higher - full charge WILL overshoot
        sc.chargeSpeed = 22f;    // faster charge = tighter timing window
        EditorUtility.SetDirty(sp);

        // 2. Update Arrow prefab - less gravity for flatter trajectory
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var rb = prefab.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.4f;  // was 0.6 - flatter arc, more horizontal travel
        EditorUtility.SetDirty(prefab);

        // 3. Save
        AssetDatabase.SaveAssets();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("OVERSHOOT BALANCE FIXED: maxForce=70, gravity=0.4, chargeSpeed=22. Sweet spot ~55% power at ~15° angle.");
    }
}
