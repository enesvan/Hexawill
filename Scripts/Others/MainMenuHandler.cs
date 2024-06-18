using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour {
    [Header("References")]
    [SerializeField] private List<GameObject> toCloses;
    [SerializeField] private List<GameObject> toOpens;

    private void Start() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();

        foreach (var item in toCloses) item.SetActive(true);
        foreach (var item in toOpens) item.SetActive(false);

        uiManager.OnLoadingFadeEnd += () => {
            foreach (var item in toCloses) item.SetActive(false);
            foreach (var item in toOpens) item.SetActive(true);
        };
    }
}