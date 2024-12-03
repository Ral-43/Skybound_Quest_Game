using UnityEngine;

public class Plataf_Movel : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveDistance = 2f;
    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x > startPos.x + moveDistance)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x < startPos.x - moveDistance)
                movingRight = true;
        }
    }
}
