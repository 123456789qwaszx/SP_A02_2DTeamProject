using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    private void OnMouseDown()
    {
        ShopUIManager.Instance.OpenShopUI();
    }
}
