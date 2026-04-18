using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class WireReferences
{
    public static void Execute()
    {
        // Wire GameManager references
        GameObject gmObj = GameObject.Find("GameManager");
        GameManager gm = gmObj.GetComponent<GameManager>();

        // Find UI elements
        GameObject scoreText = GameObject.Find("ScoreText");
        GameObject msgText = GameObject.Find("MessageText");

        if (scoreText != null)
            gm.scoreText = scoreText.GetComponent<TextMeshProUGUI>();
        if (msgText != null)
            gm.messageText = msgText.GetComponent<TextMeshProUGUI>();

        EditorUtility.SetDirty(gmObj);

        // Wire ShootingController references
        GameObject shootPoint = GameObject.Find("ShootPoint");
        ShootingController sc = shootPoint.GetComponent<ShootingController>();

        // Load arrow prefab
        GameObject arrowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Arrow.prefab");
        if (arrowPrefab != null)
            sc.arrowPrefab = arrowPrefab;

        sc.shootPoint = shootPoint.transform;

        // Find power bar fill
        GameObject fillObj = GameObject.Find("PowerBarFill");
        if (fillObj != null)
            sc.powerBarFill = fillObj.GetComponent<Image>();

        EditorUtility.SetDirty(shootPoint);

        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("All references wired and scene saved!");
    }
}
