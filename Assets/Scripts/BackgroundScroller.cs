using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float tileSize = 10f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        transform.position += Vector3.down * scrollSpeed * Time.deltaTime;

        if (transform.position.y <= startY - tileSize)
        {
            transform.position = new Vector3(
                transform.position.x,
                startY,
                transform.position.z);
        }
    }
}