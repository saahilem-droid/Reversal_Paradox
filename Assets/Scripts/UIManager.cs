using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject levelCompletePanel;

    [Header("Scenes")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string firstLevelScene;

    [Header("Options")]
    [SerializeField] private bool pauseWithEscape = true;

    [Header("Audio")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip buttonClick;

    private bool paused;

    private void Start()
    {
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 1;
            FadeFromBlack();
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1;

        if (fadeGroup != null)
            fadeGroup.alpha = 0;

        CloseAllPanels();
    }

    private void Update()
    {
        if (!pauseWithEscape)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    #region Panel Functions

    public void PlayButtonSound()
    {
        if (uiAudioSource == null)
            return;

        if (buttonClick == null)
            return;

        uiAudioSource.PlayOneShot(buttonClick);
    }

    public void TogglePanel(GameObject panel)
    {
        if (panel == null)
            return;

        panel.SetActive(!panel.activeSelf);
    }

    public void OpenPanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void CloseAllPanels()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    #endregion

    #region Pause

    public void PauseGame()
    {
        paused = true;

        Time.timeScale = 0;

        OpenPanel(pausePanel);
    }

    public void SetGameplayEnabled(bool enabled)
    {
        Time.timeScale = enabled ? 1 : 0;
    }

    public void ResumeGame()
    {
        paused = false;

        Time.timeScale = 1;

        ClosePanel(pausePanel);
    }

    public void TogglePause()
    {
        if (paused)
            ResumeGame();
        else
            PauseGame();
    }

    #endregion

    #region Game Over

    public void ShowGameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return Fade(1f);

        CloseAllPanels();

        OpenPanel(gameOverPanel);

        Time.timeScale = 0;
    }

    public void HideGameOver()
    {
        ClosePanel(gameOverPanel);

        Time.timeScale = 1;
    }

    #endregion

    #region Level Complete

    public void ShowLevelComplete()
{
    StartCoroutine(LevelCompleteRoutine());
}
private IEnumerator LevelCompleteRoutine()
{
    yield return Fade(1f);

    CloseAllPanels();

    OpenPanel(levelCompletePanel);

    Time.timeScale = 0;
}

    public void HideLevelComplete()
    {
        ClosePanel(levelCompletePanel);

        Time.timeScale = 1;
    }

    #endregion

    #region Scene Functions

    public void RestartLevel()
{
    Time.timeScale = 1;
    StartCoroutine(RestartRoutine());
}
private IEnumerator RestartRoutine()
{
    yield return Fade(1f);

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(MainMenuRoutine());
    }

    private IEnumerator MainMenuRoutine()
    {
        yield return Fade(1f);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        yield return Fade(1f);

        SceneManager.LoadScene(firstLevelScene);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        StartCoroutine(NextLevelRoutine());
    }

    private IEnumerator NextLevelRoutine()
    {
        yield return Fade(1f);

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quit Game");
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return Fade(1f);

        SceneManager.LoadScene(sceneName);
    }

    #endregion

    #region Fade

    public void FadeToBlack()
    {
        StartCoroutine(Fade(1f));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeGroup == null)
            yield break;

        float startAlpha = fadeGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            fadeGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                timer / fadeDuration);

            yield return null;
        }

        fadeGroup.alpha = targetAlpha;
    }

    #endregion

    #region Navigation

    public void OpenCredits()
    {
        CloseAllPanels();
        OpenPanel(creditsPanel);
    }

    public void OpenSettings()
    {
        CloseAllPanels();
        OpenPanel(settingsPanel);
    }

    public void OpenPause()
    {
        CloseAllPanels();
        OpenPanel(pausePanel);
    }

    public void OpenMainMenuPanel()
    {
        CloseAllPanels();
        OpenPanel(mainMenuPanel);
    }

    public void Back()
    {
        CloseAllPanels();

        if (SceneManager.GetActiveScene().name == mainMenuScene)
            OpenPanel(mainMenuPanel);
        else
            OpenPanel(pausePanel);
    }

    #endregion
}