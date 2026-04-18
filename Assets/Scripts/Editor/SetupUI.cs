using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class SetupUI
{
    public static void Execute()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        var scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // === Score Text ===
        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(canvasObj.transform, false);
        var scoreTMP = scoreObj.AddComponent<TextMeshProUGUI>();
        scoreTMP.text = "Puntos: 0";
        scoreTMP.fontSize = 48;
        scoreTMP.color = Color.white;
        scoreTMP.fontStyle = FontStyles.Bold;
        scoreTMP.alignment = TextAlignmentOptions.TopLeft;
        var scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.pivot = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(30, -20);
        scoreRect.sizeDelta = new Vector2(400, 70);

        // Add outline for better readability
        var scoreOutline = scoreObj.AddComponent<Outline>();
        scoreOutline.effectColor = Color.black;
        scoreOutline.effectDistance = new Vector2(2, -2);

        // === Message Text (center) ===
        GameObject msgObj = new GameObject("MessageText");
        msgObj.transform.SetParent(canvasObj.transform, false);
        var msgTMP = msgObj.AddComponent<TextMeshProUGUI>();
        msgTMP.text = "";
        msgTMP.fontSize = 56;
        msgTMP.color = Color.yellow;
        msgTMP.fontStyle = FontStyles.Bold;
        msgTMP.alignment = TextAlignmentOptions.Center;
        var msgRect = msgObj.GetComponent<RectTransform>();
        msgRect.anchorMin = new Vector2(0.5f, 0.5f);
        msgRect.anchorMax = new Vector2(0.5f, 0.5f);
        msgRect.pivot = new Vector2(0.5f, 0.5f);
        msgRect.anchoredPosition = new Vector2(0, 150);
        msgRect.sizeDelta = new Vector2(600, 80);

        // === Power Bar Background ===
        GameObject barBg = new GameObject("PowerBarBG");
        barBg.transform.SetParent(canvasObj.transform, false);
        var bgImage = barBg.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        var barBgRect = barBg.GetComponent<RectTransform>();
        barBgRect.anchorMin = new Vector2(0, 0);
        barBgRect.anchorMax = new Vector2(0, 0);
        barBgRect.pivot = new Vector2(0, 0);
        barBgRect.anchoredPosition = new Vector2(30, 30);
        barBgRect.sizeDelta = new Vector2(300, 40);

        // === Power Bar Fill ===
        GameObject barFill = new GameObject("PowerBarFill");
        barFill.transform.SetParent(barBg.transform, false);
        var fillImage = barFill.AddComponent<Image>();
        fillImage.color = new Color(0.1f, 0.8f, 0.1f, 1f);
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillAmount = 0f;
        var fillRect = barFill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = new Vector2(4, 4);
        fillRect.offsetMax = new Vector2(-4, -4);

        // === Power Bar Label ===
        GameObject barLabel = new GameObject("PowerBarLabel");
        barLabel.transform.SetParent(barBg.transform, false);
        var labelTMP = barLabel.AddComponent<TextMeshProUGUI>();
        labelTMP.text = "POTENCIA";
        labelTMP.fontSize = 22;
        labelTMP.color = Color.white;
        labelTMP.fontStyle = FontStyles.Bold;
        labelTMP.alignment = TextAlignmentOptions.Center;
        var labelRect = barLabel.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        // === Instructions Text ===
        GameObject instrObj = new GameObject("InstructionsText");
        instrObj.transform.SetParent(canvasObj.transform, false);
        var instrTMP = instrObj.AddComponent<TextMeshProUGUI>();
        instrTMP.text = "Mantén ESPACIO para cargar - Suelta para disparar";
        instrTMP.fontSize = 28;
        instrTMP.color = new Color(1f, 1f, 1f, 0.7f);
        instrTMP.alignment = TextAlignmentOptions.BottomRight;
        var instrRect = instrObj.GetComponent<RectTransform>();
        instrRect.anchorMin = new Vector2(1, 0);
        instrRect.anchorMax = new Vector2(1, 0);
        instrRect.pivot = new Vector2(1, 0);
        instrRect.anchoredPosition = new Vector2(-30, 30);
        instrRect.sizeDelta = new Vector2(600, 50);

        // Create EventSystem if not exists
        if (Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }

        Debug.Log("UI setup complete!");
    }
}
