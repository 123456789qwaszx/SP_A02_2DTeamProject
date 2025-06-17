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
        // DontDestroyOnLoad 적용으로 씬 전환 시에도 삭제되지 않음
        CameraController[] uis = FindObjectsOfType<CameraController>();

        if (uis.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // 씬 전환 시 자동으로 (0,0,-10) 위치로 재배치
        transform.position = new Vector3(0f, 0f, -10f);

        if (Target == null)
            Target = GetPlayerFromLayer();

        if (Target == null)
            Debug.LogWarning("Player not found in the Player layer!");
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            Target = GetPlayerFromLayer();
            if (Target == null)
            {
                Debug.LogWarning("Player not found in the Player layer!");
                return;
            }
        }
        // 플레이어 위치 기준의 목표 카메라 위치
        Vector3 targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

    /// <summary>
    /// 현재 활성화된 오브젝트 중 레이어가 "Player"인 게임오브젝트를 찾아서 반환
    /// </summary>
    /// <returns>Player 레이어에 속한 게임오브젝트, 없으면 null</returns>
    private GameObject GetPlayerFromLayer()
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        // 모든 활성화된 게임오브젝트를 검색
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.activeInHierarchy && obj.layer == playerLayer)
            {
                return obj;
            }
        }
        return null;
    }
}