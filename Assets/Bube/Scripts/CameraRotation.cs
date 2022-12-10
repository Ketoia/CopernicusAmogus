using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float radius = 1;
    public float rotationSpeedx = 1;

    private bool isStart = true;
    private float anglesx = 0;

    private Vector3 CameraCache;
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            if (isStart)
            {
                isStart = false;
                CameraCache = Input.mousePosition;
            }
            anglesx += (Input.mousePosition - CameraCache).x * rotationSpeedx;
            CameraCache = Input.mousePosition;

            Vector3 position = new Vector3(Mathf.Sin(anglesx) * radius, 8, Mathf.Cos(anglesx) * radius);

            transform.position = position;
            transform.LookAt(new Vector3(0, -2, 0));
        }
        else isStart = true;
    }
}
