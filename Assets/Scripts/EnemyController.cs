using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1;
    public float offset = 0.64f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;

        if (posX > ScreenBounds.horizontal + offset)
            transform.position = new Vector2(-ScreenBounds.horizontal - offset, posY);
        else if (posX < -ScreenBounds.horizontal - offset)
            transform.position = new Vector2(ScreenBounds.horizontal + offset, posY);
    }

    public void RaiseSpeed(float multiplier)
    {
        speed *= multiplier;
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }
}
