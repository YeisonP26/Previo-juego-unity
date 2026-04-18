using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class WireReferences2
{
    public static void Execute()
    {
        // Wire GameManager
        GameObject gmObj = GameObject.Find("GameManager");
        GameManager gm = gmObj.GetComponent<GameManager>();
        gm.scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        gm.messageText = GameObject.Find("MessageText").GetComponent<TextMeshProUGUI>();
        EditorUtility.SetDirty(gmObj);

        // Wire ShootingController
        GameObject shootPoint = GameObject.Find("ShootPoint");
        ShootingController sc = shootPoint.GetComponent<ShootingController>();
        sc.arrowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Arrow.prefab");
        sc.shootPoint = shootPoint.transform;
        sc.target = GameObject.Find("Diana").transform;
        sc.powerBarFill = GameObject.Find("PowerBarFill").GetComponent<Image>();
        sc.maxForce = 50f;
        sc.chargeSpeed = 18f;
        sc.minAngle = 5f;
        sc.maxAngle = 75f;
        EditorUtility.SetDirty(shootPoint);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("References wired v2!");
    }
}
