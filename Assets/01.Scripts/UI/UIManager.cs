using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, UI_Base> UIs = new();

    private int _popupOrder = 100;
    
    private List<UI_Popup> _popupList = new List<UI_Popup>();
    private UI_Scene _sceneUI = null;

    public UI_Scene SceneUI
    {
        set { _sceneUI = value; }
        get { return _sceneUI; }
    }


    public void Init()
    {
        CacheUIs();
        ShowSceneUI<UI_Title>();
    }


    void CacheUIs()
    {
        UIs.Clear();

        UI_Base[] list = GetComponentsInChildren<UI_Base>(true);

        foreach (UI_Base ui in list)
        {
            ui.Init();
            
            string key = ui.gameObject.name;

            if (UIs.ContainsKey(key))
            {
                Debug.LogWarning($"Duplicate popup detected: : {key}");
                continue;
            }

            UIs.Add(key, ui);
        }
    }

    
    public Canvas SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
    {
        Canvas canvas = ComponentHelper.GetOrAddComponent<Canvas>(go);
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
        }

        CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
        if (cs != null)
        {
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(2640, 1080);
        }

        go.GetOrAddComponent<GraphicRaycaster>();

        if (sort)
        {
            canvas.sortingOrder = _popupOrder;
            _popupOrder++;
        }

        return canvas;
    }


    public T GetSceneUI<T>() where T : UI_Base
    {
        return _sceneUI as T;
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate($"{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return ComponentHelper.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = false)
        where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate($"{name}", parent, pooling);
        go.transform.SetParent(parent);
        return ComponentHelper.GetOrAddComponent<T>(go);
    }
    
    public T ShowSceneUI<T>() where T : UI_Scene
    {
        String key = typeof(T).Name;

        if (UIs.TryGetValue(key, out UI_Base ui) == false)
        {
            Debug.LogError($"Popup not registered: {key}");
        }

        T sceneUI = ui as T;

        sceneUI.gameObject.SetActive(true);

        SceneUI = sceneUI;

        return sceneUI;
    }
    
    public void ShowPopupUI<T>(Action<T> callback = null, Transform parent = null) where T : UI_Popup
    {
        String key = typeof(T).Name;

        if (UIs.TryGetValue(key, out UI_Base ui) == false)
        {
            Debug.LogError($"Popup not registered: {key}");
        }

        T popup = ui as T;

        _popupList.Add(popup);

        popup.gameObject.SetActive(true);
        _popupOrder++;

        callback?.Invoke(popup);

        if (parent != null)
            popup.transform.SetParent(parent);
    }


    public T ShowPopupUI<T>() where T : UI_Popup
    {
        String key = typeof(T).Name;

        if (UIs.TryGetValue(key, out UI_Base ui) == false)
        {
            Debug.LogError($"Popup not registered: {key}");
        }

        T popup = ui as T;

        _popupList.Add(popup);

        popup.gameObject.SetActive(true);
        _popupOrder++;

        return popup;
    }

    
    public T GetLastPopupUI<T>() where T : UI_Popup
    {
        if (_popupList.Count == 0)
            return null;

        return _popupList.Last() as T;
    }
    
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupList.Count == 0)
            return;

        _popupList.Remove(popup);
        popup.gameObject.SetActive(false);
        _popupOrder--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupList.Count > 0)
            CloseTopPopupUI();
    }

    public void CloseTopPopupUI()
    {
        if (_popupList.Count == 0)
            return;

        UI_Popup popup = _popupList[_popupList.Count - 1];
        ClosePopupUI(popup);
    }

    public int GetPopupCount()
    {
        return _popupList.Count;
    }
}