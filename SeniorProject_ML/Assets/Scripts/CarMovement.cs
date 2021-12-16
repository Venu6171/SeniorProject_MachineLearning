using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> wheelTransform;
    [SerializeField] private int rpm = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < wheelTransform.Count; ++i)
            wheelTransform[i] = wheelTransform[i].GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        foreach (var wheels in wheelTransform)
            wheels.Rotate(rpm / 60.0f * 360.0f * Time.fixedDeltaTime, 0.0f, 0.0f);
    }
}
