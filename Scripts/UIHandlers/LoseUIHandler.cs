using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseUIHandler : InGameUIHandler {
    [Header("Values")]
    [SerializeField] private float fadeTime = 1f;

    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image resetButtonImg;
    [SerializeField] private Image quitButtonImg;
    [SerializeField] private TextMeshProUGUI dayText;
    protected override void OnStart() {
        base.OnStart();
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
        OpenCloseButtons(false);
    }

    public void OpenUI() {
        soundManager.PlayLoseSound();
        canvasGroup.DOFade(1f, fadeTime).OnComplete(() => {
            canvasGroup.interactable = true;
        });
        OpenCloseButtons(true);
        dayText.text = $"your village survived for {ServiceManager.Instance.GetManager<EventManager>().EventData.Day} days";
    }

    private void OpenCloseButtons(bool state) {
        resetButton.interactable = state;
        quitButton.interactable = state;
        resetButtonImg.raycastTarget = state;
        quitButtonImg.raycastTarget = state;
    }
}