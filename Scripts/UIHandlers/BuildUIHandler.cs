public class BuildUIHandler : UIHandler {
    protected override void OnStart() {
        base.OnStart();
        uiManager.OnBuildOpen += OpenUI;
    }

    public void BuildButtonOnClick(int id) {
        soundManager.PlayButtonPositiveSound();
        if (!resourceManager.IsEnoughResource()) {
            PopUp(resourcePopUpTf);
            if (activeNumerator != null) StopCoroutine(activeNumerator);
            activeNumerator = PopUpColor(resourcePopUpImg);
            StartCoroutine(activeNumerator);
            return;
        }
        gridManager.GetActiveHexagonGrid().Build(id);
        CloseUI();
    }

    protected override void CloseEvent() {
        uiManager.OnBuildClose?.Invoke();
    }
}