using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ShootingController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Transform target;
    public float maxForce = 50f;
    public float chargeSpeed = 18f;
    public float minAngle = 5f;
    public float maxAngle = 75f;
    public Image powerBarFill;
    public TextMeshProUGUI powerPercentText;
    public Image powerBarBG;
    public TextMeshProUGUI angleText;
    public LineRenderer aimLine;

    private float currentForce = 0f;
    private bool isCharging = false;
    private Vector3 originalBarScale;
    private Color bgDefaultColor;
    private float currentAngle = 25f;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (powerBarBG != null)
        {
            originalBarScale = powerBarBG.rectTransform.localScale;
            bgDefaultColor = powerBarBG.color;
        }
        UpdatePowerDisplay(0f);

        if (aimLine == null)
        {
            aimLine = gameObject.AddComponent<LineRenderer>();
            aimLine.startWidth = 0.05f;
            aimLine.endWidth = 0.03f;
            aimLine.material = new Material(Shader.Find("Sprites/Default"));
            aimLine.startColor = new Color(1f, 1f, 1f, 0.6f);
            aimLine.endColor = new Color(1f, 1f, 1f, 0.15f);
            aimLine.sortingOrder = 20;
        }
    }

    void Update()
    {
        UpdateAimDirection();
        DrawAimLine();

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            isCharging = true;
            currentForce = 0f;
        }

        if (keyboard.spaceKey.isPressed && isCharging)
        {
            currentForce += chargeSpeed * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, 0f, maxForce);
            float pct = currentForce / maxForce;
            UpdatePowerDisplay(pct);
        }

        if (keyboard.spaceKey.wasReleasedThisFrame && isCharging)
        {
            Shoot();
            isCharging = false;
            currentForce = 0f;
            UpdatePowerDisplay(0f);
        }
    }

    void UpdateAimDirection()
    {
        if (mainCam == null || shootPoint == null) return;

        var mouse = Mouse.current;
        if (mouse == null) return;

        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouse.position.ReadValue());
        mouseWorld.z = 0f;

        Vector2 dir = (Vector2)(mouseWorld - shootPoint.position);

        if (dir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            currentAngle = Mathf.Clamp(angle, minAngle, maxAngle);
        }

        if (angleText != null)
            angleText.text = Mathf.RoundToInt(currentAngle) + "°";
    }

    void DrawAimLine()
    {
        if (aimLine == null || shootPoint == null) return;

        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        int segments = 20;
        aimLine.positionCount = segments;

        // Trajectory preview showing actual parabola
        float previewForce = isCharging ? currentForce : maxForce * 0.5f;
        float gravity = 9.81f * 0.15f; // match bullet gravity scale
        float totalTime = 0.45f; // preview ~0.45 seconds of flight

        for (int i = 0; i < segments; i++)
        {
            float t = (i / (float)(segments - 1)) * totalTime;
            float x = shootPoint.position.x + dir.x * previewForce * t;
            float y = shootPoint.position.y + dir.y * previewForce * t - 0.5f * gravity * t * t;
            aimLine.SetPosition(i, new Vector3(x, y, 0));
        }

        // Color the line based on charging state
        if (isCharging)
        {
            float pct = currentForce / maxForce;
            Color lineColor;
            if (pct < 0.5f)
                lineColor = Color.Lerp(new Color(1f, 1f, 1f, 0.5f), new Color(1f, 1f, 0.2f, 0.7f), pct / 0.5f);
            else
                lineColor = Color.Lerp(new Color(1f, 1f, 0.2f, 0.7f), new Color(1f, 0.3f, 0.1f, 0.8f), (pct - 0.5f) / 0.5f);
            aimLine.startColor = lineColor;
            aimLine.endColor = new Color(lineColor.r, lineColor.g, lineColor.b, 0.1f);
        }
        else
        {
            aimLine.startColor = new Color(1f, 1f, 1f, 0.4f);
            aimLine.endColor = new Color(1f, 1f, 1f, 0.1f);
        }
    }

    void UpdatePowerDisplay(float pct)
    {
        if (powerBarFill != null)
        {
            powerBarFill.fillAmount = pct;

            Color barColor;
            if (pct < 0.4f)
                barColor = Color.Lerp(new Color(0.1f, 0.85f, 0.2f), new Color(1f, 0.9f, 0.1f), pct / 0.4f);
            else if (pct < 0.7f)
                barColor = Color.Lerp(new Color(1f, 0.9f, 0.1f), new Color(1f, 0.5f, 0f), (pct - 0.4f) / 0.3f);
            else
                barColor = Color.Lerp(new Color(1f, 0.5f, 0f), new Color(0.9f, 0.1f, 0.1f), (pct - 0.7f) / 0.3f);

            powerBarFill.color = barColor;
        }

        if (powerPercentText != null)
        {
            int percent = Mathf.RoundToInt(pct * 100f);
            powerPercentText.text = percent + "%";
        }

        if (powerBarBG != null && originalBarScale.sqrMagnitude > 0)
        {
            if (pct > 0.8f)
            {
                float pulse = 1f + Mathf.Sin(Time.time * 12f) * 0.03f;
                powerBarBG.rectTransform.localScale = originalBarScale * pulse;
                powerBarBG.color = Color.Lerp(bgDefaultColor, new Color(0.4f, 0.15f, 0.15f, 0.9f), (pct - 0.8f) / 0.2f);
            }
            else
            {
                powerBarBG.rectTransform.localScale = originalBarScale;
                powerBarBG.color = bgDefaultColor;
            }
        }
    }

    void Shoot()
    {
        if (arrowPrefab == null || shootPoint == null) return;

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        rb.linearVelocity = direction * currentForce;
    }
}
