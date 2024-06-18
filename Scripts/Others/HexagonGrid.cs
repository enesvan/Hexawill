using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HexagonGrid : MonoBehaviour {
    public GridData GridData;

    [Header("Values")]
    [SerializeField] private float hoverEasePosition = .5f;
    [SerializeField] private float hoverEaseTime = .5f;
    [SerializeField] private Color hoverColor = Color.gray;
    [SerializeField] private Vector3 lookAtPosition;

    [Header("References")]
    [SerializeField] private List<Transform> buildingTfs;
    [SerializeField] private ParticleSystem smokeVfx;
    [SerializeField] private GameObject gridObj;
    [SerializeField] private Collider _collider;
    [SerializeField] private MeshRenderer _renderer;

    private Color initColor;
    private Transform _transform;
    private bool isUpgraded = false;

    private ResourceManager resourceManager;

    private void Awake() {
        _transform = transform;
        initColor = _renderer.material.color;
        foreach (Transform tf in buildingTfs) {
            tf.gameObject.SetActive(false);
            tf.localScale = Vector3.zero;
            tf.LookAt(lookAtPosition);
        }
    }

    private void Start() {
        var service = ServiceManager.Instance;
        resourceManager = service.GetManager<ResourceManager>();
    }

    public void Load() {
        if (!GridData.IsBuilded) return;
        buildingTfs[GridData.BuildId].gameObject.SetActive(true);
        buildingTfs[GridData.BuildId].DOScale(1f, hoverEaseTime);
        _transform.DOMoveY(hoverEasePosition * GridData.BuildLevel - hoverEasePosition, hoverEaseTime);
        if (GridData.BuildId == 0) smokeVfx.Play();
    }

    public void Build(int id) {
        buildingTfs[id].gameObject.SetActive(true);
        buildingTfs[id].DOScale(1f, hoverEaseTime);
        GridData.IsBuilded = true;
        GridData.BuildId = id;
        resourceManager.UpdateLevels(GridData.BuildId, 1);
        if (GridData.BuildId == 0) smokeVfx.Play();
    }

    public void Upgrade() {
        isUpgraded = true;
        GridData.BuildLevel++;
        resourceManager.UpdateLevels(GridData.BuildId, 1);
        MoveGridUnHover();
        Invoke("SetIsUpgraded", hoverEaseTime);
    }

    public void DestroyBuilding() {
        for (int i = 0; i < GridData.BuildLevel; i++) resourceManager.UpdateLevels(GridData.BuildId, -1);
        if(GridData.BuildId == 0) smokeVfx.Stop();
        buildingTfs[GridData.BuildId].DOScale(0f, hoverEaseTime).OnComplete(() => {
            buildingTfs[GridData.BuildId].gameObject.SetActive(false);
            ResetData();
            MoveGridUnHover();
        });
    }

    public void Hover() {
        _renderer.material.color = hoverColor;
        if (GridData.BuildLevel == 4) return;
        MoveGridHover();
    }

    public void UnHover() {
        _renderer.material.color = initColor;
        if (isUpgraded) return;
        MoveGridUnHover();
    }

    public Collider GetCollider() => _collider;
    public Transform GetTransform() => _transform;
    public bool GetIsBuilded() => GridData.IsBuilded;
    private void SetIsUpgraded() => isUpgraded = false;

    private void MoveGridUnHover() {
        _transform.DOKill();
        _transform.DOMoveY(hoverEasePosition * GridData.BuildLevel - hoverEasePosition, hoverEaseTime);
    }

    private void MoveGridHover() {
        _transform.DOKill();
        _transform.DOMoveY(hoverEasePosition * GridData.BuildLevel, hoverEaseTime);
    }

    private void ResetData() {
        GridData.BuildLevel = 1;
        GridData.BuildId = 0;
        GridData.IsBuilded = false;
    }
}

[System.Serializable]
public class GridData {
    public int BuildLevel = 1;
    public int BuildId;
    public bool IsBuilded;
}