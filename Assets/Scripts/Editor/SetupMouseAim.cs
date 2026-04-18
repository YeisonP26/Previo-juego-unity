using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class SetupMouseAim
{
    public static void Execute()
    {
        // 1. Add angle display text to UI
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null) { Debug.LogError("No Canvas!"); return; }

        // Remove old angle text if exists
        Transform oldAngle = canvas.transform.Find("AngleText");
        if (oldAngle != null) Object.DestroyImmediate(oldAngle.gameObject);

        // Create angle text - right side of power bar
        Transform powerBarBG = canvas.transform.Find("PowerBarBG");
        GameObject angleObj = new GameObject("AngleText");
        angleObj.transform.SetParent(powerBarBG, false);
        var angleRect = angleObj.AddComponent<RectTransform>();
        angleRect.anchorMin = new Vector2(1f, 0f);
        angleRect.anchorMax = new Vector2(1f, 1f);
        angleRect.pivot = new Vector2(0f, 0.5f);
        angleRect.anchoredPosition = new Vector2(15, 0);
        angleRect.sizeDelta = new Vector2(80, 0);
        var angleTMP = angleObj.AddComponent<TextMeshProUGUI>();
        angleTMP.text = "25°";
        angleTMP.fontSize = 26;
        angleTMP.fontStyle = FontStyles.Bold;
        angleTMP.color = new Color(1f, 0.9f, 0.5f);
        angleTMP.alignment = TextAlignmentOptions.MidlineLeft;
        angleTMP.enableWordWrapping = false;
        angleObj.AddComponent<CanvasRenderer>();

        // 2. Update instructions
        Transform instrTf = canvas.transform.Find("InstructionsText");
        if (instrTf != null)
        {
            var instrTMP = instrTf.GetComponent<TextMeshProUGUI>();
            if (instrTMP != null)
            {
                instrTMP.text = "Mouse = Apuntar | ESPACIO = Cargar y Disparar";
                EditorUtility.SetDirty(instrTf.gameObject);
            }
        }

        // 3. Wire references
        GameObject sp = GameObject.Find("ShootPoint");
        var sc = sp.GetComponent<ShootingController>();
        sc.angleText = angleTMP;
        sc.minAngle = 5f;
        sc.maxAngle = 75f;
        EditorUtility.SetDirty(sp);

        // Save
        AssetDatabase.SaveAssets();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        Debug.Log("Mouse aiming setup complete! Angle text + line renderer ready.");
    }
}
