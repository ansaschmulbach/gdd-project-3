using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer2 : MonoBehaviour
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

    [SerializeField]
    [Tooltip("Will the bars expand in both directions or just one?")]
    private bool doubleSided;

    private float ScaleY;

    private AudioManager audio;

    private GameObject[] bars;

    private Vector3[] barScales;

    private Vector3[] barPositions;

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
        barPositions = new Vector3[n_samples];
        if (doubleSided)
        {
            barPositions = new Vector3[n_samples];
        }

        for (int i = 0; i < n_samples; i++)
        {
            bars[i] = Instantiate(BarObject);
            ScaleY = bars[i].transform.localScale.y / 2;
            bars[i].transform.position = transform.position + new Vector3(width * i / 90, 0);

            barPositions[i] = bars[i].transform.position;
            barScales[i] = new Vector3(width, 0, 0);
            bars[i].transform.localScale = barScales[i];
        }
    }

    // Update is called once per frame
    void Update2()
    {
        for (int i = 0; i < n_samples / 16; i++)
        {
            barScales[i].y = Mathf.Lerp(barScales[i].y, height * audio.freqs[i], reactivity);
            bars[i].transform.localScale = barScales[i];
        }
    }

    void Update()
    {
        Vector3 barOffset = Vector3.zero;
        for (int i = 0; i < n_samples / 16; i++)
        {
            barScales[i].y = Mathf.Lerp(barScales[i].y, height * audio.freqs[i], reactivity) * 10;
            bars[i].transform.localScale = barScales[i];
            barOffset.y = ScaleY - barScales[i].y / 20;
            bars[i].transform.localPosition = barPositions[i] - barOffset;

        }
    } 

    private IEnumerator smoothResize()
    {
        yield return null;

    }
}
