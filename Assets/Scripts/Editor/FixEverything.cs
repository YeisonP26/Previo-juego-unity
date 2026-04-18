using UnityEngine;
using UnityEditor;

public class FixEverything
{
    public static void Execute()
    {
        // ====== CREATE A WHITE SQUARE TEXTURE FOR BACKGROUNDS ======
        Texture2D whiteTex = new Texture2D(4, 4);
        Color[] pixels = new Color[16];
        for (int i = 0; i < 16; i++) pixels[i] = Color.white;
        whiteTex.SetPixels(pixels);
        whiteTex.Apply();
        byte[] pngData = whiteTex.EncodeToPNG();
        string texPath = "Assets/Sprites";
        if (!AssetDatabase.IsValidFolder(texPath))
            AssetDatabase.CreateFolder("Assets", "Sprites");
        System.IO.File.WriteAllBytes(Application.dataPath + "/Sprites/WhiteSquare.png", pngData);
        AssetDatabase.Refresh();

        // Set import settings for the white square
        TextureImporter importer = AssetImporter.GetAtPath("Assets/Sprites/WhiteSquare.png") as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.filterMode = FilterMode.Point;
            importer.SaveAndReimport();
        }

        Sprite whiteSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/WhiteSquare.png");
        Sprite circleSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

        // ====== DELETE OLD BACKGROUND OBJECTS ======
        DestroyIfExists("Background_Sky");
        DestroyIfExists("Background_Ground");
        DestroyIfExists("BowVisual");
        DestroyIfExists("AimIndicator");

        // ====== FIX CAMERA ======
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(0, 1, -10);
        cam.orthographicSize = 6;
        cam.backgroundColor = new Color(0.4f, 0.7f, 1f, 1f); // sky blue

        // ====== CREATE PROPER BACKGROUNDS ======
        // Sky - use square sprite
        GameObject sky = new GameObject("Background_Sky");
        var srSky = sky.AddComponent<SpriteRenderer>();
        srSky.sprite = whiteSprite;
        srSky.color = new Color(0.53f, 0.81f, 0.98f, 1f);
        sky.transform.position = new Vector3(0, 4, 10);
        sky.transform.localScale = new Vector3(25f, 12f, 1f);
        srSky.sortingOrder = -10;

        // Ground
        GameObject ground = new GameObject("Background_Ground");
        var srGround = ground.AddComponent<SpriteRenderer>();
        srGround.sprite = whiteSprite;
        srGround.color = new Color(0.2f, 0.6f, 0.15f, 1f);
        ground.transform.position = new Vector3(0, -3f, 10);
        ground.transform.localScale = new Vector3(25f, 6f, 1f);
        srGround.sortingOrder = -9;

        // Darker ground line
        GameObject groundLine = new GameObject("Background_GroundLine");
        var srGL = groundLine.AddComponent<SpriteRenderer>();
        srGL.sprite = whiteSprite;
        srGL.color = new Color(0.55f, 0.35f, 0.15f, 1f); // dirt/brown
        groundLine.transform.position = new Vector3(0, -0.5f, 10);
        groundLine.transform.localScale = new Vector3(25f, 0.5f, 1f);
        srGL.sortingOrder = -8;

