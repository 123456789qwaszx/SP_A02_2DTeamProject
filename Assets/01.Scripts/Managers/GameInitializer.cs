using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject saveManagerPrefab;

    void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject gm = Instantiate(gameManagerPrefab);
            DontDestroyOnLoad(gm);
        }

        if (FindObjectOfType<SaveManager>() == null)
        {
            GameObject sm = Instantiate(saveManagerPrefab);
            DontDestroyOnLoad(sm);
        }
    }
}
