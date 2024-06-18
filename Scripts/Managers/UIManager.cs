using System;
using UnityEngine;

public class UIManager : Manager {
    public Action OnBuildOpen;
    public Action OnBuildClose;
    public Action OnUpgradeOpen;
    public Action OnUpgradeClose;
    public Action OnPlay;
    public Action OnLoadingEnd;
    public Action OnLoadingFadeEnd;
    public Action OnSettingsOpen;
    public Action OnSettingsClose;
    public Action OnLoseOpen;

    [Header("References")]
    [SerializeField] private BuildUIHandler buildUIHandler;
    [SerializeField] private UpgradeUIHandler upgradeUIHandler;
    [SerializeField] private ResourceUIHandler resourceUIHandler;
    [SerializeField] private EventUIHandler eventUIHandler;
    [SerializeField] private LoseUIHandler loseUIHandler;

    public override void AwakeManager() {
        base.AwakeManager();
        var service = ServiceManager.Instance;
        service.RegisterManager<UIManager>(this);
    }

    public BuildUIHandler GetBuildUIHandler() => buildUIHandler;
    public UpgradeUIHandler GetUpgradeUIHandler() => upgradeUIHandler;
    public ResourceUIHandler GetResourceUIHandler() => resourceUIHandler;
    public EventUIHandler GetEventUIHandler() => eventUIHandler;
    public LoseUIHandler GetLoseUIHandler() => loseUIHandler;
}