using UnityEngine;
using UnityEditor;

public class AddTwinTowers
{
    [MenuItem("Tools/Add Twin Towers")]
    public static void Execute()
    {
        var sq = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/WhiteSquare.png");
        var ci = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Circle256.png");

        float towerZ = 1.96f; // behind flags, in front of sky
        float baseY = -0.5f;
        float towerH = 7f;
        float towerW = 1.0f;
        float gap = 0.35f;
        float centerX = 0f; // centered in the background

        var towersParent = new GameObject("TwinTowers");
        towersParent.transform.position = Vector3.zero;

        // ── Tower 1 (left) ──
        var tower1 = new GameObject("Tower1");
        tower1.transform.SetParent(towersParent.transform);
        tower1.transform.localPosition = new Vector3(centerX - (towerW / 2f + gap / 2f), baseY + towerH / 2f, towerZ);
        tower1.transform.localScale = new Vector3(towerW, towerH, 1f);
        var t1SR = tower1.AddComponent<SpriteRenderer>();
        t1SR.sprite = sq;
        t1SR.color = new Color(0.55f, 0.55f, 0.58f, 0.85f);
        t1SR.sortingOrder = -9;

        // Tower 1 - antenna
        var ant1 = new GameObject("Tower1_Antenna");
        ant1.transform.SetParent(towersParent.transform);
        ant1.transform.localPosition = new Vector3(centerX - (towerW / 2f + gap / 2f), baseY + towerH + 0.6f, towerZ);
        ant1.transform.localScale = new Vector3(0.04f, 1.2f, 1f);
        var a1SR = ant1.AddComponent<SpriteRenderer>();
        a1SR.sprite = sq;
        a1SR.color = new Color(0.4f, 0.4f, 0.42f, 0.85f);
        a1SR.sortingOrder = -9;

        // ── Tower 2 (right) ──
        var tower2 = new GameObject("Tower2");
        tower2.transform.SetParent(towersParent.transform);
        tower2.transform.localPosition = new Vector3(centerX + (towerW / 2f + gap / 2f), baseY + towerH / 2f, towerZ);
        tower2.transform.localScale = new Vector3(towerW, towerH, 1f);
        var t2SR = tower2.AddComponent<SpriteRenderer>();
        t2SR.sprite = sq;
        t2SR.color = new Color(0.52f, 0.52f, 0.55f, 0.85f);
        t2SR.sortingOrder = -9;

        // ── Window rows (horizontal lines on both towers) ──
        for (int t = 0; t < 2; t++)
        {
            float tx = (t == 0)
                ? centerX - (towerW / 2f + gap / 2f)
                : centerX + (towerW / 2f + gap / 2f);

            for (int row = 0; row < 18; row++)
            {
                var winRow = new GameObject("WinRow_T" + t + "_" + row);
                winRow.transform.SetParent(towersParent.transform);
                float ry = baseY + 0.5f + row * 0.38f;
                winRow.transform.localPosition = new Vector3(tx, ry, towerZ - 0.01f);
                winRow.transform.localScale = new Vector3(towerW * 0.9f, 0.04f, 1f);
                var wrSR = winRow.AddComponent<SpriteRenderer>();
                wrSR.sprite = sq;
                wrSR.color = new Color(0.35f, 0.38f, 0.42f, 0.7f);
                wrSR.sortingOrder = -8;
            }

            // Vertical window columns
            for (int col = 0; col < 5; col++)
            {
                var winCol = new GameObject("WinCol_T" + t + "_" + col);
                winCol.transform.SetParent(towersParent.transform);
                float cx = tx - towerW * 0.4f + col * (towerW * 0.8f / 4f);
                winCol.transform.localPosition = new Vector3(cx, baseY + towerH / 2f, towerZ - 0.01f);
                winCol.transform.localScale = new Vector3(0.03f, towerH * 0.95f, 1f);
                var wcSR = winCol.AddComponent<SpriteRenderer>();
                wcSR.sprite = sq;
                wcSR.color = new Color(0.38f, 0.38f, 0.4f, 0.6f);
                wcSR.sortingOrder = -8;
            }
        }

        // ── Roof details ──
        for (int t = 0; t < 2; t++)
        {
            float tx = (t == 0)
                ? centerX - (towerW / 2f + gap / 2f)
                : centerX + (towerW / 2f + gap / 2f);

            var roof = new GameObject("Roof_T" + t);
            roof.transform.SetParent(towersParent.transform);
            roof.transform.localPosition = new Vector3(tx, baseY + towerH + 0.05f, towerZ - 0.01f);
            roof.transform.localScale = new Vector3(towerW * 1.05f, 0.1f, 1f);
            var rSR = roof.AddComponent<SpriteRenderer>();
            rSR.sprite = sq;
            rSR.color = new Color(0.45f, 0.45f, 0.48f, 0.9f);
            rSR.sortingOrder = -8;
        }

        // ── Sky bridge / plaza between towers at base ──
        var bridge = new GameObject("SkyBridge");
        bridge.transform.SetParent(towersParent.transform);
        bridge.transform.localPosition = new Vector3(centerX, baseY + 1.5f, towerZ - 0.01f);
        bridge.transform.localScale = new Vector3(gap + towerW, 0.5f, 1f);
        var brSR = bridge.AddComponent<SpriteRenderer>();
        brSR.sprite = sq;
        brSR.color = new Color(0.48f, 0.48f, 0.5f, 0.7f);
        brSR.sortingOrder = -9;

        // ── Subtle light glow at base ──
        var glow = new GameObject("TowerGlow");
        glow.transform.SetParent(towersParent.transform);
        glow.transform.localPosition = new Vector3(centerX, baseY + 0.5f, towerZ + 0.01f);
        glow.transform.localScale = new Vector3(3f, 1.5f, 1f);
        var glSR = glow.AddComponent<SpriteRenderer>();
        glSR.sprite = ci;
        glSR.color = new Color(1f, 0.85f, 0.5f, 0.1f);
        glSR.sortingOrder = -10;

        Debug.Log("Twin Towers added to background");

        EditorUtility.SetDirty(towersParent);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
}
