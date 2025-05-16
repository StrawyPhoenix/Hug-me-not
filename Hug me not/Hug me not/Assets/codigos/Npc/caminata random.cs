using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDuration = 2f;
    public float waitDuration = 1f;

    private float moveTimer = 0f;
    private float waitTimer = 0f;
    private bool isMoving = false;
    private int direction = 0;

    void Update()
    {
        if (isMoving)
        {
            moveTimer -= Time.deltaTime;

            // Mover en el eje Z
            transform.Translate(Vector3.forward * direction * moveSpeed * Time.deltaTime);

            if (moveTimer <= 0f)
            {
                isMoving = false;
                waitTimer = waitDuration;
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                ChooseNewDirection();
                isMoving = true;
                moveTimer = moveDuration;
            }
        }
    }

    void ChooseNewDirection()
    {
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
        Debug.Log("Nueva dirección: " + direction);
    }
}