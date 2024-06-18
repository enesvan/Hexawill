using UnityEngine;

public class PlayerManager : Manager {
    [SerializeField] private LayerMask interactableLayer;

    private GridManager gridManager;
    private UIManager uiManager;

    private Camera mainCamera;
    private Collider selectedTile;
    private bool isActive;

    public override void AwakeManager() {
        base.AwakeManager();
        var service = ServiceManager.Instance;
        service.RegisterManager<PlayerManager>(this);

        mainCamera = Camera.main;
        isActive = false;
    }

    private void Start() {
        var service = ServiceManager.Instance;
        gridManager = service.GetManager<GridManager>();
        uiManager = service.GetManager<UIManager>();

        uiManager.OnBuildOpen += SetPlayerDeActive;
        uiManager.OnBuildClose += SetPlayerActive;
        uiManager.OnUpgradeOpen += SetPlayerDeActive;
        uiManager.OnUpgradeClose += SetPlayerActive;
        uiManager.OnLoadingFadeEnd += SetPlayerActive;
        uiManager.OnSettingsOpen += SetPlayerDeActive;
        uiManager.OnSettingsClose += SetPlayerActive;
        uiManager.OnLoseOpen += SetPlayerDeActive;
    }

    private void Update() {
        if (!isActive) return;
        if (Input.GetMouseButtonDown(0)) {
            Interact();
        } else {
            Check();
        }
    }

    private void Check() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, interactableLayer)) {
            if (selectedTile == null) {
                selectedTile = hit.collider;
                Hover();
            } else if (selectedTile == hit.collider) return;
            else UnHover();
        } else {
            if (selectedTile != null) UnHover();
        }
    }

    private void Interact() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, interactableLayer)) {
            selectedTile = hit.collider;
            var grid = gridManager.GetHexagonGrid(selectedTile);
            if (grid.GetIsBuilded()) {
                uiManager.GetUpgradeUIHandler().MoveUI(grid.GetTransform().position);
                uiManager.OnUpgradeOpen?.Invoke();
            } else {
                uiManager.GetBuildUIHandler().MoveUI(grid.GetTransform().position);
                uiManager.OnBuildOpen?.Invoke();
            }
        }
    }

    private void Hover() => gridManager.GetHexagonGrid(selectedTile).Hover();
    private void UnHover() {
        gridManager.GetHexagonGrid(selectedTile).UnHover();
        selectedTile = null;
    }

    public void SetPlayerActive() {
        if (selectedTile != null) {
            gridManager.GetHexagonGrid(selectedTile).UnHover();
            selectedTile = null;
        }
        isActive = true;
    }

    public void SetPlayerDeActive() => isActive = false;
    public HexagonGrid GetSelectedHexagon() => gridManager.GetHexagonGrid(selectedTile);
}