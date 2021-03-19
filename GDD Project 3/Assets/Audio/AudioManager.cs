using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of songs to play in order")]
    private Track[] playlist;

    /*[SerializeField]
    [Tooltip("List of songs to play in order")]
    private AudioClip[] treble_playlist;

    [SerializeField]
    [Tooltip("List of songs to play in order")]
    private AudioClip[] bass_playlist;*/

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

    public float[] freqs;
    public float[] samples;
    private float[] temp;

    public bool onBeat { get; private set; }

    public int beatProgress { get; private set; }

    /* DFTs are O(N log(N)) and are costly; using a lower number
     * of them per second is preferable. On the frames that a DFT
     * isn't being calculated, we instead smooth out the transition
     * to the new set of frequencies generated to conserve.
  
     * seconds per frame to frames per second   *
     *              0.1     10                  *
     *        0.0833...     12                  *
     *           0.0625     16                  *
     *             0.05     20                  *
     *        0.0166...     60                  */
    public const float REFRESH_TIME = 0.0833f;


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
        freqs = new float[1024];
        samples = new float[16];
        temp = new float[16];

        trackCutoffs = new float[playlist.Length];
        trackCutoffs[0] = 20000f;
        for (int i = 1; i < playlist.Length; i++)
        {
            trackCutoffs[i] = 770f;
        }

        onBeat = true;

        t_src = treble.GetComponent<AudioSource>();
        t_lp = treble.GetComponent<AudioLowPassFilter>();

        b_src = bass.GetComponent<AudioSource>();
        b_lp = bass.GetComponent<AudioLowPassFilter>();

        trackNumber = Random.Range(0, playlist.Length);
        t_src.clip = playlist[trackNumber].Treble;
        b_src.clip = playlist[trackNumber].Bass;

        t_lp.cutoffFrequency = 1400f;
        b_lp.cutoffFrequency = 160f;

        spectrum();

        StartCoroutine(SyncPlayNextTrack(t_src, t_lp, playlist[trackNumber]));
        StartCoroutine(RefreshSpectrum());

    }

    /* Public muffle interface. */
    public void muffle(float f)
    {
        float inv = 22000f - f;
        float log = Mathf.Log(inv, 8);
        StopCoroutine("TrebleMuffle");
        StartCoroutine(TrebleMuffle(140f + Mathf.Pow(6, log)));
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
            src.clip = playlist[trackNumber].Treble;
        }

    }

    private IEnumerator SyncPlayNextTrack(AudioSource t_src, AudioLowPassFilter t_lp, 
                                            Track t)
    {
        while (true)
        {

            double elapsed = 0;
            double seconds_per_beat = 60d / t.BPM;

            t_src.Play();
            b_src.Play();

            /* Wait until the the first beat of the track plays. */
            while (elapsed < t.Offset)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            elapsed = 0;

            /* While the main section of the track plays, */
            while (t_src.isPlaying)
            {
                elapsed += Time.deltaTime;

                if (elapsed > seconds_per_beat)
                {
                    onBeat = !onBeat;
                    beatProgress = beatProgress ^ 1;
                    elapsed -= seconds_per_beat;
                }

                yield return null;
            }

            StopCoroutine(Muffle(trackCutoffs[trackNumber]));

            trackNumber += 1;

            /* Loop to start */
            if (trackNumber >= playlist.Length)
            {
                trackNumber = 0;
            }

            t_src.clip = playlist[trackNumber].Treble;
            b_src.clip = playlist[trackNumber].Bass;
        }

    }

    private IEnumerator RefreshSpectrum()
    {
        while (t_src.isPlaying)
        {
            spectrum();
            yield return new WaitForSeconds(REFRESH_TIME);
        }
    }


    private float[] spectrum()
    {
        t_src.GetSpectrumData(freqs, 0, FFTWindow.Rectangular);
        t_src.GetOutputData(temp, 0);
        t_src.GetOutputData(samples, 1);
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] += temp[i];
        }
        return freqs;
    }

    private int maxIndex(float[] a)
    {
        float m = 0;
        int index = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] > m)
            {
                m = a[i];
                index = i;
            }
        }
        return index;
    }


}