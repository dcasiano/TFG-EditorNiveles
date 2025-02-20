using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null;
    public string mainSceneName;
    float musicVolume=1;
    public Sound[] sound;
    AudioSource mainTheme;
    Slider volume;
    [System.Serializable]
    public struct Sound
    {
        // Las siguientes 4 variables las guardarán cada componente del array de structs.
        public string name;
        public AudioClip clip;
        public bool looping;
        [Range(0,1)]
        public float volume;
        [HideInInspector]
        // A las variables de tipo AudioSource se les puede asignar un clip y un volumen entre otros valores (tienen su propio .clip y .volume que en este caso estará vacío hasta más adelante).
        public AudioSource source;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        mainTheme = GetComponent<AudioSource>();

        for(int i = 0; i < sound.Length; i++)
        {
            // Al iniciar se crean todos los nuevos componentes AudioSource según el número de componentes del array creado.
            sound[i].source = gameObject.AddComponent<AudioSource>();
            // A cada componente source de tipo AudioSource se le asigna el clip y el volumen guardado en el array, además de asignar si se ha seleccionado el loop del sonido a true.
            sound[i].source.clip = sound[i].clip;
            sound[i].source.volume = sound[i].volume;
            sound[i].source.loop = sound[i].looping;
        }
    }
    // Use this for initialization
    void Start () {
        // Comunica al GM de quien es el AudioManager.
        GameManager.instance.ThisAudioManager(this);
        // recoge la referencia del Slider que controla el volumen.
        Invoke("SetSliderReference", 0.1f);
        

    }
    void SetSliderReference()
    {
        volume = GameManager.instance.GetVolumeSlider();
    }
    private void Update()
    {
        if (volume!=null)
        SetMusicVolume(volume.value);
    }

    // Metodo para efectos de sonido
    public void PlayAudio(string name)
    {
        int i = 0;
        // Busca el componente del array con nombre name.
        while (i<sound.Length && sound[i].name != name)
        {
            i++;
        }
        try
        {
            sound[i].source.Play();
        }
        catch
        {
            //Si el índice se sale del array y no se ha podido reproducir el audio, se comunica.
            Debug.LogWarning("No existe el componente con nombre "+name+" cuyo audio se intenta reproducir.");
        }
    }

    // Metodo para musica de fondo
    public void PlayMainAudio(string name)
    {
        // recoge la referencia del Slider que controla el volumen.
        Invoke("SetSliderReference", 0.1f);
        int i = 0;
        // Busca el componente del array con nombre name.
        while (i<sound.Length && sound[i].name != name)
        {
            i++;
        }
        try
        {
            // Si el tema ya esta sonando, no lo vuelve a reproducir.
            if (mainTheme.clip != sound[i].source.clip)
            {
                mainTheme.clip = sound[i].source.clip;
                mainTheme.Play();
            }
            mainTheme.volume = musicVolume;
        }
        catch
        {
            //Si el índice se sale del array y no se ha podido reproducir el audio, se comunica.
            Debug.LogWarning("No existe el componente con nombre "+name+" cuyo audio se intenta reproducir.");
        }
    }

    //Metodo para cambiar volumen de musica
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        GameManager.instance.SetCurrentVolume(musicVolume);
        mainTheme.volume = musicVolume;
    }
}
