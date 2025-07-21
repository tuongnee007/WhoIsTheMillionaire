using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Sound : MonoBehaviour
{
    public static Sound Instance;
    [SerializeField] private AudioSource[] listAudio;
    Timer timer;
    [SerializeField] private AudioMixer myMixer;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        timer = FindObjectOfType<Timer>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneBackgroundMusic(scene.buildIndex);
    }

    private void PlaySceneBackgroundMusic(int sceneIndex)
    {
        StopAllMusic();

        if (sceneIndex == 0 && listAudio.Length > 0 && listAudio[0] != null)
        {
            listAudio[0].Play();
        }
        else if (sceneIndex == 1 && listAudio.Length > 1 && listAudio[1] != null)
        {
            listAudio[1].Play();
        }
    }

    private void StopAllMusic()
    {
        foreach (var audio in listAudio)
        {
            if (audio != null)
                audio.Stop();
        }
    }

    public void SoundAnswerCorrectly()
    {
        if (listAudio.Length > 2 && listAudio[2] != null)
        {
            listAudio[2].Play();
            StartCoroutine(WaitAndReplayBaseSound(listAudio[2].clip.length));
        }
    }

    public void SoundAnswerWrong()
    {
        if (listAudio.Length > 3 && listAudio[3] != null)
        {
            listAudio[3].Play();
            StartCoroutine(WaitAndReplayBaseSound(listAudio[3].clip.length));
        }
    }

    private bool isReplaying = false;

    private IEnumerator WaitAndReplayBaseSound(float waitTime)
    {
        if (isReplaying) yield break;
        isReplaying = true;

        AudioSource bgMusic = SceneManager.GetActiveScene().buildIndex == 0 ? listAudio[0] : listAudio[1];
        bgMusic?.Pause();
        myMixer.SetFloat("Music", -80f);

        float timer = 0f;
        while ((listAudio[2]?.isPlaying == true || listAudio[3]?.isPlaying == true) && timer < 5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        myMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(savedVolume, 0.0001f)) * 20);

        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.UnPause();
        }

        isReplaying = false;
    }
}
