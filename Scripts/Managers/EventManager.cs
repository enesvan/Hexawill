using UnityEngine;

public class EventManager : Manager {
    public EventData EventData;
    [SerializeField] private float eventTime = 120f;
    [SerializeField] private int difficultyInDay = 100;

    private int activeEventResource;
    private int activeResourceId;
    private ResourceManager resourceManager;
    
    public override void AwakeManager() {
        base.AwakeManager();
        var service = ServiceManager.Instance;
        service.RegisterManager<EventManager>(this);
    }

    private void Start() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();
        var saveManager = service.GetManager<SaveManager>();
        resourceManager = service.GetManager<ResourceManager>();

        saveManager.OnSave += () => saveManager.SaveData.EventData.Day = EventData.Day;
        saveManager.OnLoad += () => EventData.Day = saveManager.SaveData.EventData.Day;
    }

    public float GetEventTime() => eventTime;
    public int GetDifficultyInDay() => difficultyInDay;
    public int GetNewResource() {
        if (EventData.Day != 0) activeEventResource = Random.Range(EventData.Day * difficultyInDay, (EventData.Day * difficultyInDay) + (difficultyInDay + (EventData.Day / 8) * difficultyInDay));
        else activeEventResource = Random.Range(0, 100);
        return activeEventResource;
    }
    public void SetActiveResourceType(int id) => activeResourceId = id;
    public bool EventSuccessCheck() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();
        if (resourceManager.ResourceData.Resources[activeResourceId] < activeEventResource) {
            uiManager.GetLoseUIHandler().OpenUI();
            uiManager.OnLoseOpen?.Invoke();
            return false;
        } else {
            resourceManager.ResourceData.Resources[activeResourceId] -= activeEventResource;
            EventData.Day++;
            uiManager.GetResourceUIHandler().UpdateTexts();
            service.GetManager<SaveManager>().Save();
            return true;
        }
    }
}

[System.Serializable]
public class EventData {
    public int Day;
}