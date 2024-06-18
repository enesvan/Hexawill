using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Manager {
    public ResourceData ResourceData;

    [Header("Values")]
    [SerializeField] private float resourceSequenceTime = 5f;
    [SerializeField] private float resourceMultiplier;
    [SerializeField] private float resourceUsageAmount = 100f;

    private UIManager uiManager;

    public override void AwakeManager() {
        base.AwakeManager();
        var service = ServiceManager.Instance;
        service.RegisterManager<ResourceManager>(this);
    }

    private void Start() {
        var service = ServiceManager.Instance;
        var saveManager = service.GetManager<SaveManager>();
        uiManager = service.GetManager<UIManager>();

        saveManager.OnSave += () => {
            saveManager.SaveData.ResourceData = ResourceData;
        };
        saveManager.OnLoad += () => {
            ResourceData = saveManager.SaveData.ResourceData;
        };
        uiManager.OnLoadingEnd += () => StartCoroutine(ResourceUpdateSequence());
        uiManager.OnLoseOpen += () => StopAllCoroutines();
    }

    private IEnumerator ResourceUpdateSequence() {
        uiManager.GetResourceUIHandler().UpdateTexts();
        while (true) {
            yield return new WaitForSeconds(resourceSequenceTime);
            UpdateResources();
        }
    }

    private void UpdateResources() {
        for(int i = 0; i < ResourceData.Resources.Count; i++) {
            ResourceData.Resources[i] += ResourceData.Levels[i] * resourceMultiplier;
        }
        uiManager.GetResourceUIHandler().UpdateTexts();
    }

    public void UpdateLevels(int id, int way) {
        ResourceData.Levels[id] += way;
        if (way == 1) UseResource();
    }

    private void UseResource() {
        for(int i = 0; i < ResourceData.Resources.Count; i++) {
            ResourceData.Resources[i] -= resourceUsageAmount;
        }
        uiManager.GetResourceUIHandler().UpdateTexts();
    }

    public bool IsEnoughResource() {
        foreach(var resource in ResourceData.Resources) {
            if (resource < resourceUsageAmount)
                return false;
        }
        return true;
    }
}

[System.Serializable]
public class ResourceData {
    public List<int> Levels;
    public List<float> Resources;
}