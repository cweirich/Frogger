using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    public delegate void FrogHandler();
    public event FrogHandler OnFrogMoved;
    public event FrogHandler OnFrogEscaped;

    public float jumpDistance = 0.32f;

    private bool jumped;
    private Vector2 startPosition = new Vector2(0, -ScreenBounds.vertical);

    // Update is called once per frame
    void Update()
    {
        float hMovement = Input.GetAxis("Horizontal");
        float vMovement = Input.GetAxis("Vertical");

        if (!jumped)
        {
            if (hMovement != 0)
            {
                Vector2 targetPosition = new Vector2(transform.position.x + hMovement * jumpDistance, transform.position.y);
                Move(targetPosition);
            }

            if (vMovement != 0)
            {
                Vector2 targetPosition = new Vector2(transform.position.x, transform.position.y + vMovement * jumpDistance);
                Move(targetPosition);
            }
        }
        else
        {
            if (hMovement == 0 && vMovement == 0)
                jumped = false;
        }
        
    }

    private void Move(Vector2 targetPosition)
    {
        if (CheckBoundaries(targetPosition) && CheckObstacles(targetPosition))
        {
            if (CheckStageComplete(targetPosition))
            {
                transform.position = startPosition;
                OnFrogEscaped?.Invoke();
            }
            else
                transform.position = targetPosition;

            jumped = true;
            GetComponent<AudioSource>().Play();
            OnFrogMoved?.Invoke();
        }
    }

    private bool CheckBoundaries(Vector2 targetPosition)
    {
        float posX = targetPosition.x;
        float posY = targetPosition.y;

        if (posX < -ScreenBounds.horizontal || posX > ScreenBounds.horizontal || posY < -ScreenBounds.vertical)
            return false;

        return true;
    }

    private bool CheckObstacles(Vector2 targetPosition)
    {
        Collider2D collider;

        collider = Physics2D.OverlapCircle(targetPosition, 0.1f);

        if (collider == null || collider.isTrigger)
            return true;

        return false;
    }

    private bool CheckStageComplete(Vector2 targetPosition)
    {
        if (targetPosition.y > ScreenBounds.vertical)
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            Destroy(gameObject);
    }

}
