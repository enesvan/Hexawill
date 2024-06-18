using UnityEngine;

public class SoundManager : Manager {
    [Header("References")]
    [SerializeField] private AudioSource backgroundSoundSource;
    [SerializeField] private AudioSource buttonPositiveSoundSource;
    [SerializeField] private AudioSource buttonNegativeSoundSource;
    [SerializeField] private AudioSource eventSoundSource;
    [SerializeField] private AudioSource loseSoundSource;
    [SerializeField] private AudioSource destructionSoundSource;

    public override void AwakeManager() {
        base.AwakeManager();
        var service = ServiceManager.Instance;
        service.RegisterManager<SoundManager>(this);
    }

    public void PlayButtonPositiveSound() => buttonPositiveSoundSource.Play();
    public void PlayButtonNegativeSound() => buttonNegativeSoundSource.Play();
    public void PlayEventSound() => eventSoundSource.Play();
    public void PlayLoseSound() => loseSoundSource.Play();
    public void PlayDestructionSound() => destructionSoundSource.Play();
}