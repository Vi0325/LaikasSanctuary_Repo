using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    
    public float fadeDuration = 1.5f;
    [Range(0f, 1f)] public float defaultMusicVolume = 0.5f;
    
    public AudioClip musicaMenu;
    public AudioClip musicaPrologo;
    public AudioClip musicaExplicacion;
    public AudioClip musicaDesafio;
    public AudioClip musicaExoPlaneta;
    public AudioClip musicaRestArea;
    public AudioClip musicaEndMenu; // <-- AÑADIDO PARA ENDMENU
    
    private Coroutine fadeCoroutine;
    private string currentSceneName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (musicSource != null)
                musicSource.volume = defaultMusicVolume;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        PlayMusicByLevel(scene.name);
    }

    public void PlayMusicByLevel(string levelName)
    {
        AudioClip clipToPlay = null;
        string musicType = "";
        
        switch (levelName)
        {
            case "MainMenu":
            case "CreditsScene":
            case "OptionsScene":
            case "ExplanationScene":
                clipToPlay = musicaMenu;
                musicType = "Menu";
                break;
                
            case "PrologueScene":
                clipToPlay = musicaPrologo;
                musicType = "Prologo";
                break;
                
            case "Level1":
                clipToPlay = musicaDesafio;
                musicType = "Desafio";
                break;
                
            case "RestArea":
                clipToPlay = musicaRestArea;
                musicType = "RestArea";
                break;
                
            case "ExoPlaneta":
                clipToPlay = musicaExoPlaneta;
                musicType = "ExoPlaneta";
                break;
                
            case "EndScene":
            case "EndMenu":
                clipToPlay = musicaEndMenu; // <-- USA LA MÚSICA ESPECÍFICA PARA ENDMENU
                musicType = "EndMenu";
                break;
                
            default:
                clipToPlay = musicaMenu;
                musicType = "Default";
                break;
        }
        
        if (clipToPlay != null)
        {
            PlayMusic(clipToPlay, musicType);
        }
    }

    public void ForceChangeToMenuMusic()
    {
        PlayMusicByLevel("MainMenu");
    }

    public void ForceChangeToGameMusic()
    {
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            PlayMusicByLevel(currentSceneName);
        }
        else if (musicaDesafio != null)
        {
            PlayMusic(musicaDesafio, "Game");
        }
    }

    public void SwitchToExplanationMusic()
    {
        if (musicaExplicacion != null)
            PlayMusic(musicaExplicacion, "Explicacion");
    }

    public void SwitchToCombatMusic()
    {
        if (musicaDesafio != null)
            PlayMusic(musicaDesafio, "Combate");
    }

    public void PlayMusic(AudioClip clip, string nombre = "")
    {
        if (clip == null || musicSource == null) return;
        
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
            
        fadeCoroutine = StartCoroutine(FadeToMusic(clip));
        Debug.Log(clip.name + " (" + nombre + ")");
    }

    IEnumerator FadeToMusic(AudioClip newClip)
    {
        float targetVolume = defaultMusicVolume;
        
        if (musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }
            musicSource.Stop();
        }
        
        musicSource.clip = newClip;
        musicSource.Play();
        musicSource.volume = 0;
        
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }
        
        fadeCoroutine = null;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        defaultMusicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = defaultMusicVolume;
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}