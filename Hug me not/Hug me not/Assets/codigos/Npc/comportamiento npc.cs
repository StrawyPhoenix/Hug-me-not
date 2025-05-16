using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comportamientonpc : MonoBehaviour
{
    public Transform player;            // Referencia al jugador
    public float maxDistance = 5f;      // Longitud máxima de la "cuerda"
    public float baseJumpForce = 5f;    // Fuerza inicial del salto
    public float baseMoveSpeed = 2f;    // Velocidad de movimiento lateral
    public float jumpInterval = 1f;     // Tiempo entre saltos

    private Rigidbody rb;
    private bool isDisconnected = false;
    private float disconnectTime = 0f;
    private float jumpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > maxDistance)
        {
            if (!isDisconnected)
            {
                // Se acaba de romper la cuerda
                isDisconnected = true;
                disconnectTime = 0f;
                jumpTimer = 0f;
            }

            disconnectTime += Time.deltaTime;
            jumpTimer += Time.deltaTime;

            if (jumpTimer >= jumpInterval)
            {
                JumpAndMove();
                jumpTimer = 0f;
            }
        }
        else
        {
            // Está conectado
            isDisconnected = false;
            disconnectTime = 0f;
        }
    }

    void JumpAndMove()
    {
        float speedMultiplier = 1f + (disconnectTime * 0.5f); // Aumenta con el tiempo
        float jumpForce = baseJumpForce * speedMultiplier;
        float moveSpeed = baseMoveSpeed * speedMultiplier;

        // Movimiento aleatorio solo en Y y Z
        float randomY = Random.Range(1f, 1.5f) * jumpForce;
        float randomZ = Random.Range(-1f, 1f) * moveSpeed;

        Vector3 jumpVector = new Vector3(0f, randomY, randomZ);
        rb.AddForce(jumpVector, ForceMode.Impulse);
    }
}
        
    

