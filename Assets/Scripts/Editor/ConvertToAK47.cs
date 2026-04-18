using UnityEngine;
using UnityEditor;

public class ConvertToAK47
{
    [MenuItem("Tools/Convert To AK47")]
    public static void Execute()
    {
        // ── 1. Remove bow parts ──
        string[] toRemove = { "Archer/BowLimb", "Archer/BowString", "Archer/ArrowResting", "Archer/ArrowRestTip", "Archer/ArrowRestFletch" };
        foreach (var path in toRemove)
        {
            var go = GameObject.Find(path);
            if (go != null) Object.DestroyImmediate(go);
        }
        Debug.Log("Removed bow parts");

        // ── References ──
        var archer = GameObject.Find("Archer");
        if (archer == null) { Debug.LogError("Archer not found!"); return; }

        var squareSpr = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/WhiteSquare.png");
        var circleSpr = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Circle256.png");

        // ── 2. Reposition arm to hold gun ──
        var armFront = GameObject.Find("Archer/ArmFront");
        if (armFront != null)
        {
            armFront.transform.localPosition = new Vector3(0.55f, 0.42f, 0f);
            armFront.transform.localScale = new Vector3(0.45f, 0.12f, 1f);
            var armSR = armFront.GetComponent<SpriteRenderer>();
            armSR.sortingOrder = 7;
        }

        // ── 3. Build AK47 parts ──
        // Gun Body (receiver) - dark grey
        var gunBody = new GameObject("GunBody");
        gunBody.transform.SetParent(archer.transform);
        gunBody.transform.localPosition = new Vector3(0.75f, 0.45f, 0f);
        gunBody.transform.localScale = new Vector3(0.7f, 0.18f, 1f);
        var bodySR = gunBody.AddComponent<SpriteRenderer>();
        bodySR.sprite = squareSpr;
        bodySR.color = new Color(0.2f, 0.2f, 0.22f, 1f);
        bodySR.sortingOrder = 8;

        // Barrel - long thin dark tube
        var barrel = new GameObject("GunBarrel");
        barrel.transform.SetParent(archer.transform);
        barrel.transform.localPosition = new Vector3(1.35f, 0.47f, 0f);
        barrel.transform.localScale = new Vector3(0.55f, 0.07f, 1f);
        var barrelSR = barrel.AddComponent<SpriteRenderer>();
        barrelSR.sprite = squareSpr;
        barrelSR.color = new Color(0.15f, 0.15f, 0.17f, 1f);
        barrelSR.sortingOrder = 9;

        // Front Sight - small square at barrel tip
        var frontSight = new GameObject("FrontSight");
        frontSight.transform.SetParent(archer.transform);
        frontSight.transform.localPosition = new Vector3(1.62f, 0.52f, 0f);
        frontSight.transform.localScale = new Vector3(0.03f, 0.08f, 1f);
        var fsSR = frontSight.AddComponent<SpriteRenderer>();
        fsSR.sprite = squareSpr;
        fsSR.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        fsSR.sortingOrder = 10;

        // Stock - wooden (brown)
        var stock = new GameObject("GunStock");
        stock.transform.SetParent(archer.transform);
        stock.transform.localPosition = new Vector3(0.2f, 0.4f, 0f);
        stock.transform.localScale = new Vector3(0.45f, 0.14f, 1f);
        var stockSR = stock.AddComponent<SpriteRenderer>();
        stockSR.sprite = squareSpr;
        stockSR.color = new Color(0.55f, 0.35f, 0.15f, 1f);
        stockSR.sortingOrder = 6;

        // Stock end - rounded
        var stockEnd = new GameObject("StockEnd");
        stockEnd.transform.SetParent(archer.transform);
        stockEnd.transform.localPosition = new Vector3(0.0f, 0.4f, 0f);
        stockEnd.transform.localScale = new Vector3(0.14f, 0.14f, 1f);
        var seSR = stockEnd.AddComponent<SpriteRenderer>();
        seSR.sprite = circleSpr;
        seSR.color = new Color(0.5f, 0.3f, 0.12f, 1f);
        seSR.sortingOrder = 6;

        // Pistol Grip
        var grip = new GameObject("GunGrip");
        grip.transform.SetParent(archer.transform);
        grip.transform.localPosition = new Vector3(0.6f, 0.3f, 0f);
        grip.transform.localScale = new Vector3(0.1f, 0.2f, 1f);
        grip.transform.localRotation = Quaternion.Euler(0, 0, -10f);
        var gripSR = grip.AddComponent<SpriteRenderer>();
        gripSR.sprite = squareSpr;
        gripSR.color = new Color(0.2f, 0.2f, 0.22f, 1f);
        gripSR.sortingOrder = 7;

        // Magazine (banana mag)
        var mag = new GameObject("GunMagazine");
        mag.transform.SetParent(archer.transform);
        mag.transform.localPosition = new Vector3(0.82f, 0.28f, 0f);
        mag.transform.localScale = new Vector3(0.12f, 0.25f, 1f);
        mag.transform.localRotation = Quaternion.Euler(0, 0, 8f);
        var magSR = mag.AddComponent<SpriteRenderer>();
        magSR.sprite = squareSpr;
        magSR.color = new Color(0.25f, 0.25f, 0.27f, 1f);
        magSR.sortingOrder = 7;

        // Gas tube (above barrel)
        var gasTube = new GameObject("GasTube");
        gasTube.transform.SetParent(archer.transform);
        gasTube.transform.localPosition = new Vector3(1.15f, 0.52f, 0f);
        gasTube.transform.localScale = new Vector3(0.35f, 0.05f, 1f);
        var gtSR = gasTube.AddComponent<SpriteRenderer>();
        gtSR.sprite = squareSpr;
        gtSR.color = new Color(0.5f, 0.3f, 0.12f, 1f);
        gtSR.sortingOrder = 9;

        // Muzzle (circle at barrel end)
        var muzzle = new GameObject("Muzzle");
        muzzle.transform.SetParent(archer.transform);
        muzzle.transform.localPosition = new Vector3(1.65f, 0.47f, 0f);
        muzzle.transform.localScale = new Vector3(0.09f, 0.09f, 1f);
        var muzSR = muzzle.AddComponent<SpriteRenderer>();
        muzSR.sprite = circleSpr;
        muzSR.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        muzSR.sortingOrder = 10;

        Debug.Log("AK47 built on Archer");

        // ── 4. Update ShootPoint position to match barrel end ──
        // Archer is at world (-7, 0.5). Muzzle local (1.65, 0.47) → world (-5.35, 0.97)
        var shootPoint = GameObject.Find("ShootPoint");
        if (shootPoint != null)
        {
            shootPoint.transform.position = new Vector3(-5.35f, 0.97f, 0f);
        }

        // ── 5. Rebuild Arrow prefab as Bullet ──
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab != null)
        {
            // Edit this prefab
            var prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

            // Remove old child sprites
            for (int i = prefabRoot.transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(prefabRoot.transform.GetChild(i).gameObject);
            }

            // Update Rigidbody2D for bullet physics
            var rb = prefabRoot.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0.15f; // bullets drop much less
                rb.mass = 0.1f;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }

