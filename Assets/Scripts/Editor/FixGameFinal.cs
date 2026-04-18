using UnityEngine;
using UnityEditor;

public class FixGameFinal
{
    public static void Execute()
    {
        Sprite squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/WhiteSquare.png");
        Sprite circleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Circle256.png");

        // ========================================
        // 1. FIX GAME BALANCE
        // ========================================
        // Max force should OVERSHOOT so player needs ~60-70% charge
        GameObject sp = GameObject.Find("ShootPoint");
        var sc = sp.GetComponent<ShootingController>();
        sc.maxForce = 50f;        // max power overshoots past diana
        sc.chargeSpeed = 18f;     // moderate charge speed - player must time it
        sc.minAngle = 5f;
        sc.maxAngle = 75f;
        EditorUtility.SetDirty(sp);

        // Arrow: more gravity so trajectory curves more
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var rb = prefab.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.6f;   // noticeable curve

        var ac = prefab.GetComponent<ArrowController>();
        ac.bullseyeRadius = 0.55f;
        EditorUtility.SetDirty(prefab);

        // ========================================
        // 2. IMPROVE BOW/ARCHER VISUAL
        // ========================================
        // Delete old bow
        DestroyIfExists("BowGroup");

        // Create new archer group
        GameObject archer = new GameObject("Archer");
        archer.transform.position = new Vector3(-7f, 0.5f, 0);

        // --- Archer body (torso) ---
        var torso = MakeChild(archer, "Torso", squareSprite,
            new Color(0.2f, 0.45f, 0.7f), // blue shirt
            new Vector3(0, 0.3f, 0), new Vector3(0.5f, 0.8f, 1), 3);

        // --- Archer head ---
        MakeChild(archer, "Head", circleSprite,
            new Color(0.95f, 0.78f, 0.6f), // skin
            new Vector3(0, 0.95f, 0), new Vector3(0.4f, 0.4f, 1), 4);

        // --- Hair ---
        MakeChild(archer, "Hair", circleSprite,
            new Color(0.25f, 0.15f, 0.05f), // dark brown
            new Vector3(0, 1.05f, 0), new Vector3(0.42f, 0.25f, 1), 5);

        // --- Archer legs ---
        MakeChild(archer, "LegL", squareSprite,
            new Color(0.3f, 0.3f, 0.35f), // dark pants
            new Vector3(-0.1f, -0.35f, 0), new Vector3(0.18f, 0.6f, 1), 2);
        MakeChild(archer, "LegR", squareSprite,
            new Color(0.3f, 0.3f, 0.35f),
            new Vector3(0.1f, -0.35f, 0), new Vector3(0.18f, 0.6f, 1), 2);

        // --- Arm (holding bow) ---
        MakeChild(archer, "ArmFront", squareSprite,
            new Color(0.95f, 0.78f, 0.6f), // skin
            new Vector3(0.35f, 0.4f, 0), new Vector3(0.5f, 0.1f, 1), 6);

        // --- Bow ---
        // Bow limb (curved brown piece - using circle squished)
        var bowLimb = MakeChild(archer, "BowLimb", circleSprite,
            new Color(0.6f, 0.3f, 0.05f),
            new Vector3(0.6f, 0.4f, 0), new Vector3(0.25f, 1.2f, 1), 7);

        // Bow string (thin white line)
        MakeChild(archer, "BowString", squareSprite,
            new Color(0.9f, 0.88f, 0.8f),
            new Vector3(0.5f, 0.4f, 0), new Vector3(0.02f, 1.1f, 1), 8);

        // --- Arrow resting on bow ---
        MakeChild(archer, "ArrowResting", squareSprite,
            new Color(0.5f, 0.28f, 0.08f),
            new Vector3(0.85f, 0.4f, 0), new Vector3(0.7f, 0.04f, 1), 9);
        
        // Arrow tip on bow
        MakeChild(archer, "ArrowRestTip", circleSprite,
            new Color(0.75f, 0.75f, 0.78f),
            new Vector3(1.2f, 0.4f, 0), new Vector3(0.08f, 0.08f, 1), 10);

        // Arrow fletching on bow
        MakeChild(archer, "ArrowRestFletch", circleSprite,
            new Color(0.9f, 0.2f, 0.2f),
            new Vector3(0.52f, 0.4f, 0), new Vector3(0.06f, 0.12f, 1), 10);

        // --- Ground platform for archer ---
        MakeChild(archer, "Platform", squareSprite,
            new Color(0.45f, 0.28f, 0.1f),
            new Vector3(0, -0.7f, 0), new Vector3(1f, 0.15f, 1), 1);

        // ========================================
        // 3. MAKE DIANA MORE CHALLENGING
        // ========================================
        GameObject diana = GameObject.Find("Diana");
        if (diana != null)
        {
            var tm = diana.GetComponent<TargetMovement>();
            if (tm != null)
            {
                tm.speed = 2.5f;    // moderate speed
                tm.leftLimit = 1f;
                tm.rightLimit = 8f;  // wider range
            }
            EditorUtility.SetDirty(diana);
        }

        // ========================================
        // Move shoot point to align with archer
        // ========================================
        sp.transform.position = new Vector3(-5.8f, 0.9f, 0);

        // ========================================
        // SAVE
        // ========================================
        AssetDatabase.SaveAssets();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("FINAL FIX COMPLETE! Balanced power, new archer, challenging diana.");
    }

    static GameObject MakeChild(GameObject parent, string name, Sprite sprite, Color color,
        Vector3 localPos, Vector3 localScale, int sortOrder)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent.transform);
        obj.transform.localPosition = localPos;
        obj.transform.localScale = localScale;
        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        sr.sortingOrder = sortOrder;
        return obj;
    }

    static void DestroyIfExists(string name)
    {
        var obj = GameObject.Find(name);
        if (obj != null) Object.DestroyImmediate(obj);
    }
}
