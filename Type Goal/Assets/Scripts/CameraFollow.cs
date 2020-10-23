using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    private Vector3 deltaPos;

    private void Start()
    {
        deltaPos = transform.position - player.position;
    }

    private void LateUpdate()
    {
        transform.position = player.position + deltaPos;
    }
}
