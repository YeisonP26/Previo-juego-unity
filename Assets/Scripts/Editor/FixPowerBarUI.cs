using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class FixPowerBarUI
{
    public static void Execute()
    {
        // ============================
        // 1. REBUILD POWER BAR - BIGGER, CENTERED BOTTOM
        // ============================
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null) { Debug.LogError("No Canvas!"); return; }
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Delete old power bar
        Transform oldBar = canvas.transform.Find("PowerBarBG");
        if (oldBar != null) Object.DestroyImmediate(oldBar.gameObject);

        // Create new Power Bar container - centered at bottom, bigger
        GameObject barBG = new GameObject("PowerBarBG");
        barBG.transform.SetParent(canvas.transform, false);
        var bgRect = barBG.AddComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0.5f, 0f);
        bgRect.anchorMax = new Vector2(0.5f, 0f);
        bgRect.pivot = new Vector2(0.5f, 0f);
        bgRect.anchoredPosition = new Vector2(0, 25);
        bgRect.sizeDelta = new Vector2(500, 50);
        var bgImage = barBG.AddComponent<Image>();
        bgImage.color = new Color(0.15f, 0.15f, 0.15f, 0.85f);
        barBG.AddComponent<CanvasRenderer>();

        // Border/outline effect (slightly larger dark rect behind)
        GameObject barBorder = new GameObject("PowerBarBorder");
        barBorder.transform.SetParent(barBG.transform, false);
        var borderRect = barBorder.AddComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = new Vector2(-3, -3);
        borderRect.offsetMax = new Vector2(3, 3);
        borderRect.SetAsFirstSibling();
        var borderImg = barBorder.AddComponent<Image>();
        borderImg.color = new Color(0.8f, 0.75f, 0.6f, 0.9f);
        barBorder.AddComponent<CanvasRenderer>();

        // Fill bar
        GameObject barFill = new GameObject("PowerBarFill");
        barFill.transform.SetParent(barBG.transform, false);
        var fillRect = barFill.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = new Vector2(4, 4);
        fillRect.offsetMax = new Vector2(-4, -4);
        var fillImg = barFill.AddComponent<Image>();
        fillImg.color = new Color(0.1f, 0.85f, 0.2f, 1f);
        fillImg.type = Image.Type.Filled;
        fillImg.fillMethod = Image.FillMethod.Horizontal;
        fillImg.fillAmount = 0f;
        barFill.AddComponent<CanvasRenderer>();

        // "POTENCIA" label - left of bar
        GameObject labelObj = new GameObject("PowerBarLabel");
        labelObj.transform.SetParent(barBG.transform, false);
        var labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0f, 0f);
        labelRect.anchorMax = new Vector2(0f, 1f);
        labelRect.pivot = new Vector2(1f, 0.5f);
        labelRect.anchoredPosition = new Vector2(-10, 0);
        labelRect.sizeDelta = new Vector2(160, 0);
        var labelTMP = labelObj.AddComponent<TextMeshProUGUI>();
        labelTMP.text = "POTENCIA";
        labelTMP.fontSize = 24;
        labelTMP.fontStyle = FontStyles.Bold;
        labelTMP.color = Color.white;
        labelTMP.alignment = TextAlignmentOptions.MidlineRight;
        labelTMP.enableWordWrapping = false;
        labelObj.AddComponent<CanvasRenderer>();

        // Percentage text - centered on bar
        GameObject pctObj = new GameObject("PowerPercentText");
        pctObj.transform.SetParent(barBG.transform, false);
        var pctRect = pctObj.AddComponent<RectTransform>();
        pctRect.anchorMin = Vector2.zero;
        pctRect.anchorMax = Vector2.one;
        pctRect.offsetMin = Vector2.zero;
        pctRect.offsetMax = Vector2.zero;
        var pctTMP = pctObj.AddComponent<TextMeshProUGUI>();
        pctTMP.text = "0%";
        pctTMP.fontSize = 28;
        pctTMP.fontStyle = FontStyles.Bold;
        pctTMP.color = Color.white;
        pctTMP.alignment = TextAlignmentOptions.Center;
        pctTMP.enableWordWrapping = false;
        // Add outline for readability
        pctTMP.outlineWidth = 0.2f;
        pctTMP.outlineColor = new Color32(0, 0, 0, 200);
        pctObj.AddComponent<CanvasRenderer>();

        // ============================
        // 2. WIRE REFERENCES
        // ============================
        GameObject sp = GameObject.Find("ShootPoint");
        var sc = sp.GetComponent<ShootingController>();
        sc.powerBarFill = fillImg;
        sc.powerPercentText = pctTMP;
        sc.powerBarBG = bgImage;
        EditorUtility.SetDirty(sp);

        // ============================
        // 3. ALSO UPDATE INSTRUCTIONS TEXT
        // ============================
        Transform instrTf = canvas.transform.Find("InstructionsText");
        if (instrTf != null)
        {
            var instrTMP = instrTf.GetComponent<TextMeshProUGUI>();
            if (instrTMP != null)
            {
                instrTMP.text = "Mantén ESPACIO para cargar | Suelta para disparar";
                instrTMP.fontSize = 22;
                // Move instructions above the power bar
                var instrRect = instrTf.GetComponent<RectTransform>();
                instrRect.anchorMin = new Vector2(0.5f, 0f);
                instrRect.anchorMax = new Vector2(0.5f, 0f);
                instrRect.pivot = new Vector2(0.5f, 0f);
                instrRect.anchoredPosition = new Vector2(0, 85);
                instrRect.sizeDelta = new Vector2(600, 35);
                EditorUtility.SetDirty(instrTf.gameObject);
            }
        }

        // ============================
        // SAVE
        // ============================
        AssetDatabase.SaveAssets();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        Debug.Log("Power bar UI rebuilt - bigger, centered, with percentage and dynamic color!");
    }
}
