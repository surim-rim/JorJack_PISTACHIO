using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BoxTriggerLoader : MonoBehaviour
{
    public string nextScene = "Ocean";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
