using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float smoothing = 0.2f;

    public GameObject Target;

    void Start()
    {
        if (Target == null)
            Target = GameObject.FindWithTag("Player");
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            Target = GameObject.FindWithTag("Player");
            if (Target == null)
            {
                Debug.LogWarning("Player not found!");
                return;
            }
        }
        // 플레이어 위치 기준의 목표 카메라 위치 (Z축은 고정)
        Vector3 targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);

        //transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, -10);
    }

}
