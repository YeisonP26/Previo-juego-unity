using UnityEngine;
using UnityEditor;

public class FixAll2
{
    public static void Execute()
    {
        // ====== CREATE PROPER TEXTURES ======
        string spritesPath = "Assets/Sprites";
        if (!AssetDatabase.IsValidFolder(spritesPath))
            AssetDatabase.CreateFolder("Assets", "Sprites");

        // Create 256x256 white square
        CreateTexture("WhiteSquare", 256, 256, Color.white, 1f);
        // Create 256x256 circle for diana rings
        CreateCircleTexture("Circle256", 256, Color.white);

        AssetDatabase.Refresh();

        // Set PPU to 100 for WhiteSquare
        SetSpriteImport("Assets/Sprites/WhiteSquare.png", 256);
        // Set PPU to 256 for Circle so 1 unit = full circle
        SetSpriteImport("Assets/Sprites/Circle256.png", 256);

        AssetDatabase.Refresh();

        Sprite squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/WhiteSquare.png");
        Sprite circleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Circle256.png");

        if (squareSprite == null || circleSprite == null)
        {
            Debug.LogError("Sprites not loaded! square=" + (squareSprite != null) + " circle=" + (circleSprite != null));
            return;
        }

        // ====== CLEAN UP EVERYTHING ======
        string[] toDelete = {
            "Background_Sky", "Background_Ground", "Background_GroundLine",
            "BowVisual", "AimIndicator", "BowGroup"
        };
        foreach (var name in toDelete)
        {
            var obj = GameObject.Find(name);
            if (obj != null) Object.DestroyImmediate(obj);
        }

        // ====== CAMERA SETUP ======
        // ortho size 6 => viewport is 12 units tall, ~21.3 units wide at 16:9
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(0, 1, -10);
        cam.orthographicSize = 6f;
        cam.backgroundColor = new Color(0.3f, 0.6f, 1f);

        // ====== BACKGROUND ======
        // Sky
        GameObject sky = CreateSprite("Background_Sky", squareSprite,
            new Color(0.4f, 0.75f, 1f), new Vector3(0, 5, 2), new Vector3(25, 14, 1), -10);

        // Ground (grass)
        GameObject ground = CreateSprite("Background_Ground", squareSprite,
            new Color(0.18f, 0.55f, 0.12f), new Vector3(0, -3.5f, 2), new Vector3(25, 5, 1), -9);

        // Dirt line
        GameObject dirt = CreateSprite("Background_Dirt", squareSprite,
            new Color(0.5f, 0.3f, 0.1f), new Vector3(0, -1.1f, 2), new Vector3(25, 0.3f, 1), -8);

        // ====== FIX DIANA ======
        GameObject diana = GameObject.Find("Diana");
        if (diana != null)
        {
            // Remove ALL children
            for (int i = diana.transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(diana.transform.GetChild(i).gameObject);

            diana.transform.position = new Vector3(4, 2.5f, 0);
            diana.transform.localScale = Vector3.one;

            var tm = diana.GetComponent<TargetMovement>();
            if (tm != null)
            {
                tm.speed = 2f;
                tm.leftLimit = 0f;
                tm.rightLimit = 7f;
            }

            // Diana board background (white circle, big)
            CreateChildSprite(diana, "Board", circleSprite,
                new Color(0.95f, 0.95f, 0.9f), Vector3.zero, 3.5f, -1);

            // Ring 4 - outermost (red)
            var ring4 = CreateChildSprite(diana, "Ring4_Red", circleSprite,
                new Color(0.85f, 0.1f, 0.1f), Vector3.zero, 3.3f, 0);
            var col4 = ring4.AddComponent<CircleCollider2D>();
            col4.isTrigger = true;
            col4.radius = 0.5f;
            ring4.tag = "Target";

            // Ring 3 (white)
            CreateChildSprite(diana, "Ring3_White", circleSprite,
                Color.white, Vector3.zero, 2.5f, 1);

            // Ring 2 (red)
            CreateChildSprite(diana, "Ring2_Red", circleSprite,
                new Color(0.9f, 0.15f, 0.15f), Vector3.zero, 1.7f, 2);

            // Ring 1 (white)
            CreateChildSprite(diana, "Ring1_White", circleSprite,
                Color.white, Vector3.zero, 1.0f, 3);

            // Bullseye center (gold/yellow)
            var bullseye = CreateChildSprite(diana, "Bullseye", circleSprite,
                new Color(1f, 0.85f, 0f), Vector3.zero, 0.5f, 4);
            var bullCol = bullseye.AddComponent<CircleCollider2D>();
            bullCol.isTrigger = true;
            bullCol.radius = 0.5f;
            bullseye.tag = "Bullseye";

            // Support post
            CreateChildSprite(diana, "Post", squareSprite,
                new Color(0.45f, 0.25f, 0.08f), new Vector3(0, -3f, 0.1f), 1f, -3);
            diana.transform.Find("Post").localScale = new Vector3(0.2f, 4f, 1f);

            EditorUtility.SetDirty(diana);
        }

        // ====== ARCHER / BOW ======
        GameObject bowGroup = new GameObject("BowGroup");
        bowGroup.transform.position = new Vector3(-7f, 1f, 0);

        // Bow body (brown arc)
        var bowBody = CreateChildSprite(bowGroup, "BowBody", circleSprite,
            new Color(0.6f, 0.3f, 0.05f), Vector3.zero, 1f, 5);
        bowBody.transform.localScale = new Vector3(0.5f, 2f, 1f);

        // Bow string
        var bowStr = CreateChildSprite(bowGroup, "BowString", squareSprite,
            new Color(0.85f, 0.85f, 0.75f), new Vector3(-0.12f, 0, 0), 1f, 6);
        bowStr.transform.localScale = new Vector3(0.03f, 1.8f, 1f);

        // Arrow on bow
        var arrowVis = CreateChildSprite(bowGroup, "ArrowOnBow", squareSprite,
            new Color(0.55f, 0.27f, 0.07f), new Vector3(0.4f, 0, 0), 1f, 7);
        arrowVis.transform.localScale = new Vector3(1f, 0.06f, 1f);

        // Arrow tip
        var tipVis = CreateChildSprite(bowGroup, "ArrowTip", circleSprite,
            new Color(0.75f, 0.75f, 0.75f), new Vector3(0.9f, 0, 0), 0.12f, 8);

        // ====== SHOOT POINT ======
        GameObject shootPoint = GameObject.Find("ShootPoint");
        if (shootPoint != null)
        {
            shootPoint.transform.position = new Vector3(-6f, 1f, 0);
            var sc = shootPoint.GetComponent<ShootingController>();
            if (sc != null)
            {
                sc.maxForce = 20f;
                sc.chargeSpeed = 14f;
            }
            EditorUtility.SetDirty(shootPoint);
        }

        // ====== SAVE ======
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        Debug.Log("FIX ALL 2 COMPLETE!");
    }

    static void CreateTexture(string name, int w, int h, Color color, float alpha)
    {
        Texture2D tex = new Texture2D(w, h);
        Color[] pixels = new Color[w * h];
        Color c = new Color(color.r, color.g, color.b, alpha);
        for (int i = 0; i < pixels.Length; i++) pixels[i] = c;
        tex.SetPixels(pixels);
        tex.Apply();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Sprites/" + name + ".png", tex.EncodeToPNG());
    }

    static void CreateCircleTexture(string name, int size, Color color)
    {
        Texture2D tex = new Texture2D(size, size);
        float center = size / 2f;
        float radius = size / 2f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist <= radius)
                    tex.SetPixel(x, y, color);
                else
                    tex.SetPixel(x, y, Color.clear);
            }
        }
        tex.Apply();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Sprites/" + name + ".png", tex.EncodeToPNG());
    }

    static void SetSpriteImport(string path, float ppu)
    {
        TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (imp != null)
        {
            imp.textureType = TextureImporterType.Sprite;
            imp.spriteImportMode = SpriteImportMode.Single;
            imp.spritePixelsPerUnit = ppu;
            imp.filterMode = FilterMode.Bilinear;
            imp.alphaIsTransparency = true;
            imp.SaveAndReimport();
        }
    }

    static GameObject CreateSprite(string name, Sprite sprite, Color color, Vector3 pos, Vector3 scale, int order)
    {
        GameObject obj = new GameObject(name);
        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        sr.sortingOrder = order;
        obj.transform.position = pos;
        obj.transform.localScale = scale;
        return obj;
    }

    static GameObject CreateChildSprite(GameObject parent, string name, Sprite sprite, Color color, Vector3 localPos, float uniformScale, int order)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent.transform);
        obj.transform.localPosition = localPos;
        obj.transform.localScale = new Vector3(uniformScale, uniformScale, 1f);
        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        sr.sortingOrder = order;
        return obj;
    }
}
