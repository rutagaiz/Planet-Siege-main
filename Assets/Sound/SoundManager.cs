using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource Level1;
    public AudioSource Menu;
    public AudioSource Gun;

    

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLevel1Music()
    {
        if (!Level1.isPlaying)
        {
            Level1.loop = true;
            Level1.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        Level1.Stop();
    }

    public void PlayMenuMusic()
    {
        if (!Level1.isPlaying)
        {
            Menu.loop = true;
            Menu.Play();
        }
    }

    public void StopMenuMusic()
    {
        Menu.Stop();
    }

    public void PlayGunSound()
    {
        Gun.PlayOneShot(Gun.clip);
    }
}