        // ====== FIX DIANA - BIGGER AND BETTER ======
        GameObject diana = GameObject.Find("Diana");
        if (diana != null)
        {
            // Remove old children
            for (int i = diana.transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(diana.transform.GetChild(i).gameObject);

            diana.transform.position = new Vector3(4, 2.5f, 0);

            // Use TargetMovement settings
            var tm = diana.GetComponent<TargetMovement>();
            if (tm != null)
            {
                tm.speed = 2.5f;
                tm.leftLimit = 1f;
                tm.rightLimit = 7f;
            }

            // Ring 5 (outermost - red)
            CreateRing(diana, "Ring5_Red", circleSprite, new Color(0.8f, 0.1f, 0.1f), 3.0f, 0, "Target");
            // Ring 4 (white)
            CreateRing(diana, "Ring4_White", circleSprite, Color.white, 2.4f, 1, null);
            // Ring 3 (red)
            CreateRing(diana, "Ring3_Red", circleSprite, new Color(0.9f, 0.15f, 0.15f), 1.8f, 2, null);
            // Ring 2 (white)
            CreateRing(diana, "Ring2_White", circleSprite, Color.white, 1.2f, 3, null);
            // Ring 1 (red)
            CreateRing(diana, "Ring1_Red", circleSprite, new Color(1f, 0.2f, 0.2f), 0.7f, 4, null);
            // Bullseye (center - gold/yellow)
            CreateBullseye(diana, circleSprite);

            // Post behind diana
            GameObject post = new GameObject("TargetPost");
            post.transform.SetParent(diana.transform);
            post.transform.localPosition = new Vector3(0, -2.5f, 0);
            post.transform.localScale = new Vector3(0.15f, 3f, 1f);
            var srPost = post.AddComponent<SpriteRenderer>();
            srPost.sprite = whiteSprite;
            srPost.color = new Color(0.45f, 0.25f, 0.1f);
            srPost.sortingOrder = -2;

            EditorUtility.SetDirty(diana);
        }

        // ====== CREATE BOW VISUAL ======
        // Bow left side
        GameObject bowGroup = new GameObject("BowGroup");
        bowGroup.transform.position = new Vector3(-6.5f, 1f, 0);

        // Bow arc (using circle sprite, scaled to look like arc)
        GameObject bowArc = new GameObject("BowArc");
        bowArc.transform.SetParent(bowGroup.transform);
        bowArc.transform.localPosition = new Vector3(0, 0, 0);
        bowArc.transform.localScale = new Vector3(0.6f, 2f, 1f);
        var srArc = bowArc.AddComponent<SpriteRenderer>();
        srArc.sprite = circleSprite;
        srArc.color = new Color(0.55f, 0.27f, 0.07f, 1f);
        srArc.sortingOrder = 5;

        // Bow string (thin vertical line)
        GameObject bowString = new GameObject("BowString");
        bowString.transform.SetParent(bowGroup.transform);
        bowString.transform.localPosition = new Vector3(-0.15f, 0, 0);
        bowString.transform.localScale = new Vector3(0.03f, 2f, 1f);
        var srStr = bowString.AddComponent<SpriteRenderer>();
        srStr.sprite = whiteSprite;
        srStr.color = new Color(0.9f, 0.9f, 0.8f);
        srStr.sortingOrder = 6;

        // Arrow indicator at shoot point
        GameObject arrowInd = new GameObject("ArrowIndicator");
        arrowInd.transform.SetParent(bowGroup.transform);
        arrowInd.transform.localPosition = new Vector3(0.1f, 0, 0);
        arrowInd.transform.localScale = new Vector3(0.8f, 0.08f, 1f);
        var srInd = arrowInd.AddComponent<SpriteRenderer>();
        srInd.sprite = whiteSprite;
        srInd.color = new Color(0.55f, 0.27f, 0.07f);
        srInd.sortingOrder = 7;

        // Arrow tip indicator
        GameObject tipInd = new GameObject("ArrowTipInd");
        tipInd.transform.SetParent(bowGroup.transform);
        tipInd.transform.localPosition = new Vector3(0.5f, 0, 0);
        tipInd.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
        var srTipI = tipInd.AddComponent<SpriteRenderer>();
        srTipI.sprite = circleSprite;
        srTipI.color = new Color(0.7f, 0.7f, 0.7f);
        srTipI.sortingOrder = 8;

        // ====== FIX SHOOT POINT ======
        GameObject shootPoint = GameObject.Find("ShootPoint");
        if (shootPoint != null)
        {
            shootPoint.transform.position = new Vector3(-6f, 1f, 0);
            var sc = shootPoint.GetComponent<ShootingController>();
            if (sc != null)
            {
                sc.maxForce = 18f;
                sc.chargeSpeed = 12f;
            }
            EditorUtility.SetDirty(shootPoint);
        }

        // Save
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("EVERYTHING FIXED! Scene saved.");
    }

    static void CreateRing(GameObject parent, string name, Sprite sprite, Color color, float scale, int order, string tag)
    {
        GameObject ring = new GameObject(name);
        ring.transform.SetParent(parent.transform);
        ring.transform.localPosition = Vector3.zero;
        ring.transform.localScale = new Vector3(scale, scale, 1f);
        var sr = ring.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        sr.sortingOrder = order;

        if (tag == "Target")
        {
            var col = ring.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.5f;
            ring.tag = tag;
        }
    }

    static void CreateBullseye(GameObject parent, Sprite sprite)
    {
        GameObject bullseye = new GameObject("Bullseye");
        bullseye.transform.SetParent(parent.transform);
        bullseye.transform.localPosition = Vector3.zero;
        bullseye.transform.localScale = new Vector3(0.35f, 0.35f, 1f);
        var sr = bullseye.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = new Color(1f, 0.84f, 0f, 1f); // gold
        sr.sortingOrder = 5;

        var col = bullseye.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.5f;
        bullseye.tag = "Bullseye";
    }

    static void DestroyIfExists(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null) Object.DestroyImmediate(obj);
    }
}
