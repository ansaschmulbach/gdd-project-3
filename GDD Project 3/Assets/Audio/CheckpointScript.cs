using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    /* Global audio manager. */
    private AudioManager audioManager;

    private AudioSource audioSource;

    private bool activated;

    private Vector3 size;

    private Vector3 offset;

    private Vector3 loc;

    private float height;

    [SerializeField] [Tooltip("Which checkpoint is this? (zero-indexed)")]
    private int order;

    // Start is called before the first frame update
    void Start()
    {
        /* Add this checkpoint to the checkpoint list.
         * If not done, the checkpoint can't be interacted
         * with after the player dies due to a null pointer 
         * exception. */
        audioManager = AudioManager.instance;
        audioManager.checkpoints[order] = gameObject;

        audioSource = GetComponent<AudioSource>();

        /* Store the original position & scale of the checkpoint to help it bob in place. */
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
            print("Triggered");
            PlayerMovement pm = GetPlayerMovement(c);
            if (!pm) return;

            if (!activated && pm.canWallJump)
            {
                activated = true;
                audioSource.Play();
                StopCoroutine(Pulse());
                StartCoroutine(Pulse());
            } else if (activated && !pm.canWallJump)
            {
                activated = false;
                audioSource.Play();
                StopCoroutine(Pulse());
                StartCoroutine(Pulse());
            }

            // audioManager.muffle(3000f);
            audioManager.muffle(gameObject, pm.canWallJump);
        }

    }

    private PlayerMovement GetPlayerMovement(Collider2D c)
    {
        PlayerMovement pm_child = c.gameObject.GetComponent<PlayerMovement>();
        PlayerMovement pm_parent = c.gameObject.GetComponentInParent<PlayerMovement>();
        PlayerMovement pm = null;
        if (pm_child != null)
        {
            pm = pm_child;
        }
        else if (pm_parent != null)
        {
            pm = pm_parent;
        }
        else
        {
            return null;
        }
        return pm;
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
