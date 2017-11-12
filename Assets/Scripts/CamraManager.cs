using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamraManager : MonoBehaviour {
    private float offset;
    private float YPos;

    public Transform player;
    Camera mainCamera;
    
    void Start()
    {
        offset = transform.position.z - player.transform.position.z;
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, player.position.y, offset);
    }
}
