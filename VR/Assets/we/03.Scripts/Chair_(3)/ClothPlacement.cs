using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothPlacement : MonoBehaviour
{
    public Transform targetPosition; // 씌울 위치
    private bool isPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cloth") && !isPlaced)
        {
            other.transform.position = targetPosition.position;
            other.GetComponent<Rigidbody>().isKinematic = true; // 천 고정
            isPlaced = true;
        }
    }
}
