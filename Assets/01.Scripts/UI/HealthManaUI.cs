using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    public Image healthFillImage;

    [SerializeField] private float smoothSpeed = 5f; // 부드럽게 전환되는 속도

    private void Update()
    {
        if (GameManager.Instance.player == null)
            return;

        float targetHpFill = Mathf.Clamp01(GameManager.Instance.player.GetHP() / GameManager.Instance.player.GetMaxHP());

        //currentHpFill = Mathf.Lerp(currentHpFill, targetHpFill, Time.deltaTime * smoothSpeed);

        healthFillImage.fillAmount = targetHpFill;
    }
}
