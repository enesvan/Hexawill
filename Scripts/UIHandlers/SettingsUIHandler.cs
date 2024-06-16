using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SettingsUIHandler : MonoBehaviour {
    [Header("Values")]
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float hoverTime = .2f;
    [SerializeField] private float hoverScale = 1.1f;

    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroupSettings;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button quitButtonSettings;
    [SerializeField] private Image resumeButtonImg;
    [SerializeField] private Image resetButtonImg;
    [SerializeField] private Image quitButtonSettingsImg;

    private bool isSettingsActive = false;
    private bool isFading = false;
    private bool isActive = false;

    private UIManager uiManager;
    private SoundManager soundManager;

    private void Start() {
        var service = ServiceManager.Instance;
        uiManager = service.GetManager<UIManager>();
        soundManager = service.GetManager<SoundManager>();
        uiManager.OnLoadingFadeEnd += () => isActive = true;

        canvasGroupSettings.interactable = false;
        canvasGroupSettings.alpha = 0f;
        OpenCloseButtonsSettings(false);
    }

    private void Update() {
        if (!isActive) return;
        if (isFading) return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isSettingsActive) {
                uiManager.OnSettingsOpen?.Invoke();
                OpenSettings();
            } else {
                ResumeButtonOnClick();
            }
        }
    }

    public void OpenSettings() {
        isSettingsActive = true;
        isFading = true;
        canvasGroupSettings.DOFade(1f, fadeTime).OnComplete(() => {
            canvasGroupSettings.interactable = true;
            isFading = false;
        });
        OpenCloseButtonsSettings(true);
    }

    public void ResumeButtonOnClick() {
        soundManager.PlayButtonPositiveSound();
        uiManager.OnSettingsClose?.Invoke();
        isSettingsActive = false;
        isFading = true;
        canvasGroupSettings.DOFade(0f, fadeTime).OnComplete(() => {
            isFading = false;
            canvasGroupSettings.interactable = false;
        });
        OpenCloseButtonsSettings(false);
    }

    private void OpenCloseButtonsSettings(bool state) {
        resumeButton.interactable = state;
        resetButton.interactable = state;
        quitButtonSettings.interactable = state;
        resumeButtonImg.raycastTarget = state;
        resetButtonImg.raycastTarget = state;
        quitButtonSettingsImg.raycastTarget = state;
    }

    public void ResetButtonOnClick() {
        soundManager.PlayButtonNegativeSound();
        var service = ServiceManager.Instance;
        var saveManager = service.GetManager<SaveManager>();
        saveManager.Delete();

        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitButtonOnClick() {
        soundManager.PlayButtonNegativeSound();
        Application.Quit();
    }

    public void HoverButton(Transform tf) {
        tf.DOKill();
        tf.DOScale(hoverScale, hoverTime);
    }
    public void UnHoverButton(Transform tf) {
        tf.DOKill();
        tf.DOScale(1f, hoverTime);
    }
}