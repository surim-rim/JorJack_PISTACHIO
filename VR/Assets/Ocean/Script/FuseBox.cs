using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    private FireworkLauncher launcher;

    void Start()
    {
        launcher = GetComponentInParent<FireworkLauncher>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lighter"))
        {
            launcher?.Activate();
        }
    }
}
