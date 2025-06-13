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

        // 카메라 뷰포트의 절반 크기를 고려한 경계 클램핑
        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        float minX = minCameraBoundary.x + cameraHalfWidth;
        float maxX = maxCameraBoundary.x - cameraHalfWidth;
        float minY = minCameraBoundary.y + cameraHalfHeight;
        float maxY = maxCameraBoundary.y - cameraHalfHeight;

        // 카메라가 경계 밖으로 넘어가지 않도록 위치 제한
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);

        //transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, -10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = new Vector3(
            (minCameraBoundary.x + maxCameraBoundary.x) / 2f,
            (minCameraBoundary.y + maxCameraBoundary.y) / 2f,
            transform.position.z);

        Vector3 size = new Vector3(
            Mathf.Abs(maxCameraBoundary.x - minCameraBoundary.x),
            Mathf.Abs(maxCameraBoundary.y - minCameraBoundary.y),
            0f);

        Gizmos.DrawWireCube(center, size);
    }
}
