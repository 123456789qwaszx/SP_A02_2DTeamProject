using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTest : MonoBehaviour
{
    public ItemDropManager dropManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // G 키 누르면 드랍
        {
            Vector3 dropPos = transform.position;
            dropManager.TryDropItem(dropPos);
        }
    }
}
