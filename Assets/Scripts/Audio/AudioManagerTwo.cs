using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManagerTwo : MonoBehaviour
{
    public static AudioManagerTwo Instance;
    [System.Serializable]
    public struct ButtonSFXEntry
    {
        public ButtonSFXType type;
        public AudioClip clip;
    }
    [SerializeField] AudioSource buttonSource;

    [SerializeField] private List<ButtonSFXEntry> buttonSFXList;
    private Dictionary<ButtonSFXType, AudioClip> buttonSFXDict;

    private float lastHoverSoundTime = -1f;
    [SerializeField] private float hoverSoundCooldown = 0.15f;

    private float lastAnyButtonSoundTime = -1f;
    [SerializeField] private float buttonSFXGlobalCooldown = 0.1f;
    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        buttonSFXDict = new Dictionary<ButtonSFXType, AudioClip>();
        foreach (var entry in buttonSFXList)
        {
            if (!buttonSFXDict.ContainsKey(entry.type))
            {
                buttonSFXDict.Add(entry.type, entry.clip);
            }
        }
    }
    public void PlayButtonSFX(ButtonSFXType type, bool ignoreCooldown = false)
    {
        float now = Time.unscaledTime;
        if (!ignoreCooldown)
        {
            if (now - lastAnyButtonSoundTime < buttonSFXGlobalCooldown)
                return;

            if (type == ButtonSFXType.Hover)
            {
                if (now - lastHoverSoundTime < hoverSoundCooldown)
                    return;
                lastHoverSoundTime = now;
            }

            lastAnyButtonSoundTime = now;
        }

        if (buttonSFXDict.TryGetValue(type, out var clip) && clip != null)
        {
            buttonSource.PlayOneShot(clip);
        }
    }
}
