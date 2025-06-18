using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Toggle backgroundMusicToggle;
    [SerializeField] private Slider effectSoundSlider;
    [SerializeField] private Toggle effectSoundToggle;

    private void Start()
    {
        // 초기값 설정
        backgroundMusicSlider.value = SoundManager.Instance.BGMVolume;
        effectSoundSlider.value = SoundManager.Instance.SFXVolume;

        backgroundMusicToggle.isOn = !SoundManager.Instance.IsBGMMuted;
        effectSoundToggle.isOn = !SoundManager.Instance.IsSFXMuted;

        // UI 이벤트 리스너 추가
        backgroundMusicSlider.onValueChanged.AddListener(SetBackgroundVolume);
        effectSoundSlider.onValueChanged.AddListener(SetEffectVolume);
        backgroundMusicToggle.onValueChanged.AddListener(SetBackgroundMute);
        effectSoundToggle.onValueChanged.AddListener(SetEffectMute);
    }

    private void SetBackgroundVolume(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
    }

    private void SetEffectVolume(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
    }

    private void SetBackgroundMute(bool isMuted)
    {
        SoundManager.Instance.SetBGMMute(!isMuted);
    }

    private void SetEffectMute(bool isMuted)
    {
        SoundManager.Instance.SetSFXMute(!isMuted);
    }
}