using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseUIHandler : MonoBehaviour {
    [Header("Values")]
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float hoverTime = .2f;
    [SerializeField] private float hoverScale = 1.1f;

    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image resetButtonImg;
    [SerializeField] private Image quitButtonImg;
    [SerializeField] private TextMeshProUGUI dayText;

    private SoundManager soundManager;

    private void Start() {
        var service = ServiceManager.Instance;
        soundManager = service.GetManager<SoundManager>();

        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
        OpenCloseButtonsSettings(false);
    }

    public void OpenUI() {
        soundManager.PlayLoseSound();
        canvasGroup.DOFade(1f, fadeTime).OnComplete(() => {
            canvasGroup.interactable = true;
        });
        OpenCloseButtonsSettings(true);
        dayText.text = $"your village survived for {ServiceManager.Instance.GetManager<EventManager>().EventData.Day} days";
    }

    public void ResetButtonOnClick() {
        soundManager.PlayButtonNegativeSound();
        var service = ServiceManager.Instance;
        var saveManager = service.GetManager<SaveManager>();
        saveManager.Delete();

        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    private void OpenCloseButtonsSettings(bool state) {
        resetButton.interactable = state;
        quitButton.interactable = state;
        resetButtonImg.raycastTarget = state;
        quitButtonImg.raycastTarget = state;
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