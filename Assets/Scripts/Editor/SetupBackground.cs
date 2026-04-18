using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class SetupBackground
{
    public static void Execute()
    {
        // Create background
        Sprite knobSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

        // Sky background
        GameObject sky = new GameObject("Background_Sky");
        var srSky = sky.AddComponent<SpriteRenderer>();
        srSky.sprite = knobSprite;
        srSky.color = new Color(0.53f, 0.81f, 0.92f, 1f); // light blue sky
        sky.transform.position = new Vector3(0, 2, 5);
        sky.transform.localScale = new Vector3(30f, 15f, 1f);
        srSky.sortingOrder = -10;

        // Ground
        GameObject ground = new GameObject("Background_Ground");
        var srGround = ground.AddComponent<SpriteRenderer>();
        srGround.sprite = knobSprite;
        srGround.color = new Color(0.13f, 0.55f, 0.13f, 1f); // green grass
        ground.transform.position = new Vector3(0, -4, 5);
        ground.transform.localScale = new Vector3(30f, 6f, 1f);
        srGround.sortingOrder = -9;

        // Bow / Archer visual at shoot point
        GameObject bow = new GameObject("BowVisual");
        var srBow = bow.AddComponent<SpriteRenderer>();
        srBow.sprite = knobSprite;
        srBow.color = new Color(0.55f, 0.27f, 0.07f, 1f); // brown
        bow.transform.position = new Vector3(-7.3f, 0, 0);
        bow.transform.localScale = new Vector3(0.15f, 1.5f, 1f);
        bow.transform.rotation = Quaternion.Euler(0, 0, 15);
        srBow.sortingOrder = 5;

        // Aim crosshair indicator at shoot point
        GameObject cross = new GameObject("AimIndicator");
        var srCross = cross.AddComponent<SpriteRenderer>();
        srCross.sprite = knobSprite;
        srCross.color = new Color(1f, 0.3f, 0.3f, 0.5f);
        cross.transform.position = new Vector3(-7f, 0, 0);
        cross.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        srCross.sortingOrder = 6;

        // Target post (pole under diana)
        GameObject post = new GameObject("TargetPost");
        var srPost = post.AddComponent<SpriteRenderer>();
        srPost.sprite = knobSprite;
        srPost.color = new Color(0.55f, 0.27f, 0.07f, 1f);
        post.transform.position = new Vector3(3, 0, 0);
        post.transform.localScale = new Vector3(0.15f, 4f, 1f);
        srPost.sortingOrder = -1;

        // Parent post to Diana so it moves with it
        GameObject diana = GameObject.Find("Diana");
        if (diana != null)
        {
            post.transform.SetParent(diana.transform);
            post.transform.localPosition = new Vector3(0, -2f, 0);
        }

        Debug.Log("Background setup complete!");
    }
}
