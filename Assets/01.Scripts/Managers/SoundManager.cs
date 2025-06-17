using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;

    // BGM 재생
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    // BGM 정지
    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null || clip == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }
}
