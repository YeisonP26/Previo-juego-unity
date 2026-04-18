using UnityEngine;
using UnityEditor;

public class FixBalance2
{
    public static void Execute()
    {
        // Current problem: overshoot speed=25 is too low, almost every shot passes through
        // maxForce=70 at angle 15° gives velocity ~67, WAY above 25 threshold
        //
        // Fix: raise overshoot threshold to 45, reduce maxForce to 55
        // Sweet spot: ~50-70% power at 10-25° angle
        // Overshoot: only happens at >80% power with flat angles

        // 1. Arrow prefab - raise overshoot threshold
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var ac = prefab.GetComponent<ArrowController>();
        ac.overshootSpeed = 45f;  // was 25 - only extreme shots pass through now
        EditorUtility.SetDirty(prefab);

        // 2. ShootingController - dial back max force
        GameObject sp = GameObject.Find("ShootPoint");
        var sc = sp.GetComponent<ShootingController>();
        sc.maxForce = 55f;       // was 70 - less extreme range
        sc.chargeSpeed = 18f;    // was 22 - slower charge = easier to control
        EditorUtility.SetDirty(sp);

        AssetDatabase.SaveAssets();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("Balance v2: maxForce=55, overshootSpeed=45, chargeSpeed=18. Sweet spot ~50-70% power.");
    }
}
