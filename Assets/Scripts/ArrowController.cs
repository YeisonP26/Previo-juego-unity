using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float bullseyeRadius = 0.5f;
    public float overshootSpeed = 60f; // speed above which bullet passes through

    private Rigidbody2D rb;
    private bool hasHit = false;
    private bool insideDiana = false;
    private bool isOvershooting = false;
    private Transform dianaTransform;
    private float minDistToCenter = float.MaxValue;
    private float timeInsideDiana = 0f;
    private float enterSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        if (!hasHit && rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // While inside diana, track closest distance to center
        if (insideDiana && !hasHit && !isOvershooting && dianaTransform != null)
        {
            float dist = Vector2.Distance(transform.position, dianaTransform.position);
            if (dist < minDistToCenter)
                minDistToCenter = dist;

            timeInsideDiana += Time.deltaTime;

            // Resolve after the arrow has had time to pass through
            if (timeInsideDiana > 0.05f)
            {
                ResolveHit();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Target"))
        {
            enterSpeed = rb.linearVelocity.magnitude;
            dianaTransform = other.transform;

            // Too much force -> arrow passes right through!
            if (enterSpeed > overshootSpeed)
            {
                isOvershooting = true;
                if (GameManager.Instance != null)
                    GameManager.Instance.ShowOvershootMessage();
                return;
            }

            insideDiana = true;
            timeInsideDiana = 0f;
            minDistToCenter = Vector2.Distance(transform.position, other.transform.position);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Target"))
        {
            if (isOvershooting)
            {
                // Arrow flew through - keep going, already showed message
                isOvershooting = false;
                insideDiana = false;
                return;
            }

            if (insideDiana)
            {
                ResolveHit();
            }
        }
    }

    void ResolveHit()
    {
        if (hasHit || dianaTransform == null) return;
        hasHit = true;

        // Final distance check
        float finalDist = Vector2.Distance(transform.position, dianaTransform.position);
        if (finalDist < minDistToCenter)
            minDistToCenter = finalDist;

        if (minDistToCenter <= bullseyeRadius)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(10);
        }
        else
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ShowMissMessage();
        }

        StickArrow(dianaTransform);
    }

    private void StickArrow(Transform parent)
    {
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(parent);
        Destroy(gameObject, 2f);
    }
}
