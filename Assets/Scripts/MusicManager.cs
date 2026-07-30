using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public enum MusicType
    {
        Menu,
        Level
    }

    [Header("Scene Type")]
    [SerializeField] private MusicType musicType;

    [Header("Menu")]
    [SerializeField] private AudioClip menuMusic;

    [Header("Level")]
    [SerializeField] private AudioClip normalLevelMusic;
    [SerializeField] private AudioClip cloneLevelMusic;

    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float volume = 0.5f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = volume;
    }

    private void Start()
    {

        Debug.Log("Music Type: " + musicType);
        if (musicType == MusicType.Menu)
        {
            PlayMenuMusic();
        }
        else
        {
            PlayNormalLevelMusic();
        }
    }

    public void PlayMenuMusic()
    {
        if (menuMusic == null)
            return;

        if (audioSource.clip == menuMusic && audioSource.isPlaying)
            return;

        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    public void PlayNormalLevelMusic()
    {
        if (normalLevelMusic == null)
            return;

        if (audioSource.clip == normalLevelMusic && audioSource.isPlaying)
            return;

        audioSource.clip = normalLevelMusic;
        audioSource.Play();
    }

    public void PlayCloneMusic()
    {
        if (cloneLevelMusic == null)
            return;

        if (audioSource.clip == cloneLevelMusic && audioSource.isPlaying)
            return;

        audioSource.clip = cloneLevelMusic;
        audioSource.Play();
    }
}