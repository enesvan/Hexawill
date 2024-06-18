using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuUIHandler : InGameUIHandler {
    [Header("Values")]
    [SerializeField] private float fadeTime = 1f;

    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image playButtonImg;
    [SerializeField] private Image quitButtonImg;

    public void PlayButtonOnClick() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();
        soundManager.PlayButtonPositiveSound();

        uiManager.OnPlay?.Invoke();
        canvasGroup.DOFade(0f, fadeTime).OnComplete(() => canvasGroup.interactable = false);
        playButton.interactable = false;
        quitButton.interactable = false;
        playButtonImg.raycastTarget = false;
        quitButtonImg.raycastTarget = false;
    }
}