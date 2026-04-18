using UnityEngine;
using UnityEditor;

public class WarBackground
{
    [MenuItem("Tools/War Background")]
    public static void Execute()
    {
        var sq = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/WhiteSquare.png");
        var ci = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Circle256.png");

        // ── 1. Darken Sky → smoky war sky gradient ──
        var sky = GameObject.Find("Background_Sky");
        if (sky != null)
        {
            var sr = sky.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.35f, 0.28f, 0.22f, 1f); // smoky brownish-orange sky
        }

        // ── Sky gradient top (darker, more smoke) ──
        var skyTop = new GameObject("SkyTop");
        skyTop.transform.position = new Vector3(0f, 8f, 1.99f);
        skyTop.transform.localScale = new Vector3(25f, 6f, 1f);
        var skyTopSR = skyTop.AddComponent<SpriteRenderer>();
        skyTopSR.sprite = sq;
        skyTopSR.color = new Color(0.18f, 0.15f, 0.12f, 1f); // dark smoky
        skyTopSR.sortingOrder = -10;

        // ── War sky reddish glow (horizon) ──
        var horizonGlow = new GameObject("HorizonGlow");
        horizonGlow.transform.position = new Vector3(0f, 2.5f, 1.98f);
        horizonGlow.transform.localScale = new Vector3(25f, 3f, 1f);
        var hgSR = horizonGlow.AddComponent<SpriteRenderer>();
        hgSR.sprite = sq;
        hgSR.color = new Color(0.65f, 0.25f, 0.1f, 0.5f); // fiery orange glow
        hgSR.sortingOrder = -9;

