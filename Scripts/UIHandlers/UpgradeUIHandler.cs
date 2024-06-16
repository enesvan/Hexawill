using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIHandler : UIHandler {
    [SerializeField] private Transform maxLevelPopUpTf;
    [SerializeField] private Image maxLevelPopUpImg;
    protected override void OnStart() {
        base.OnStart();
        uiManager.OnUpgradeOpen += OpenUI;
        maxLevelPopUpTf.localScale = Vector3.zero;
    }

    public void UpgradeButtonOnClick() {
        soundManager.PlayButtonPositiveSound();
        if (gridManager.GetActiveHexagonGrid().GridData.BuildLevel == 4) {
            PopUp(maxLevelPopUpTf);
            if (activeNumerator != null) StopCoroutine(activeNumerator);
            activeNumerator = PopUpColor(maxLevelPopUpImg);
            StartCoroutine(activeNumerator);
            return;
        } else if (!resourceManager.IsEnoughResource()) {
            PopUp(resourcePopUpTf);
            if (activeNumerator != null) StopCoroutine(activeNumerator);
            activeNumerator = PopUpColor(resourcePopUpImg);
            StartCoroutine(activeNumerator);
            return;
        }
        gridManager.GetActiveHexagonGrid().Upgrade();
        CloseUI();
    }

    public void DestroyButtonOnClick() {
        soundManager.PlayDestructionSound();
        gridManager.GetActiveHexagonGrid().DestroyBuilding();
        CloseUI();
    }

    protected override void CloseEvent() {
        uiManager.OnUpgradeClose?.Invoke();
    }
}
