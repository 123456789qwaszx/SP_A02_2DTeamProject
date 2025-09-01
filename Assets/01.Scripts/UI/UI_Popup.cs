using UnityEngine;

public class UI_Popup : UI_Base
{
    [HideInInspector] public Canvas UICanvas { get; set; }

    protected override void Awake()
    {
        base.Awake();

        UICanvas = UIManager.Instance.SetCanvas(gameObject);
    }
    
    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI(this);
    }

}
