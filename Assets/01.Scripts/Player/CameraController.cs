using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Vector2 minCameraBoundary;
    [SerializeField]
    Vector2 maxCameraBoundary;
    [SerializeField]
    float smoothing = 0.2f;

    public GameObject Target;

    void Start()
    {
    }

    void LateUpdate()
    {
        if (Target == null)
            return;

        if(Target == null)
        Target = GameObject.Find("Player");
        Vector3 targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);

        //transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, -10);
    }
}
