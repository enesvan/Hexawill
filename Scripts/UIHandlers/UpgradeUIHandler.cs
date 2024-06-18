using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIHandler : InteractionUIHandler {
    [Header("References")]
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
            OnMaxLevel();
            return;
        } else if (!resourceManager.IsEnoughResource()) {
            NotEnoughResource();
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

    private void OnMaxLevel() {
        PopUp(maxLevelPopUpTf);
        if (activeNumerator != null) StopCoroutine(activeNumerator);
        activeNumerator = PopUpColor(maxLevelPopUpImg);
        StartCoroutine(activeNumerator);
    }

    protected override void CloseEvent() {
        uiManager.OnUpgradeClose?.Invoke();
    }
}
