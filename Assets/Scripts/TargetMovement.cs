using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float speed = 3f;
    public float leftLimit = -3f;
    public float rightLimit = 3f;

    private int direction = 1;

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (transform.position.x >= rightLimit)
            direction = -1;
        else if (transform.position.x <= leftLimit)
            direction = 1;
    }
}
