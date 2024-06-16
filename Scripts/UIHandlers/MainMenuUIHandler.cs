using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour {
    [Header("Values")]
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float hoverTime = .2f;
    [SerializeField] private float hoverScale = 1.1f;

    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image playButtonImg;
    [SerializeField] private Image quitButtonImg;

    private SoundManager soundManager;

    private void Start() {
        var service = ServiceManager.Instance;
        soundManager = service.GetManager<SoundManager>();
    }

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