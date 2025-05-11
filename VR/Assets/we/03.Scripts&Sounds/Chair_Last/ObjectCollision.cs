using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 'cube_chair'인 경우
        if (collision.gameObject.CompareTag("cube_chair"))
        {
            // 'NASA' 태그를 가진 오브젝트 삭제
            if (gameObject.CompareTag("NASA"))
            {
                Destroy(gameObject);
            }
        }
    }
}
