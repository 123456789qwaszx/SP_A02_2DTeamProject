using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    [SerializeField] private GameObject modelTransform;

    public float ViewAngle = 360;    //시야각
    public float ViewDistance = 15; //시야거리
    
    public LayerMask _layermask;

    void Update()
    {
        RotateModel();
        
        DrawView();
        //FinaVisibleTargets();
    }


    public void FindVisibleTargets()
    {
        // 시야범위 내에서 MonsterLayer를 가진 콜라이더 가져오기
        // 이러면 저 시야 범위를 5~10으로 정할 수도 있나? 그러면 그걸로 바닥을 판단해서 지워버리면 진짜 편할 것 같은데 나중에 해봐야겠다.
        Collider[] targets = Physics.OverlapSphere(gameObject.transform.position, ViewDistance, _layermask);

        for (int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;

            // 타겟 까지의 단위벡터
            Vector3 dirToTarget = (target.position - gameObject.transform.position).normalized;

            // 시야에 들어왔는지 체크
            if (Vector3.Dot(gameObject.transform.position, dirToTarget) > Mathf.Cos((ViewAngle / 2) * Mathf.Deg2Rad))
            {
                float distToTarget = Vector3.Distance(gameObject.transform.position, target.position);

                if (Physics.Raycast(gameObject.transform.position, dirToTarget, distToTarget, _layermask))
                {
                    Debug.DrawLine(gameObject.transform.position, target.position, Color.red);
                }
            }
        }
    }

    public void DrawView()
    {
        Vector2 leftBoundary = DirFromAngle(-ViewAngle / 2);
        Vector2 rightBoundary = DirFromAngle(ViewAngle / 2);
        Debug.DrawLine(gameObject.transform.position, (Vector2)gameObject.transform.position + leftBoundary * ViewDistance, Color.green);
        Debug.DrawLine(gameObject.transform.position, (Vector2)gameObject.transform.position + rightBoundary * ViewDistance, Color.green);
    }


    public Vector2 DirFromAngle(float angleInDegrees)
    {
        // 좌우 회전값 갱신
        angleInDegrees += Mathf.Abs(transform.eulerAngles.y)+Mathf.Abs(transform.eulerAngles.x);
        // 경계 벡터값 반환
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void RotateModel()
    {
        Vector3 moveDir = GameManager.Instance.MoveDir;
        Quaternion lookRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 50f);

    }

}
