public class BuildUIHandler : InteractionUIHandler {
    protected override void OnStart() {
        base.OnStart();
        uiManager.OnBuildOpen += OpenUI;
    }

    public void BuildButtonOnClick(int id) {
        soundManager.PlayButtonPositiveSound();
        if (!resourceManager.IsEnoughResource()) {
            NotEnoughResource();
            return;
        }
        gridManager.GetActiveHexagonGrid().Build(id);
        CloseUI();
    }

    protected override void CloseEvent() {
        uiManager.OnBuildClose?.Invoke();
    }
}