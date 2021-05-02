using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotationsPerMinute = .01f;
    private void Update()
    {
        transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);

    }
}
