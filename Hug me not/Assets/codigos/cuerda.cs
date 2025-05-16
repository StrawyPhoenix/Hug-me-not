using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuerda : MonoBehaviour
{
    public Transform player;
    public Transform npc;
    public float maxDistance = 5f;
    public float reconnectDistance = 3f;

    private LineRenderer line;
    private bool isConnected = true;

    void Start()
    {
        // Crear el LineRenderer
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.white;
        line.endColor = Color.white;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, npc.position);

        if (isConnected && distance > maxDistance)
        {
            // Se rompe la cuerda
            isConnected = false;
        }
        else if (!isConnected && distance < reconnectDistance)
        {
            // Se reconecta
            isConnected = true;
        }

        // Actualizar visual de la cuerda
        if (isConnected)
        {
            line.enabled = true;
            line.SetPosition(0, player.position);
            line.SetPosition(1, npc.position);
        }
        else
        {
            line.enabled = false;
        }
    }

    // Método externo para consultar el estado de conexión
    public bool IsConnected()
    {
        return isConnected;
    }
}
