using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : MonoBehaviour
{
    public Image healthFillImage;

    //private float currentHpFill;

    //[SerializeField] private float smoothSpeed = 5f; // 부드럽게 전환되는 속도


    // private void Start()
    // {
    //     currentHpFill = 1f;
    // }

    private void Update()
    {
        if (GameManager.Instance.player == null)
            return;

        float targetHpFill = Mathf.Clamp01(GameManager.Instance.player.GetHP() / GameManager.Instance.player.GetMaxHP());

        //currentHpFill = Mathf.Lerp(currentHpFill, targetHpFill, Time.deltaTime * smoothSpeed);

        healthFillImage.fillAmount = targetHpFill;
    }
}
