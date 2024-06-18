using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUIHandler : InGameUIHandler {
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private Image eventResourceImg;
    [SerializeField] private List<Sprite> resourceSprites;

    private EventManager eventManager;
    private bool isActive = false;
    private float aimedSliderValue;

    protected override void OnStart() {
        base.OnStart();
        var service = ServiceManager.Instance;
        eventManager = service.GetManager<EventManager>();

        uiManager.OnPlay += () => {
            slider.minValue = 0;
            slider.maxValue = eventManager.GetEventTime() * 2f;
            slider.value = slider.maxValue;
            aimedSliderValue = slider.value / 2f;
        };
        uiManager.OnLoadingEnd += SetEvent;
    }

    private void Update() {
        if (!isActive) return;
        slider.value -= Time.deltaTime;
        if (slider.value <= aimedSliderValue) {
            isActive = false;
            if (!eventManager.EventSuccessCheck()) return;
            SetEvent();
        }
    }
    
    public void SetEvent() {
        var random = Random.Range(0, resourceSprites.Count);
        eventManager.SetActiveResourceType(random);
        eventResourceImg.sprite = resourceSprites[random];
        eventText.text = GetFormattedText(eventManager.GetNewResource());
        slider.DOValue(slider.maxValue, 1f).OnComplete(() => {
            isActive = true;
            soundManager.PlayEventSound();
        });
    }
}