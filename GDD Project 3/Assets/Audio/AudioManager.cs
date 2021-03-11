using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of songs to play in order")]
    private AudioClip[] playlist;

    [SerializeField]
    [Tooltip("List of songs to play in order")]
    private AudioClip[] treble_playlist;

    [SerializeField]
    [Tooltip("List of songs to play in order")]
    private AudioClip[] bass_playlist;

    [SerializeField]
    [Tooltip("Treble audio manager")]
    private GameObject treble;

    [SerializeField]
    [Tooltip("Bass audio manager")]
    private GameObject bass;

    public static AudioManager instance = null;
    private AudioSource t_src;
    private AudioLowPassFilter t_lp;

    private AudioSource b_src;
    private AudioLowPassFilter b_lp;

    private int trackNumber;
    private float[] trackCutoffs;
    private float lastCutoff;
    private float transition = 0.3f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (instance == this) return;
        Destroy(gameObject);
    }

    void Start()
    {
        trackCutoffs = new float[playlist.Length];
        trackCutoffs[0] = 20000f;
        for (int i = 1; i < playlist.Length; i++)
        {
            trackCutoffs[i] = 770f;
        }
        t_src = treble.GetComponent<AudioSource>();
        t_lp = treble.GetComponent<AudioLowPassFilter>();

        b_src = bass.GetComponent<AudioSource>();
        b_lp = bass.GetComponent<AudioLowPassFilter>();

        trackNumber = Random.Range(0, playlist.Length);
        t_src.clip = playlist[trackNumber];

        StartCoroutine(SyncPlayNextTrack(t_src, t_lp, b_src, b_lp));
    }

    /* Muffle the track to cutoff frequency f & return
     * the previous cutoff frequency. */
    public IEnumerator Muffle(float f)
    {
        float elapsed = 0;
        while (elapsed < transition)
        {
            elapsed += Time.deltaTime;
            // t_lp.cutoffFrequency = Mathf.Lerp(t_lp.cutoffFrequency, f, elapsed / transition);
            b_lp.cutoffFrequency = Mathf.Lerp(b_lp.cutoffFrequency, f, elapsed / transition);
            yield return null;
        }
    }

    private bool audioNotPlaying()
    {
        return !t_src.isPlaying;
    }

    private IEnumerator PlayNextTrack(AudioSource src, AudioLowPassFilter lp)
    {
        while (true)
        {
            src.Play();
            while (src.isPlaying)
            {
                if (SceneManager.GetActiveScene().name == "MovementTestScene2")
                {
                    StartCoroutine(Muffle(
                        Mathf.Min(
                            trackCutoffs[trackNumber], 22000)
                        )
                    );
                }
                else
                {
                    StartCoroutine(Muffle(500f));
                }
                yield return new WaitForSeconds(0.75f);
            }

            StopCoroutine(Muffle(trackCutoffs[trackNumber]));
            trackNumber += 1;

            /* Loop to start */
            if (trackNumber >= playlist.Length)
            {
                trackNumber = 0;
            }

            /* Reset lowpass filter */
            lp.cutoffFrequency = trackCutoffs[trackNumber];
            src.clip = playlist[trackNumber];
        }

    }

    private IEnumerator SyncPlayNextTrack(AudioSource t_src, AudioLowPassFilter t_lp, 
                                            AudioSource b_src, AudioLowPassFilter b_lp)
    {
        while (true)
        {
            while (t_src.isPlaying)
            {
                if (SceneManager.GetActiveScene().name == "MovementTestScene2")
                {
                    StartCoroutine(Muffle(
                        Mathf.Min(
                            trackCutoffs[trackNumber], 2000)
                        )
                    );
                }
                else
                {
                    StartCoroutine(Muffle(500f));
                }
                yield return new WaitForSeconds(0.75f);
            }

            StopCoroutine(Muffle(trackCutoffs[trackNumber]));
            trackNumber += 1;

            /* Loop to start */
            if (trackNumber >= playlist.Length)
            {
                trackNumber = 0;
            }

            t_src.Play();
            b_src.Play();

            /* Reset lowpass filter */
            //t_lp.cutoffFrequency = trackCutoffs[trackNumber];
            //b_lp.cutoffFrequency = trackCutoffs[trackNumber];

            t_src.clip = treble_playlist[trackNumber];
            b_src.clip = bass_playlist[trackNumber];
        }

    }
}