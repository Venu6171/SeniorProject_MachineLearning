using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float speed = 0.0f;

    private Camera myCamera;
    private Vector3 cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        cameraTransform.y = speed * Time.deltaTime;
        myCamera.transform.position += cameraTransform;
    }
}
