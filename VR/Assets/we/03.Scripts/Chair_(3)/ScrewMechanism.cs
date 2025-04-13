using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewMechanism : MonoBehaviour
{
    public Transform targetPosition;
    private bool isScrewed = false;

    void Update()
    {
        if (isScrewed) return;
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.05f)
        {
            transform.position = targetPosition.position;
            transform.rotation = targetPosition.rotation;
            isScrewed = true;
        }
    }
}
