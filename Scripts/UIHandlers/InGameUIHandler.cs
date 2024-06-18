using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIHandler : MonoBehaviour {
    [Header("In Game UI Values")]
    [SerializeField] protected float hoverTime = .2f;
    [SerializeField] protected float hoverScale = 1.1f;

    protected UIManager uiManager;
    protected SoundManager soundManager;

    private void Start() {
        OnStart();
    }

    protected virtual void OnStart() {
        var service = ServiceManager.Instance;
        uiManager = service.GetManager<UIManager>();
        soundManager = service.GetManager<SoundManager>();
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

    protected string GetFormattedText(int amount) {
        if (amount < 1000) return $"{amount}";
        else return $"{amount / 1000}.{(amount % 1000) / 100}k";
    }
}