using DG.Tweening;
using UnityEngine;

public class LoadingUIHandler : MonoBehaviour {
    [Header("Values")]
    [SerializeField] private float loadingTime = 5f;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float bigPartSpeed = 700f;
    [SerializeField] private float smallPartSpeed = 700f;

    [Header("References")]
    [SerializeField] private Transform loading0Tf;
    [SerializeField] private Transform loading1Tf;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup backgroundGroup;

    private bool isActive = false;

    private void Start() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();
        uiManager.OnPlay += StartAnim;
    }

    private void Update() {
        if (!isActive) return;
        loading0Tf.eulerAngles += Vector3.forward * Time.deltaTime * bigPartSpeed;
        loading1Tf.eulerAngles += Vector3.forward * Time.deltaTime * smallPartSpeed;
    }

    public void StartAnim() {
        isActive = true;
        canvasGroup.DOFade(1f, fadeTime);
        Invoke("StopAnim", loadingTime);
    }

    public void StopAnim() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();
        uiManager.OnLoadingEnd?.Invoke();
        canvasGroup.DOFade(0f, fadeTime).OnComplete(() => {
            isActive = false;
            canvasGroup.interactable = false;
            uiManager.OnLoadingFadeEnd?.Invoke();
        });
        backgroundGroup.DOFade(0f, fadeTime);
    }
}