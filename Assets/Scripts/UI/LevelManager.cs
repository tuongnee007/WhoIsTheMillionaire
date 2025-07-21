using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Slider progressSlider;

    private bool isLoading = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void LoadLevel(int levelIndex)
    {
        if (isLoading) return;
        isLoading = true;

        if (loaderCanvas != null) loaderCanvas.SetActive(true);
        if (progressSlider != null) progressSlider.value = 0f;

        StartCoroutine(LoadAsync(levelIndex));
    }

    private IEnumerator LoadAsync(int levelIndex)
    {
        float minLoadTime = 0.5f;
        float timer = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        operation.allowSceneActivation = false; 

        while (operation.progress < 0.9f) 
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressSlider != null) progressSlider.value = progress;

            timer += Time.deltaTime;
            yield return null;
        }

        while (timer < minLoadTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (progressSlider != null) progressSlider.value = 1f;

        operation.allowSceneActivation = true;
    }

}
