using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public float rotateSpeed=15;
    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime, Space.World);
    }
}
