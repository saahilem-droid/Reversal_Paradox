using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private AudioClip buttonClick;

    [Header("Gameplay")]
    [SerializeField] private AudioClip cloneSpawn;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip levelComplete;
    [SerializeField] private AudioClip buttonHover;



    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayButtonHover()
{
    Play(buttonHover);
}

    public void PlayButtonClick()
    {
        Play(buttonClick);
    }

    public void PlayCloneSpawn()
    {
        Play(cloneSpawn);
    }

    public void PlayGameOver()
    {
        Play(gameOver);
    }

    public void PlayLevelComplete()
    {
        Play(levelComplete);
    }

    private void Play(AudioClip clip)
    {
        if (clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}