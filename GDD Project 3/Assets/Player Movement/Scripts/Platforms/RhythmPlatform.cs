using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmPlatform : MonoBehaviour
{

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

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        contractSize = transform.localScale;
        height = contractSize.y / 200; // Divide by two to center, divide by 100 because we're using a 1x1 texture.

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

        while (elapsed < 0.16f)
        {
            elapsed += Time.deltaTime;

            Vector3 newScale = Vector3.Lerp(transform.localScale, expandSize, transitionSpeed);
            offset.y = height - newScale.y / 200;
            if (player)
            {
                player.transform.position = new Vector3(player.transform.position.x, transform.localPosition.y - offset.y + player.transform.localScale.y / 2);
            }
            transform.localScale = newScale;

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

    #region Collision Methods

    private void OnCollisionEnter2D(Collision2D other)
    {

        PlayerMovement pm_child = other.collider.GetComponent<PlayerMovement>();
        PlayerMovement pm_parent = other.collider.GetComponentInParent<PlayerMovement>();
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
            return;
        }

        if (pm.isActive)
        {
            Debug.Log(other.gameObject);
            player = pm.gameObject;

        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }

    }

    #endregion

}
