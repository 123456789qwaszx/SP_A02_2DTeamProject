using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Title : UI_Scene
{
    #region enum
    enum Buttons
    {
        Btn_Start,
        Btn_Exit
    }
    enum Images
    {
        Img_BG00
    }
    #endregion

    Button Btn_Start;
    Button Btn_Exit;
    Image Img_BG00;

    protected override void Awake()
    {
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));

        Btn_Start = GetButton((int)Buttons.Btn_Start);
        Btn_Exit = GetButton((int)Buttons.Btn_Exit);
        Img_BG00 = GetImage((int)Images.Img_BG00);

        BindEvent(Btn_Start.gameObject, OnClickStartGame);
        BindEvent(Btn_Exit.gameObject, OnClickQuitGame);

        Img_BG00.sprite = Resources.Load<Sprite>("UI_Title/Img_BG00");
    }


    #region Button
    public void OnClickStartGame(PointerEventData eventData)
    {
        SceneManager.LoadScene("MainScene");
        UIManager.Instance.GetSceneUI<UI_Title>().gameObject.SetActive(false);
        UIManager.Instance.ShowSceneUI<UI_GameScene>();
    }

    public void OnClickQuitGame(PointerEventData eventData)
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}