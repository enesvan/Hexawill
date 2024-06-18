using TMPro;
using UnityEngine;

public class ResourceUIHandler : InGameUIHandler {
    [Header("References")]
    [SerializeField] private TextMeshProUGUI humanResourceText;
    [SerializeField] private TextMeshProUGUI funResourceText;
    [SerializeField] private TextMeshProUGUI warResourceText;
    [SerializeField] private TextMeshProUGUI farmResourceText;
    [SerializeField] private TextMeshProUGUI mineResourceText;
    [SerializeField] private TextMeshProUGUI coinResourceText;

    private ResourceManager resourceManager;

    protected override void OnStart() {
        base.OnStart();
        var service = ServiceManager.Instance;
        resourceManager = service.GetManager<ResourceManager>();
        UpdateTexts();
    }

    public void UpdateTexts() {
        humanResourceText.text = GetFormattedText((int)resourceManager.ResourceData.Resources[0]);
        funResourceText.text = GetFormattedText((int)resourceManager.ResourceData.Resources[1]);
        warResourceText.text = GetFormattedText((int)resourceManager.ResourceData.Resources[2]);
        farmResourceText.text = GetFormattedText((int)resourceManager.ResourceData.Resources[3]);
        mineResourceText.text = GetFormattedText((int)resourceManager.ResourceData.Resources[4]);
        coinResourceText.text = GetFormattedText((int)resourceManager.ResourceData.Resources[5]);
    }
}
