using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearPlatform : MonoBehaviour
{

    private AudioManager audioManager;

    [SerializeField]
    [Tooltip("Appears on the on-beat.")]
    private bool onBeat;

    [SerializeField]
    [Tooltip("Deadly on the non-appearing beat.")]
    private bool deadlyWhenInactive;

    private bool deadly;

    private bool lastBeat;

    private SpriteRenderer sr;

    private Collider2D bc;

    private CapsuleCollider2D cc;

    /*[SerializeField]
    [Tooltip("Platform color.")]*/
    private Color enabledColor;

    private Color disabledColor;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = true;

        /* Resize the capsule collider to be just inside the box collider. */
        cc = GetComponent<CapsuleCollider2D>();
        if (deadlyWhenInactive)
        {
            Vector2 size = Vector2.zero;
            // size.y = 0.0088f + 0.0012f * Mathf.InverseLerp(500, 3000, transform.localScale.y);
            // size.x = 0.0088f + 0.0012f * Mathf.InverseLerp(500, 3000, transform.localScale.x);

            size.y = 0.01001f;
            size.x = 0.01001f;

            cc.size = size;
        } else
        {
            cc.enabled = false;
        }
            cc.enabled = false;

        deadly = false;

        sr = GetComponent<SpriteRenderer>();
        enabledColor = sr.color;
        if (deadlyWhenInactive)
        {
            disabledColor = Color.red;
        } else
        {
            Color disabled = enabledColor;
            disabled.a = enabledColor.a / 5;
            disabledColor = disabled;
        }

        lastBeat = audioManager.onBeat;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioManager.onBeat != lastBeat)
        {
            //StopCoroutine("Expand");
            if (audioManager.onBeat ^ onBeat)
            {
                StopCoroutine("Contract");
                StartCoroutine(Expand());
            }
            else
            {
                StopCoroutine("Expand");
                StartCoroutine(Contract());
            }
            lastBeat = audioManager.onBeat;
        }
    }

    private IEnumerator Expand()
    {
        float elapsed = 0f;
        deadly = false;

        while (elapsed < 0.17f)
        {
            elapsed += Time.deltaTime;

            sr.color = Color.Lerp(disabledColor, enabledColor, elapsed);
            yield return null;
        }
        sr.color = enabledColor;
        bc.enabled = true;
    }

    private IEnumerator Contract()
    {
        float elapsed = 0f;

        while (elapsed < 0.17f)
        {
            elapsed += Time.deltaTime;
            sr.color = Color.Lerp(enabledColor, disabledColor, elapsed);
            yield return null;
        }
        if (!deadlyWhenInactive)
        {
            bc.enabled = false;
        }
        deadly = true;
        sr.color = disabledColor;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        print("Triggered");
        if (deadlyWhenInactive && deadly && (audioManager.onBeat ^ onBeat) && other.gameObject.CompareTag("Player"))
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (!pc)
            {
                pc = other.gameObject.GetComponentInParent<PlayerController>();
            }
            pc.Die();
        }
    }

}
