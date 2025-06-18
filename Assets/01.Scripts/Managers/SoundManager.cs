using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Main,
    Lv1_Castle,
    Lv2_Poison,
    Lv3_Desert,
    Lv4_Gold,
    Final,
    EndingScene
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("BGM AudioSource")]
    [SerializeField] private AudioSource bgmSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip bgm_Title;
    [SerializeField] private AudioClip bgm_Main;
    [SerializeField] private AudioClip bgm_Lv1Castle;
    [SerializeField] private AudioClip bgm_Lv2Poison;
    [SerializeField] private AudioClip bgm_Lv3Desert;
    [SerializeField] private AudioClip bgm_Lv4Gold;
    [SerializeField] private AudioClip bgm_Final;
    [SerializeField] private AudioClip endingScene;

    public float BGMVolume => bgmSource.volume;
    public float SFXVolume => sfxPool[0].volume;
    public bool IsBGMMuted => bgmSource.mute;
    public bool IsSFXMuted => sfxPool[0].mute;


    [Header("SFX Pooling")]
    [SerializeField] private int sfxSourcePoolSize = 20;

    private Coroutine bgmFadeCoroutine;

    private List<AudioSource> sfxPool = new List<AudioSource>();
    private int sfxPoolIndex = 0;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // SFX 풀 생성
        for (int i = 0; i < sfxSourcePoolSize; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.loop = false;
            sfxPool.Add(newSource);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 이름 → SceneType 변환
    private SceneType GetSceneType(string sceneName)
    {
        switch (sceneName)
        {
            case "TitleScene": return SceneType.Title;
            case "MainScene": return SceneType.Main;
            case "Dun_Lv.1_CastleScene": return SceneType.Lv1_Castle;
            case "Dun_Lv.2_PoisonScene": return SceneType.Lv2_Poison;
            case "Dun_Lv.3_DesertScene": return SceneType.Lv3_Desert;
            case "Dun_Lv.4_GoldScene": return SceneType.Lv4_Gold;
            case "Dun_FinalScene": return SceneType.Final;
            case "EndingScene": return SceneType.EndingScene;

            default: return SceneType.Main;
        }
    }

    // 씬 로드 시 자동 BGM 전환
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneType sceneType = GetSceneType(scene.name);

        switch (sceneType)
        {
            case SceneType.Title:
                ChangeBGM(bgm_Title);
                break;
            case SceneType.Main:
                ChangeBGM(bgm_Main);
                break;
            case SceneType.Lv1_Castle:
                ChangeBGM(bgm_Lv1Castle);
                break;
            case SceneType.Lv2_Poison:
                ChangeBGM(bgm_Lv2Poison);
                break;
            case SceneType.Lv3_Desert:
                ChangeBGM(bgm_Lv3Desert);
                break;
            case SceneType.Lv4_Gold:
                ChangeBGM(bgm_Lv4Gold);
                break;
            case SceneType.Final:
                ChangeBGM(bgm_Final);
                break;
            case SceneType.EndingScene:
                ChangeBGM(endingScene, 1f, 0.5f);
                break;
            default:
                ChangeBGM(bgm_Main);
                break;
        }
    }

    // BGM 전환 (자동 페이드)
    public void ChangeBGM(AudioClip newClip, float fadeDuration = 1f, float targetVolume = 0.8f)
    {
        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(FadeOutInBGM(newClip, fadeDuration, targetVolume));
    }

    private IEnumerator FadeOutInBGM(AudioClip newClip, float duration, float targetVolume)
    {
        float startVolume = bgmSource.volume;
        float timer = 0f;

        // 페이드 아웃
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        // 페이드 인
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, timer / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    // BGM 강제 정지
    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip, float volume = 0.4f)
    {
        if (clip == null || sfxPool.Count == 0) return;

        AudioSource source = sfxPool[sfxPoolIndex];
        source.volume = volume;
        source.PlayOneShot(clip);

        sfxPoolIndex = (sfxPoolIndex + 1) % sfxPool.Count;
    }

    public void SetBGMVolume(float sliderValue)
    {
        float adjustedVolume = Mathf.Pow(sliderValue, 2f); // 지수 적용
        bgmSource.volume = adjustedVolume;
    }

    public void SetSFXVolume(float sliderValue)
    {
        float adjustedVolume = Mathf.Pow(sliderValue, 2f); // 지수 적용
        foreach (var source in sfxPool)
        {
            source.volume = adjustedVolume;
        }
    }

    public void SetBGMMute(bool mute)
    {
        bgmSource.mute = mute;
    }

    public void SetSFXMute(bool mute)
    {
        foreach (var source in sfxPool)
        {
            source.mute = mute;
        }
    }

}
