using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    /* Global audio manager. */
    [SerializeField]
    [Tooltip("Music controller.")]
    private GameObject musicController;

    private AudioManager audioManager;

    private bool activated;

    private Vector3 size;

    private Vector3 offset;

    private Vector3 loc;

    private float height;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = musicController.GetComponent<AudioManager>();
        size = transform.localScale;
        height = size.y / 2;

        loc = transform.localPosition;
        offset = Vector3.zero;
        activated = false;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            print("Checkpoint");
            if (!activated)
            {
                activated = true;
                StartCoroutine(Pulse());
            }
            
            // audioManager.muffle(3000f);
            audioManager.muffle(gameObject);
        }

    }

    private float Sum(float[] f)
    {
        float sum = 0;
        for (int i = 0; i < f.Length; i++)
        {
            sum += Mathf.Abs(f[i]);
        }
        return sum;
    }

    private IEnumerator Pulse()
    {
        float elapsed = 0f;
        float refresh = 0.0833f;

        while (elapsed < 5f)
        {
            elapsed += Time.deltaTime;
            refresh -= Time.deltaTime;
            if (refresh < 0)
            {
                size.y = audioManager.freqs[16] * 7;
                refresh = 0.08f;
            }
            transform.localScale = Vector3.Lerp(transform.localScale, size, 0.2f);
            offset.y = height - transform.localScale.y / 2;
            transform.localPosition = loc - offset;
            yield return null;
        }

        while (elapsed < 5.25f)
        {
            elapsed += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.2f);
            offset.y = height - transform.localScale.y / 2;
            transform.localPosition = loc - offset;
            yield return null;
        }
    }
}
