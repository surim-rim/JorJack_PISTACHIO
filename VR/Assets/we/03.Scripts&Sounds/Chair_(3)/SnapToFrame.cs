using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToFrame : MonoBehaviour
{
    public Transform snapTarget; // 조립될 위치
    private bool isSnapped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FramePart") && !isSnapped)
        {
            other.transform.position = snapTarget.position;
            other.transform.rotation = snapTarget.rotation;
            other.GetComponent<Rigidbody>().isKinematic = true; // 물리 적용 해제
            isSnapped = true;
        }
    }
}
