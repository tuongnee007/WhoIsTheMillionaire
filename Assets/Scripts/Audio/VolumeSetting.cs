using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class VolumeSetting : MonoBehaviour
{
    public static VolumeSetting Instance;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider backGroundSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider buttonSlider;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("HasSetVolume", 0) == 1) LoadVolume();
        else
        {
            FirstTimeSetup();
        }
    }
    private void FirstTimeSetup()
    {
        PlayerPrefs.SetInt("HasSetVolume", 1);
        PlayerPrefs.SetFloat("MusicVolume", backGroundSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("ButtonVolume", buttonSlider.value);
        ApplyAllVolumes();
    }
    public void SetBackGroundVolume()
    {
        float volume = Mathf.Max(backGroundSlider.value, 0.0001f);
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = Mathf.Max(sfxSlider.value, 0.0001f);
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetButtonVolume()
    {
        float volume = Mathf.Max(buttonSlider.value, 0.0001f);
        myMixer.SetFloat("Button", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ButtonVolume", volume);
    }
    private void ApplyAllVolumes()
    {
        SetBackGroundVolume();
        SetSFXVolume();
        SetButtonVolume();
    }
    private void LoadVolume()
    {
        backGroundSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        buttonSlider.value = PlayerPrefs.GetFloat("ButtonVolume");
        ApplyAllVolumes();
    }
}