            // Update collider
            var col = prefabRoot.GetComponent<CircleCollider2D>();
            if (col != null)
            {
                col.radius = 0.08f;
                col.offset = new Vector2(0.1f, 0f);
                col.isTrigger = true;
            }

            // Create bullet body (brass/golden elongated shape)
            var bulletBody = new GameObject("BulletBody");
            bulletBody.transform.SetParent(prefabRoot.transform);
            bulletBody.transform.localPosition = Vector3.zero;
            bulletBody.transform.localScale = new Vector3(0.25f, 0.08f, 1f);
            var bbSR = bulletBody.AddComponent<SpriteRenderer>();
            bbSR.sprite = squareSpr;
            bbSR.color = new Color(0.85f, 0.7f, 0.2f, 1f); // brass
            bbSR.sortingOrder = 15;

            // Bullet tip
            var bulletTip = new GameObject("BulletTip");
            bulletTip.transform.SetParent(prefabRoot.transform);
            bulletTip.transform.localPosition = new Vector3(0.13f, 0f, 0f);
            bulletTip.transform.localScale = new Vector3(0.09f, 0.08f, 1f);
            var btSR = bulletTip.AddComponent<SpriteRenderer>();
            btSR.sprite = circleSpr;
            btSR.color = new Color(0.75f, 0.55f, 0.15f, 1f); // darker brass tip
            btSR.sortingOrder = 15;

            // Casing base
            var bulletBase = new GameObject("BulletBase");
            bulletBase.transform.SetParent(prefabRoot.transform);
            bulletBase.transform.localPosition = new Vector3(-0.13f, 0f, 0f);
            bulletBase.transform.localScale = new Vector3(0.04f, 0.09f, 1f);
            var baseSR = bulletBase.AddComponent<SpriteRenderer>();
            baseSR.sprite = squareSpr;
            baseSR.color = new Color(0.7f, 0.55f, 0.15f, 1f);
            baseSR.sortingOrder = 15;

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            Debug.Log("Bullet prefab saved");
        }

        // ── 6. Update ShootingController values for gun feel ──
        var sc = shootPoint.GetComponent<ShootingController>();
        if (sc != null)
        {
            sc.maxForce = 70f;   // bullets are faster
            sc.chargeSpeed = 25f; // charges faster
        }

        EditorUtility.SetDirty(archer);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log("✓ AK47 conversion complete!");
    }
}
