using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float speed = 0.0f;
    public Transform playerTransform;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = playerTransform.transform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, speed);
        transform.position = smoothPosition;

        transform.LookAt(playerTransform.transform);
    }
}
