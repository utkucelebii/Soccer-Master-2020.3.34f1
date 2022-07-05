using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    public Transform target;

    private void FixedUpdate()
    {
        Vector3 camPos = player.transform.position;
        if(player.position.y > 6)
            camPos.y = player.position.y + 12.5f;
        else
            camPos.y = 18.5f;
        camPos.z += -20f;
        transform.position = Vector3.Lerp(transform.position, camPos, 0.125f);
        transform.LookAt(target);
        transform.rotation = Quaternion.Euler(30, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
