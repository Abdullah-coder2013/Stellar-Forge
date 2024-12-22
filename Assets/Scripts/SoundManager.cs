using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource audioSourcePrefab;
    
    public SoundManager()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void PlaySpecifiedSound(AudioClip clip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        var clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
    public void PlayRandomSpecifiedSound(AudioClip[] clips, Transform spawnTransform, float volume)
    {
        int randomIndex = Random.Range(0, clips.Length);
        var clip = clips[randomIndex];
        AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        var clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
