using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    // Public
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private int direction = 0;

    // Private
    private Rigidbody rigidBody;
    private Vector3 velocity;

    // Screen Wrapping for 2D
    /*private CameraController cameraController;
    private MeshRenderer[] renderers;
    private bool isWrappingX = false;*/

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (direction)
        {
            case -1:
                MoveLeft();
                break;
            case 1:
                MoveRight();
                break;
        }
    }

    private void MoveLeft()
    {
        velocity.x = direction * speed;
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    }

    private void MoveRight()
    {
        velocity.x = direction * speed;
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Collider_L" || other.gameObject.name == "Collider_R")
            direction = -direction;
    }

    // Screen Wrapping for 2D
    /* private void ScreenWrap()
     {
         bool isVisible = CheckRenderers;

         if (isVisible)
         {
             isWrappingX = false;
             return;
         }

         if (isWrappingX)
         {
             return;
         }

         Vector3 viewPosition = cameraController.GetComponent<Camera>().WorldToViewportPoint(transform.position);
         Vector3 newPosition = rigidBody.position;
         if (viewPosition.x > 1 || viewPosition.x < 0)
         {
             newPosition.x = -newPosition.x;
             isWrappingX = true;
         }

         rigidBody.position = newPosition;
     }

     bool CheckRenderers
     {
         get
         {
             foreach (MeshRenderer renderer in renderers)
             {
                 if (renderer.isVisible)
                 {
                     return true;
                 }
             }
             return false;
         }
     }*/
}
