using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsUIHandler : InGameUIHandler {
    [Header("Values")]
    [SerializeField] private float fadeTime = 1f;

    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image resumeButtonImg;
    [SerializeField] private Image resetButtonImg;
    [SerializeField] private Image quitButtonImg;

    private bool isSettingsActive = false;
    private bool isFading = false;
    private bool isActive = false;

    protected override void OnStart() {
        base.OnStart();
        uiManager.OnLoadingFadeEnd += () => isActive = true;

        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
        OpenCloseButtons(false);
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
        canvasGroup.DOFade(1f, fadeTime).OnComplete(() => {
            canvasGroup.interactable = true;
            isFading = false;
        });
        OpenCloseButtons(true);
    }

    public void ResumeButtonOnClick() {
        soundManager.PlayButtonPositiveSound();
        uiManager.OnSettingsClose?.Invoke();
        isSettingsActive = false;
        isFading = true;
        canvasGroup.DOFade(0f, fadeTime).OnComplete(() => {
            isFading = false;
            canvasGroup.interactable = false;
        });
        OpenCloseButtons(false);
    }

    private void OpenCloseButtons(bool state) {
        resumeButton.interactable = state;
        resetButton.interactable = state;
        quitButton.interactable = state;
        resumeButtonImg.raycastTarget = state;
        resetButtonImg.raycastTarget = state;
        quitButtonImg.raycastTarget = state;
    }
}