        // ── 2. Change ground to desert sand ──
        var ground = GameObject.Find("Background_Ground");
        if (ground != null)
        {
            var sr = ground.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.76f, 0.65f, 0.42f, 1f); // desert sand
        }

        // ── Dirt → darker sand border ──
        var dirt = GameObject.Find("Background_Dirt");
        if (dirt != null)
        {
            var sr = dirt.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.55f, 0.42f, 0.25f, 1f);
        }

        // ── 3. Smoke clouds ──
        float[] smokeX = { -6f, -2f, 3f, 7f, -4f, 5f, 0f, 8f };
        float[] smokeY = { 4.5f, 5.5f, 3.8f, 5f, 6.2f, 6.5f, 4f, 3.5f };
        float[] smokeS = { 2.5f, 3f, 2f, 2.8f, 1.8f, 2.2f, 3.5f, 1.5f };
        for (int i = 0; i < smokeX.Length; i++)
        {
            var smoke = new GameObject("Smoke_" + i);
            smoke.transform.position = new Vector3(smokeX[i], smokeY[i], 1.9f);
            float s = smokeS[i];
            smoke.transform.localScale = new Vector3(s, s * 0.6f, 1f);
            var ssr = smoke.AddComponent<SpriteRenderer>();
            ssr.sprite = ci;
            float alpha = Random.Range(0.15f, 0.35f);
            ssr.color = new Color(0.3f, 0.28f, 0.25f, alpha);
            ssr.sortingOrder = -7;
        }

        // ── 4. Distant fire/explosion glows ──
        float[] fireX = { -3f, 4f, 8f, -7f };
        float[] fireY = { 1.5f, 2f, 1.2f, 1.8f };
        for (int i = 0; i < fireX.Length; i++)
        {
            var fire = new GameObject("FireGlow_" + i);
            fire.transform.position = new Vector3(fireX[i], fireY[i], 1.92f);
            fire.transform.localScale = new Vector3(1.5f, 1.2f, 1f);
            var fsr = fire.AddComponent<SpriteRenderer>();
            fsr.sprite = ci;
            fsr.color = new Color(1f, 0.5f, 0.1f, Random.Range(0.2f, 0.4f));
            fsr.sortingOrder = -8;
        }

        // ── 5. USA FLAG (left side) ──
        float flagUSAx = -7.5f;
        float flagUSAy = 4.5f;
        float flagZ = 1.85f;

        // Flag pole
        var usaPole = new GameObject("USA_Pole");
        usaPole.transform.position = new Vector3(flagUSAx - 1.2f, 2.8f, flagZ);
        usaPole.transform.localScale = new Vector3(0.08f, 5f, 1f);
        var upSR = usaPole.AddComponent<SpriteRenderer>();
        upSR.sprite = sq;
        upSR.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        upSR.sortingOrder = -5;

        // USA flag background (stripes area)
        var usaFlag = new GameObject("USA_Flag");
        usaFlag.transform.position = new Vector3(flagUSAx, flagUSAy, flagZ);
        usaFlag.transform.localScale = new Vector3(2.2f, 1.4f, 1f);
        var ufSR = usaFlag.AddComponent<SpriteRenderer>();
        ufSR.sprite = sq;
        ufSR.color = new Color(0.95f, 0.95f, 0.95f, 1f); // white base
        ufSR.sortingOrder = -4;

        // Red stripes (7 red stripes)
        float stripeH = 1.4f / 13f;
        for (int i = 0; i < 13; i++)
        {
            if (i % 2 == 0) // red stripes on even indices
            {
                var stripe = new GameObject("USA_Stripe_" + i);
                float yOff = (6f - i) * stripeH;
                stripe.transform.position = new Vector3(flagUSAx, flagUSAy + yOff, flagZ - 0.01f);
                stripe.transform.localScale = new Vector3(2.2f, stripeH, 1f);
                var stSR = stripe.AddComponent<SpriteRenderer>();
                stSR.sprite = sq;
                stSR.color = new Color(0.7f, 0.1f, 0.15f, 1f);
                stSR.sortingOrder = -3;
            }
        }

        // Blue canton
        var canton = new GameObject("USA_Canton");
        canton.transform.position = new Vector3(flagUSAx - 0.55f, flagUSAy + 0.32f, flagZ - 0.02f);
        canton.transform.localScale = new Vector3(0.9f, 0.72f, 1f);
        var cSR = canton.AddComponent<SpriteRenderer>();
        cSR.sprite = sq;
        cSR.color = new Color(0.05f, 0.15f, 0.4f, 1f);
        cSR.sortingOrder = -2;

        // Stars (small white circles in canton)
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                var star = new GameObject("USA_Star");
                float sx = flagUSAx - 0.55f - 0.35f + col * 0.22f;
                float sy = flagUSAy + 0.32f + 0.25f - row * 0.17f;
                star.transform.position = new Vector3(sx, sy, flagZ - 0.03f);
                star.transform.localScale = new Vector3(0.06f, 0.06f, 1f);
                var staSR = star.AddComponent<SpriteRenderer>();
                staSR.sprite = ci;
                staSR.color = Color.white;
                staSR.sortingOrder = -1;
            }
        }

        // ── 6. IRAN FLAG (right side, near diana) ──
        float flagIRx = 7f;
        float flagIRy = 4.5f;

        // Flag pole
        var irPole = new GameObject("IRAN_Pole");
        irPole.transform.position = new Vector3(flagIRx + 1.2f, 2.8f, flagZ);
        irPole.transform.localScale = new Vector3(0.08f, 5f, 1f);
        var ipSR = irPole.AddComponent<SpriteRenderer>();
        ipSR.sprite = sq;
        ipSR.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        ipSR.sortingOrder = -5;

        // Green stripe (top third)
        var irGreen = new GameObject("IRAN_Green");
        irGreen.transform.position = new Vector3(flagIRx, flagIRy + 0.47f, flagZ);
        irGreen.transform.localScale = new Vector3(2.2f, 0.47f, 1f);
        var igSR = irGreen.AddComponent<SpriteRenderer>();
        igSR.sprite = sq;
        igSR.color = new Color(0.14f, 0.55f, 0.2f, 1f);
        igSR.sortingOrder = -4;

        // White stripe (middle)
        var irWhite = new GameObject("IRAN_White");
        irWhite.transform.position = new Vector3(flagIRx, flagIRy, flagZ);
        irWhite.transform.localScale = new Vector3(2.2f, 0.47f, 1f);
        var iwSR = irWhite.AddComponent<SpriteRenderer>();
        iwSR.sprite = sq;
        iwSR.color = new Color(0.95f, 0.95f, 0.95f, 1f);
        iwSR.sortingOrder = -4;

        // Red stripe (bottom)
        var irRed = new GameObject("IRAN_Red");
        irRed.transform.position = new Vector3(flagIRx, flagIRy - 0.47f, flagZ);
        irRed.transform.localScale = new Vector3(2.2f, 0.47f, 1f);
        var irRSR = irRed.AddComponent<SpriteRenderer>();
        irRSR.sprite = sq;
        irRSR.color = new Color(0.85f, 0.1f, 0.12f, 1f);
        irRSR.sortingOrder = -4;

        // Iran emblem (red circle/symbol in center)
        var emblem = new GameObject("IRAN_Emblem");
        emblem.transform.position = new Vector3(flagIRx, flagIRy, flagZ - 0.01f);
        emblem.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        var emSR = emblem.AddComponent<SpriteRenderer>();
        emSR.sprite = ci;
        emSR.color = new Color(0.85f, 0.12f, 0.15f, 1f);
        emSR.sortingOrder = -3;

        // Green/red border lines between stripes
        var irBorderTop = new GameObject("IRAN_BorderTop");
        irBorderTop.transform.position = new Vector3(flagIRx, flagIRy + 0.235f, flagZ - 0.01f);
        irBorderTop.transform.localScale = new Vector3(2.2f, 0.03f, 1f);
        var ibtSR = irBorderTop.AddComponent<SpriteRenderer>();
        ibtSR.sprite = sq;
        ibtSR.color = new Color(0.14f, 0.55f, 0.2f, 0.7f);
        ibtSR.sortingOrder = -3;

        var irBorderBot = new GameObject("IRAN_BorderBot");
        irBorderBot.transform.position = new Vector3(flagIRx, flagIRy - 0.235f, flagZ - 0.01f);
        irBorderBot.transform.localScale = new Vector3(2.2f, 0.03f, 1f);
        var ibbSR = irBorderBot.AddComponent<SpriteRenderer>();
        ibbSR.sprite = sq;
        ibbSR.color = new Color(0.85f, 0.1f, 0.12f, 0.7f);
        ibbSR.sortingOrder = -3;

        // ── 7. Sandbags near shooter ──
        for (int i = 0; i < 3; i++)
        {
            var bag = new GameObject("Sandbag_" + i);
            bag.transform.position = new Vector3(-5.5f + i * 0.45f, -0.5f, 0.5f);
            bag.transform.localScale = new Vector3(0.5f, 0.25f, 1f);
            bag.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
            var bSR = bag.AddComponent<SpriteRenderer>();
            bSR.sprite = ci;
            bSR.color = new Color(0.65f, 0.55f, 0.35f, 1f);
            bSR.sortingOrder = 2;
        }
        // Stack on top
        var bagTop = new GameObject("Sandbag_top");
        bagTop.transform.position = new Vector3(-5.25f, -0.25f, 0.5f);
        bagTop.transform.localScale = new Vector3(0.5f, 0.25f, 1f);
        var btSR2 = bagTop.AddComponent<SpriteRenderer>();
        btSR2.sprite = ci;
        btSR2.color = new Color(0.6f, 0.5f, 0.3f, 1f);
        btSR2.sortingOrder = 2;

        // ── 8. Barbed wire along ground ──
        for (int i = 0; i < 8; i++)
        {
            var wire = new GameObject("BarbedWire_" + i);
            wire.transform.position = new Vector3(-8f + i * 2.5f, -0.85f, 1.0f);
            wire.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
            wire.transform.localRotation = Quaternion.Euler(0, 0, 45f);
            var wSR = wire.AddComponent<SpriteRenderer>();
            wSR.sprite = sq;
            wSR.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
            wSR.sortingOrder = 1;
        }

        // ── 9. Craters on ground ──
        float[] craterX = { -3f, 2f, 6f, -1f };
        for (int i = 0; i < craterX.Length; i++)
        {
            var crater = new GameObject("Crater_" + i);
            crater.transform.position = new Vector3(craterX[i], -1.3f, 1.5f);
            float cs = Random.Range(0.6f, 1.2f);
            crater.transform.localScale = new Vector3(cs, cs * 0.3f, 1f);
            var crSR = crater.AddComponent<SpriteRenderer>();
            crSR.sprite = ci;
            crSR.color = new Color(0.35f, 0.25f, 0.15f, 0.7f);
            crSR.sortingOrder = -6;
        }

        // ── 10. Distant buildings/ruins silhouette on horizon ──
        float[] bldX = { -9f, -5f, -2f, 1f, 4f, 9f };
        float[] bldH = { 1.8f, 2.5f, 1.2f, 2f, 1.5f, 2.2f };
        float[] bldW = { 1.2f, 0.8f, 1.5f, 1f, 1.3f, 0.9f };
        for (int i = 0; i < bldX.Length; i++)
        {
            var bld = new GameObject("Ruin_" + i);
            bld.transform.position = new Vector3(bldX[i], -0.1f + bldH[i] * 0.5f, 1.95f);
            bld.transform.localScale = new Vector3(bldW[i], bldH[i], 1f);
            var bsr = bld.AddComponent<SpriteRenderer>();
            bsr.sprite = sq;
            bsr.color = new Color(0.15f, 0.12f, 0.1f, 0.5f);
            bsr.sortingOrder = -8;

            // Damage holes
            if (Random.value > 0.4f)
            {
                var hole = new GameObject("Hole_" + i);
                hole.transform.position = new Vector3(bldX[i] + Random.Range(-0.2f, 0.2f),
                    -0.1f + bldH[i] * 0.4f, 1.94f);
                hole.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                var hsr = hole.AddComponent<SpriteRenderer>();
                hsr.sprite = ci;
                hsr.color = new Color(0.08f, 0.06f, 0.05f, 0.6f);
                hsr.sortingOrder = -7;
            }
        }

        // ── 11. FLYING AIRPLANE ──
        var airplane = new GameObject("Airplane");
        airplane.transform.position = new Vector3(-8f, 5.5f, 1.0f);

        // Fuselage
        var fuselage = new GameObject("Fuselage");
        fuselage.transform.SetParent(airplane.transform);
        fuselage.transform.localPosition = Vector3.zero;
        fuselage.transform.localScale = new Vector3(2f, 0.35f, 1f);
        var fusSR = fuselage.AddComponent<SpriteRenderer>();
        fusSR.sprite = sq;
        fusSR.color = new Color(0.35f, 0.4f, 0.35f, 1f); // military green
        fusSR.sortingOrder = -5;

        // Nose
        var nose = new GameObject("Nose");
        nose.transform.SetParent(airplane.transform);
        nose.transform.localPosition = new Vector3(1.1f, 0f, 0f);
        nose.transform.localScale = new Vector3(0.35f, 0.3f, 1f);
        var nosSR = nose.AddComponent<SpriteRenderer>();
        nosSR.sprite = ci;
        nosSR.color = new Color(0.3f, 0.35f, 0.3f, 1f);
        nosSR.sortingOrder = -5;

        // Main wing
        var wing = new GameObject("Wing");
        wing.transform.SetParent(airplane.transform);
        wing.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
        wing.transform.localScale = new Vector3(0.8f, 1.5f, 1f);
        var wingSR = wing.AddComponent<SpriteRenderer>();
        wingSR.sprite = sq;
        wingSR.color = new Color(0.3f, 0.35f, 0.3f, 1f);
        wingSR.sortingOrder = -6;

        // Tail fin
        var tail = new GameObject("TailFin");
        tail.transform.SetParent(airplane.transform);
        tail.transform.localPosition = new Vector3(-1.1f, 0.2f, 0f);
        tail.transform.localScale = new Vector3(0.3f, 0.6f, 1f);
        var tailSR = tail.AddComponent<SpriteRenderer>();
        tailSR.sprite = sq;
        tailSR.color = new Color(0.3f, 0.35f, 0.3f, 1f);
        tailSR.sortingOrder = -5;

        // Tail horizontal
        var tailH = new GameObject("TailHoriz");
        tailH.transform.SetParent(airplane.transform);
        tailH.transform.localPosition = new Vector3(-1.05f, 0f, 0f);
        tailH.transform.localScale = new Vector3(0.4f, 0.7f, 1f);
        var thSR = tailH.AddComponent<SpriteRenderer>();
        thSR.sprite = sq;
        thSR.color = new Color(0.28f, 0.33f, 0.28f, 1f);
        thSR.sortingOrder = -6;

        // Cockpit window
        var cockpit = new GameObject("Cockpit");
        cockpit.transform.SetParent(airplane.transform);
        cockpit.transform.localPosition = new Vector3(0.7f, 0.08f, 0f);
        cockpit.transform.localScale = new Vector3(0.25f, 0.12f, 1f);
        var cpSR = cockpit.AddComponent<SpriteRenderer>();
        cpSR.sprite = ci;
        cpSR.color = new Color(0.5f, 0.7f, 0.9f, 0.8f);
        cpSR.sortingOrder = -4;

        // Engine exhaust trail
        var trail = new GameObject("Exhaust");
        trail.transform.SetParent(airplane.transform);
        trail.transform.localPosition = new Vector3(-2f, 0f, 0f);
        trail.transform.localScale = new Vector3(1.5f, 0.2f, 1f);
        var trSR = trail.AddComponent<SpriteRenderer>();
        trSR.sprite = ci;
        trSR.color = new Color(0.7f, 0.7f, 0.7f, 0.3f);
        trSR.sortingOrder = -7;

        // US star on wing
        var usaStar = new GameObject("WingStar");
        usaStar.transform.SetParent(airplane.transform);
        usaStar.transform.localPosition = new Vector3(-0.1f, 0.35f, 0f);
        usaStar.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
        var usSR = usaStar.AddComponent<SpriteRenderer>();
        usSR.sprite = ci;
        usSR.color = new Color(0.9f, 0.9f, 0.9f, 0.8f);
        usSR.sortingOrder = -4;

        // Add flying script
        airplane.AddComponent<AirplaneFly>();

        // ── 12. "VS" Text (optional large text between flags — using a visual substitute) ──
        // Since we can't easily add TMP in editor script, we use sprite-based "VS"

        Debug.Log("✓ War background complete: USA vs IRAN!");

        EditorUtility.SetDirty(airplane);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
}
