using UnityEngine;
using UnityEditor;

public class SetupArrowPrefab
{
    public static void Execute()
    {
        // Create arrow GameObject
        GameObject arrow = new GameObject("Arrow");

        // Load circle/knob sprite as base
        Sprite knobSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

        // Arrow body (elongated)
        GameObject body = new GameObject("Body");
        body.transform.parent = arrow.transform;
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = new Vector3(0.8f, 0.12f, 1f);
        var srBody = body.AddComponent<SpriteRenderer>();
        srBody.sprite = knobSprite;
        srBody.color = new Color(0.55f, 0.27f, 0.07f, 1f); // brown
        srBody.sortingOrder = 10;

        // Arrow tip (triangle-like)
        GameObject tip = new GameObject("Tip");
        tip.transform.parent = arrow.transform;
        tip.transform.localPosition = new Vector3(0.45f, 0, 0);
        tip.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        var srTip = tip.AddComponent<SpriteRenderer>();
        srTip.sprite = knobSprite;
        srTip.color = new Color(0.7f, 0.7f, 0.7f, 1f); // silver
        srTip.sortingOrder = 11;

        // Arrow fletching (tail)
        GameObject fletch = new GameObject("Fletching");
        fletch.transform.parent = arrow.transform;
        fletch.transform.localPosition = new Vector3(-0.45f, 0, 0);
        fletch.transform.localScale = new Vector3(0.15f, 0.25f, 1f);
        var srFletch = fletch.AddComponent<SpriteRenderer>();
        srFletch.sprite = knobSprite;
        srFletch.color = new Color(0.1f, 0.6f, 0.1f, 1f); // green
        srFletch.sortingOrder = 11;

        // Add Rigidbody2D with gravity
        var rb = arrow.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Add CircleCollider2D trigger on the tip area
        var col = arrow.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.15f;
        col.offset = new Vector2(0.45f, 0f);

        // Add ArrowController script
        arrow.AddComponent<ArrowController>();

        // Save as prefab
        string prefabPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        PrefabUtility.SaveAsPrefabAsset(arrow, prefabPath + "/Arrow.prefab");
        Object.DestroyImmediate(arrow);

        Debug.Log("Arrow prefab created at Assets/Prefabs/Arrow.prefab");
    }
}
