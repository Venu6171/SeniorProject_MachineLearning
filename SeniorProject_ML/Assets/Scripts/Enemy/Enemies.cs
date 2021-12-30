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
        Vector3 newPosition = rigidBody.position;
        if (other.gameObject.name == "Collider_L" && direction == -1)
        {
            newPosition.x = -newPosition.x;
            rigidBody.position = newPosition;
        }

        if (other.gameObject.name == "Collider_R" && direction == 1)
        {
            newPosition.x = -newPosition.x;
            rigidBody.position = newPosition;
        }
    }
}
