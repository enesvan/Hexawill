using System.Collections.Generic;
using UnityEngine;

public class GridManager : Manager {
    [SerializeField] private List<HexagonGrid> hexagonGrids;
    private Dictionary<Collider, HexagonGrid> hexagonGridsDic = new Dictionary<Collider, HexagonGrid>();
    private HexagonGrid activeGrid;

    #region CREATE GRID
    [Header("Grid Settings")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform tilesParentTf;

    [ContextMenu("CREATE GRIDS")]
    public void CreateGrids() {
        Vector3 worldPosition;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                worldPosition = grid.GetCellCenterWorld(new Vector3Int(i, j));
                Instantiate(tilePrefab, worldPosition, Quaternion.identity).transform.parent = tilesParentTf;
            }
        }
    }
    #endregion

    public override void AwakeManager() {
        base.AwakeManager();
        var service = ServiceManager.Instance;
        service.RegisterManager<GridManager>(this);

        RegisterGrids();
    }

    private void Start() {
        var service = ServiceManager.Instance;
        var saveManager = service.GetManager<SaveManager>();

        saveManager.OnSave += () => {
            saveManager.SaveData.GridDatas.Clear();
            foreach (var item in hexagonGrids) {
                saveManager.SaveData.GridDatas.Add(item.GridData);
            }
        };
        saveManager.OnLoad += () => {
            for(int i = 0; i < hexagonGrids.Count; i++) {
                hexagonGrids[i].GridData = saveManager.SaveData.GridDatas[i];
                hexagonGrids[i].Load();
            }
        };
    }

    private void RegisterGrids() {
        foreach (var grid in hexagonGrids) {
            hexagonGridsDic.Add(grid.GetCollider(), grid);
        }
    }

    public HexagonGrid GetHexagonGrid(Collider collider) {
        activeGrid = hexagonGridsDic[collider];
        return activeGrid;
    }
    public HexagonGrid GetActiveHexagonGrid() => activeGrid;
}