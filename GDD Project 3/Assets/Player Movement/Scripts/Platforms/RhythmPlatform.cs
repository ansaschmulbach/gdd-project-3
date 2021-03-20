using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmPlatform : MonoBehaviour
{
    /* Global audio manager. */
    [SerializeField]
    [Tooltip("Music controller.")]
    private GameObject musicController;

    [SerializeField]
    [Tooltip("How many times taller this platform gets.")]
    private float expandedSize;

    [SerializeField]
    [Tooltip("How fast a transition occurs. (Coefficient of a geometric series between 0 & 1)")]
    private float transitionSpeed;

    private AudioManager audioManager;

    private Vector3 contractSize;

    private Vector3 expandSize;

    private Vector3 offset;

    private Vector3 loc;

    private float height;

    private bool lastBeat;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = musicController.GetComponent<AudioManager>();
        contractSize = transform.localScale;
        height = contractSize.y / 200;

        expandSize = new Vector3(contractSize.x, expandedSize * contractSize.y);

        lastBeat = true;

        loc = transform.localPosition;
        offset = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioManager.onBeat != lastBeat)
        {
            //StopCoroutine("Expand");
            if (audioManager.onBeat)
            {
                StopCoroutine("Contract");
                StartCoroutine(Expand(expandSize));
            }
            else
            {
                StopCoroutine("Expand");
                StartCoroutine(Contract());
            }
            lastBeat = audioManager.onBeat;
        }

    }

    private IEnumerator Expand(Vector3 newSize)
    {
        float elapsed = 0f;

        while (elapsed < 0.16f)
        {
            elapsed += Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, newSize, transitionSpeed);
            offset.y = height - transform.localScale.y / 200;
            transform.localPosition = loc - offset;
            yield return null;
        }
    }

    private IEnumerator Contract()
    {
        float elapsed = 0f;

        while (elapsed < 0.16f)
        {
            elapsed += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(transform.localScale, contractSize, transitionSpeed);
            offset.y = height - transform.localScale.y / 200;
            transform.localPosition = loc - offset;
            yield return null;
        }
    }
}
