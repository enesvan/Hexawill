using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Sprite baseSprite;

    private Transform _transform;
    private EventTrigger eventTrigger;

    private void Awake() {
        _transform = transform;
        _transform.localScale = Vector3.zero;
        eventTrigger = GetComponent<EventTrigger>();
    }

    public void Open() {
        eventTrigger.enabled = true;
        _transform.localScale = Vector3.zero;
        _transform.DOKill();
        _transform.DOScale(1f, .1f);
    }

    public void Close() {
        eventTrigger.enabled = false;
        _transform.localScale = Vector3.one;
        _transform.DOKill();
        _transform.DOScale(0f, .1f);
    }

    public void Hover() {
        _transform.DOKill();
        _transform.DOScale(1.1f, .2f);
    }

    public void UnHover() {
        _transform.DOKill();
        _transform.DOScale(1f, .2f);
    }

    public void ActivateDeactivate(bool state) {
        button.enabled = state;
        image.sprite = baseSprite;
    }
}