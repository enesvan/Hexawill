using UnityEngine;

public class CameraInputHandler : MonoBehaviour {
    [Header("Values")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationEaseMultiplier = 80f;

    private float rotationEase = 0f;
    private int rotationWay = 1;
    private Transform _transform;
    private bool isActive;

    private void Awake() {
        _transform = transform;
        isActive = false;
    }

    private void Start() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();

        uiManager.OnBuildOpen += SetHandlerDeActive;
        uiManager.OnBuildClose += SetHandlerActive;
        uiManager.OnUpgradeOpen += SetHandlerDeActive;
        uiManager.OnUpgradeClose += SetHandlerActive;
        uiManager.OnLoadingFadeEnd += SetHandlerActive;
        uiManager.OnSettingsOpen += SetHandlerDeActive;
        uiManager.OnSettingsClose += SetHandlerActive;
        uiManager.OnLoseOpen += SetHandlerDeActive;
    }

    private void Update() {
        if (!isActive) return;
        if (Input.GetKey(KeyCode.E)) {
            rotationEase = rotationSpeed;
            rotationWay = 1;
            _transform.eulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
            
        } else if (Input.GetKey(KeyCode.Q)) {
            rotationEase = rotationSpeed;
            rotationWay = -1;
            _transform.eulerAngles += Vector3.down * rotationSpeed * Time.deltaTime;
            
        } else if (rotationEase > 0f) {
            _transform.eulerAngles += Vector3.up * rotationWay * rotationEase * Time.deltaTime;
            rotationEase -= rotationEaseMultiplier * Time.deltaTime;
        }
    }

    public void SetHandlerActive() => isActive = true;
    public void SetHandlerDeActive() {
        isActive = false;
        rotationEase = 0f;
    }
}