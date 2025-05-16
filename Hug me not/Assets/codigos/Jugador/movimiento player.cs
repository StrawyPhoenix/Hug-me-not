using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento

    void Update()
    {
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveZ = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveZ = 1f;
        }

        transform.Translate(0f, 0f, moveZ * speed * Time.deltaTime);
    }
}
