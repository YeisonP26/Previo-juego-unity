using UnityEngine;

public class AirplaneFly : MonoBehaviour
{
    public float speed = 3f;
    public float minX = -12f;
    public float maxX = 12f;
    public float bobAmplitude = 0.3f;
    public float bobFrequency = 0.5f;

    private float baseY;
    private float direction = 1f;

    void Start()
    {
        baseY = transform.position.y;
    }

    void Update()
    {
        // Move horizontally
        float x = transform.position.x + speed * direction * Time.deltaTime;

        // When reaching edge, wrap around
        if (x > maxX)
        {
            x = minX;
        }
        else if (x < minX)
        {
            x = maxX;
        }

        // Gentle bobbing
        float y = baseY + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
