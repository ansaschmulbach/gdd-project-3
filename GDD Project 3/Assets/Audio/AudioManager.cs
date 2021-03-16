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

    [SerializeField]
    [Tooltip("Checkpoints at which to re-equalize audio")]
    private GameObject[] checkpoints;

    public static AudioManager instance = null;
    private AudioSource t_src;
    private AudioLowPassFilter t_lp;

    private AudioSource b_src;
    private AudioLowPassFilter b_lp;

    private int trackNumber;
    private float[] trackCutoffs;
    private float lastCutoff;
    private float transition = 0.6f;

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
        t_src.clip = treble_playlist[trackNumber];
        b_src.clip = bass_playlist[trackNumber];

        t_lp.cutoffFrequency = 1400f;
        b_lp.cutoffFrequency = 160f;

        StartCoroutine(SyncPlayNextTrack(t_src, t_lp, b_src, b_lp));
    }

    /* Public muffle interface. */
    public void muffle(float f)
    {
        float inv = 22000f - f;
        float log = Mathf.Log(inv, 8);
        StopCoroutine("TrebleMuffle");
        StartCoroutine(TrebleMuffle(140f + Mathf.Pow(5, log)));
        StopCoroutine("BassMuffle");
        StartCoroutine(BassMuffle(f));
    }

    public void muffle(GameObject checkpoint)
    {
        /* TODO: if PlayerOne, progressively unmuffle treble
         *       if PlayerTwo, progressively unmuffle bass   */

        for (int i = 0; i < checkpoints.Length; i++)
        {
            GameObject c = checkpoints[i];
            if (checkpoint == c)
            {
                muffle(Mathf.Lerp(160f, trackCutoffs[trackNumber], (i + 1f) * (i + 1f) / (checkpoints.Length * checkpoints.Length)));
            }
        }
    }

    /* Muffle the track to cutoff frequency f & return
     * the previous cutoff frequency. */
    private IEnumerator BassMuffle(float f)
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

    private IEnumerator TrebleMuffle(float f)
    {
        float elapsed = 0;
        float originalFrequency = t_lp.cutoffFrequency;
        while (elapsed < transition)
        {
            elapsed += Time.deltaTime;
            t_lp.cutoffFrequency = Mathf.Lerp(originalFrequency, f, elapsed / transition);
            yield return null;
        }
    }

    private IEnumerator Muffle(float f)
    {
        float elapsed = 0;
        float tOriginalFrequency = t_lp.cutoffFrequency;
        while (elapsed < transition)
        {
            elapsed += Time.deltaTime;
            t_lp.cutoffFrequency = Mathf.Lerp(tOriginalFrequency, f, elapsed / transition);
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
                /*if (SceneManager.GetActiveScene().name == "MovementTestScene2")
                {
                    StartCoroutine(Muffle(
                        Mathf.Min(
                            trackCutoffs[trackNumber], 2000)
                        )
                    );
                }*/
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