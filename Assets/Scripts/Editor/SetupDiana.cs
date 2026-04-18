using UnityEngine;
using UnityEditor;

public class SetupDiana
{
    public static void Execute()
    {
        // Get the Diana parent
        GameObject diana = GameObject.Find("Diana");
        if (diana == null)
        {
            Debug.LogError("Diana not found!");
            return;
        }

        // Remove any SpriteRenderer from Diana parent
        var parentSR = diana.GetComponent<SpriteRenderer>();
        if (parentSR != null) Object.DestroyImmediate(parentSR);

        // Load the default circle sprite
        Sprite circleSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

        // Ring 5 - outermost (red)
        GameObject ring5 = new GameObject("Ring5_Red");
        ring5.transform.parent = diana.transform;
        ring5.transform.localPosition = Vector3.zero;
        ring5.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        var sr5 = ring5.AddComponent<SpriteRenderer>();
        sr5.sprite = circleSprite;
        sr5.color = new Color(0.8f, 0.1f, 0.1f, 1f);
        sr5.sortingOrder = 0;

        // Ring 4 (white)
        GameObject ring4 = new GameObject("Ring4_White");
        ring4.transform.parent = diana.transform;
        ring4.transform.localPosition = Vector3.zero;
        ring4.transform.localScale = new Vector3(2.0f, 2.0f, 1f);
        var sr4 = ring4.AddComponent<SpriteRenderer>();
        sr4.sprite = circleSprite;
        sr4.color = Color.white;
        sr4.sortingOrder = 1;

        // Ring 3 (red)
        GameObject ring3 = new GameObject("Ring3_Red");
        ring3.transform.parent = diana.transform;
        ring3.transform.localPosition = Vector3.zero;
        ring3.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        var sr3 = ring3.AddComponent<SpriteRenderer>();
        sr3.sprite = circleSprite;
        sr3.color = new Color(0.8f, 0.1f, 0.1f, 1f);
        sr3.sortingOrder = 2;

        // Ring 2 (white)
        GameObject ring2 = new GameObject("Ring2_White");
        ring2.transform.parent = diana.transform;
        ring2.transform.localPosition = Vector3.zero;
        ring2.transform.localScale = new Vector3(1.0f, 1.0f, 1f);
        var sr2 = ring2.AddComponent<SpriteRenderer>();
        sr2.sprite = circleSprite;
        sr2.color = Color.white;
        sr2.sortingOrder = 3;

        // Bullseye (center - gold)
        GameObject bullseye = new GameObject("Bullseye");
        bullseye.transform.parent = diana.transform;
        bullseye.transform.localPosition = Vector3.zero;
        bullseye.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        var srB = bullseye.AddComponent<SpriteRenderer>();
        srB.sprite = circleSprite;
        srB.color = new Color(1f, 0.84f, 0f, 1f);
        srB.sortingOrder = 4;

        // Bullseye collider (trigger, small)
        var bullseyeCol = bullseye.AddComponent<CircleCollider2D>();
        bullseyeCol.isTrigger = true;
        bullseyeCol.radius = 0.5f;
        bullseye.tag = "Bullseye";

        // Outer ring collider (trigger) for "miss" detection
        var outerCol = ring5.AddComponent<CircleCollider2D>();
        outerCol.isTrigger = true;
        outerCol.radius = 0.5f;
        ring5.tag = "Target";

        Debug.Log("Diana setup complete!");
    }
}
