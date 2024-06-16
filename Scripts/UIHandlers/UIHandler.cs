using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    [SerializeField] protected List<BuildButton> buttons;
    [SerializeField] protected GameObject buttonsObj;
    [SerializeField] protected Transform resourcePopUpTf;
    [SerializeField] protected Image resourcePopUpImg;
    [SerializeField] protected Color popUpColor;

    protected Transform buttonsTf;
    protected UIManager uiManager;
    protected GridManager gridManager;
    protected ResourceManager resourceManager;
    protected Camera mainCamera;
    protected IEnumerator activeNumerator = null;
    protected SoundManager soundManager;

    private void Awake() {
        buttonsTf = buttonsObj.transform;
        buttonsObj.SetActive(false);
    }

    private void Start() {
        OnStart();
    }

    protected virtual void OnStart() {
        var service = ServiceManager.Instance;
        uiManager = service.GetManager<UIManager>();
        gridManager = service.GetManager<GridManager>();
        resourceManager = service.GetManager<ResourceManager>();
        soundManager = service.GetManager<SoundManager>();
        mainCamera = Camera.main;
        resourcePopUpTf.localScale = Vector3.zero;
    }

    protected void OpenUI() {
        soundManager.PlayButtonPositiveSound();
        buttonsObj.SetActive(true);
        StartCoroutine(ReOpen(0));
    }

    // OnClick
    public void CloseUI() {
        foreach (var button in buttons) button.ActivateDeactivate(false);
        StartCoroutine(ReClose(buttons.Count - 1));
    }

    protected IEnumerator ReOpen(int index) {
        buttons[index].Open();
        yield return new WaitForSeconds(.1f);
        if (++index != buttons.Count) StartCoroutine(ReOpen(index));
        else {
            foreach (var button in buttons) button.ActivateDeactivate(true);
        }
    }

    protected IEnumerator ReClose(int index) {
        buttons[index].Close();
        yield return new WaitForSeconds(.1f);
        if (--index != -1) StartCoroutine(ReClose(index));
        else {
            buttonsObj.SetActive(false);
            CloseEvent();
        }
    }

    public void MoveUI(Vector3 position) {
        var screenPosition = mainCamera.WorldToScreenPoint(position);
        buttonsTf.position = screenPosition;
    }

    protected virtual void CloseEvent() { }

    protected void PopUp(Transform tf) {
        var position = mainCamera.WorldToScreenPoint(gridManager.GetActiveHexagonGrid().GetTransform().position);
        tf.position = position;
        tf.localScale = Vector3.zero;
        tf.DOKill();
        tf.DOScale(Vector3.one, .25f);
        tf.DOLocalMoveY(tf.localPosition.y + 80f, 2f).SetEase(Ease.Linear).OnComplete(() => {
            tf.DOScale(Vector3.zero, .25f);
        });
        CloseUI();
    }

    protected IEnumerator PopUpColor(Image image) {
        image.color = Color.white;
        yield return new WaitForSeconds(.25f);
        image.color = popUpColor;
        yield return new WaitForSeconds(.25f);
        image.color = Color.white;
        yield return new WaitForSeconds(.25f);
        image.color = popUpColor;
        yield return new WaitForSeconds(.25f);
        image.color = Color.white;
        yield return new WaitForSeconds(.25f);
        image.color = popUpColor;
        yield return new WaitForSeconds(.25f);
        image.color = Color.white;
        yield return new WaitForSeconds(.25f);
        image.color = popUpColor;
        yield return new WaitForSeconds(.25f);
        image.color = Color.white;
    }
}
