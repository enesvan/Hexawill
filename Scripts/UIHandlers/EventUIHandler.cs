using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUIHandler : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private Image eventResourceImg;
    [SerializeField] private List<Sprite> resourceSprites;

    private EventManager eventManager;
    private SoundManager soundManager;
    private bool isActive = false;
    private float aimedSliderValue;

    private void Start() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();
        eventManager = service.GetManager<EventManager>();
        soundManager = service.GetManager<SoundManager>();

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

    private string GetFormattedText(int amount) {
        if (amount < 1000) return $"{amount}";
        else return $"{amount / 1000}.{(amount % 1000) / 100}k";
    }
}