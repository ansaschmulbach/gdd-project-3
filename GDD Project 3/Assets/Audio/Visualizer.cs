using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Number of samples (power of two)")]
    private int n_samples;

    [SerializeField]
    [Tooltip("Visualizer bar prefab.")]
    private GameObject BarObject;

    [SerializeField]
    [Tooltip("Bar width")]
    private float width;

    [SerializeField]
    [Tooltip("Maximum bar height")]
    private int height;

    [SerializeField]
    [Tooltip("How quickly the visualizer moves to its next value. Between 0 and 1.")]
    private float reactivity;

    //private float[] samples;

    private AudioManager audio;

    private GameObject[] bars;

    private Vector3[] barScales;

    // Start is called before the first frame update
    void Start()
    {
        if (n_samples != Mathf.NextPowerOfTwo(n_samples))
        {
            print("Sample rate must be a power of two.");
        }
        audio = AudioManager.instance;
        bars = new GameObject[n_samples];
        barScales = new Vector3[n_samples];
        for (int i = 0; i < n_samples; i++)
        {
            bars[i] = Instantiate(BarObject);
            bars[i].transform.position = transform.position + new Vector3(width * i / 90, 0);
            barScales[i] = new Vector3(width, 0, 0);
            bars[i].transform.localScale = barScales[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < n_samples / 6; i++)
        {
            barScales[i].y = Mathf.Lerp(barScales[i].y, height * audio.freqs[i], reactivity);
            bars[i].transform.localScale = barScales[i];
        }
    }

    private IEnumerator smoothResize()
    {
        yield return null;

    }
}
