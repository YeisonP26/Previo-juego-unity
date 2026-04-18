using UnityEngine;
using UnityEditor;

public class FixArrowPrefab
{
    public static void Execute()
    {
        Sprite squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/WhiteSquare.png");
        Sprite circleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Circle256.png");

        if (squareSprite == null || circleSprite == null)
        {
            Debug.LogError("Sprites not found!");
            return;
        }

        // Load existing prefab
        string prefabPath = "Assets/Prefabs/Arrow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError("Arrow prefab not found!");
            return;
        }

        // Open prefab for editing
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        // Remove old children
        for (int i = instance.transform.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(instance.transform.GetChild(i).gameObject);

        // Remove old SpriteRenderers if any
        foreach (var sr in instance.GetComponents<SpriteRenderer>())
            Object.DestroyImmediate(sr);

        // Arrow shaft (brown rectangle)
        GameObject shaft = new GameObject("Shaft");
        shaft.transform.SetParent(instance.transform);
        shaft.transform.localPosition = Vector3.zero;
        shaft.transform.localScale = new Vector3(0.6f, 0.06f, 1f);
        var srShaft = shaft.AddComponent<SpriteRenderer>();
        srShaft.sprite = squareSprite;
        srShaft.color = new Color(0.55f, 0.27f, 0.07f);
        srShaft.sortingOrder = 10;

        // Arrow tip (silver triangle - using circle)
        GameObject tip = new GameObject("Tip");
        tip.transform.SetParent(instance.transform);
        tip.transform.localPosition = new Vector3(0.35f, 0, 0);
        tip.transform.localScale = new Vector3(0.12f, 0.12f, 1f);
        var srTip = tip.AddComponent<SpriteRenderer>();
        srTip.sprite = circleSprite;
        srTip.color = new Color(0.8f, 0.8f, 0.8f);
        srTip.sortingOrder = 11;

        // Arrow fletching (green feathers)
        GameObject fletch = new GameObject("Fletching");
        fletch.transform.SetParent(instance.transform);
        fletch.transform.localPosition = new Vector3(-0.3f, 0, 0);
        fletch.transform.localScale = new Vector3(0.08f, 0.15f, 1f);
        var srFletch = fletch.AddComponent<SpriteRenderer>();
        srFletch.sprite = circleSprite;
        srFletch.color = new Color(0.1f, 0.7f, 0.1f);
        srFletch.sortingOrder = 11;

        // Fix Rigidbody2D
        var rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0.8f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Fix collider
        var col = instance.GetComponent<CircleCollider2D>();
        if (col != null)
        {
            col.radius = 0.1f;
            col.offset = new Vector2(0.35f, 0f);
        }

        // Save back to prefab
        PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
        Object.DestroyImmediate(instance);

        Debug.Log("Arrow prefab fixed!");
    }
}
