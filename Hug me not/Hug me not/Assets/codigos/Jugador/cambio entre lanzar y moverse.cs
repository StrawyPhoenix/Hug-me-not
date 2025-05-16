using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerMovement speed;
    public float moveSpeed = 5f;
    public Rigidbody npcRb;
    public float maxLaunchForce = 15f;
    public float reconnectionDistance = 1.5f;

    private enum PlayerMode { Move, Throw }
    private PlayerMode currentMode = PlayerMode.Move;

    private Rigidbody rb;
    private Renderer playerRenderer;
    private LineRenderer lineRenderer;
    private Camera mainCamera;

    private bool isDragging = false;
    private bool npcLaunched = false;
    private Vector3 dragStartPos;

    void Start()
    {


        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        if (npcRb == null)
        {
            Debug.LogError("¡npcRb no está asignado!");
        }

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }

        UpdatePlayerColor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMode();
        }

        if (currentMode == PlayerMode.Move)
        {
            HandleMovement();
        }
        else if (currentMode == PlayerMode.Throw)
        {
            HoldNPC();

            if (Input.GetMouseButtonDown(0))
            {
                dragStartPos = GetMouseWorldPosition();
                isDragging = true;

                if (lineRenderer != null)
                    lineRenderer.enabled = true;
            }

            if (isDragging)
            {
                UpdateLineRenderer();

                if (Input.GetMouseButtonUp(0))
                {
                   

                    Vector3 dragEndPos = GetMouseWorldPosition();
                    Vector3 launchDir = (dragStartPos - dragEndPos).normalized;

                    //  Restringir a plano Y-Z
                    launchDir = new Vector3(0f, launchDir.y, launchDir.z).normalized;

                    float dragDistance = Vector3.Distance(dragStartPos, dragEndPos);
                    float launchForce = Mathf.Clamp(dragDistance * 10f, 0f, maxLaunchForce);

                    npcRb.isKinematic = false;
                    npcRb.transform.parent = null;
                    npcRb.AddForce(launchDir * launchForce, ForceMode.Impulse);
                    isDragging = false;
                    npcLaunched = true;
                    print("bebe bolo");       
                    if (lineRenderer != null)
                        lineRenderer.enabled = false;

                    Debug.Log("NPC lanzado con fuerza: " + launchForce);
                    
                }
            }

            TryReconnectNPC();
        }
    }

    void HandleMovement()
    {
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.A)) moveZ = -1f;
        else if (Input.GetKey(KeyCode.D)) moveZ = 1f;

        Vector3 move = new Vector3(0f, 0f, moveZ) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + move);
    }

    void ToggleMode()
    {
        
        currentMode = (currentMode == PlayerMode.Move) ? PlayerMode.Throw : PlayerMode.Move;
        UpdatePlayerColor();

        if (currentMode == PlayerMode.Throw)
        {
            print("velocidad:" + moveSpeed);
            moveSpeed = 0f;
            speed.speed = moveSpeed;
         
        }
        if (currentMode == PlayerMode.Move)
        {
            moveSpeed = 5f;
            speed.speed = moveSpeed;
          

        }
    }

    void UpdatePlayerColor()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = (currentMode == PlayerMode.Move) ? Color.green : Color.red;
            
        }
    }

    void HoldNPC()
    {
        if (npcRb != null && !isDragging && !npcLaunched)
        {
            npcRb.isKinematic = true;
            print("agarro");
            // Mantener la X del jugador
            Vector3 holdPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.92f);
            npcRb.transform.position = holdPos;
            npcRb.transform.parent = transform;
        }
    }

    void TryReconnectNPC()
    {
        if (npcLaunched && currentMode == PlayerMode.Throw)
        {
        
            if(Input.GetKeyDown(KeyCode.F))
            {

            float distance = Vector3.Distance(transform.position, npcRb.position);
            if (distance <= reconnectionDistance)
            {
                npcLaunched = false;
                npcRb.velocity = Vector3.zero;
                npcRb.angularVelocity = Vector3.zero;
                npcRb.isKinematic = true;

                // Mantener la X del jugador
                Vector3 reconnectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0f);
                npcRb.transform.position = reconnectPos;
                npcRb.transform.parent = transform;

                Debug.Log("NPC reconectado al jugador");
            }
            }
        }
    }

    void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, GetMouseWorldPosition());
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Fijar el plano en la posición X del jugador (plano Y-Z)
        Plane plane = new Plane(Vector3.right, new Vector3(transform.position.x, 0f, 0f));

        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return transform.position;
    }
}