using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitioner : MonoBehaviour
{

    [SerializeField] private float timeToFade = 1;
    [SerializeField] private string nextLevel;
    [SerializeField] private float levelTransitionTime;
    [SerializeField] private Image blackOverlay;

    private bool isActive;
    private SpriteRenderer sr;
    private GameObject player;
    private static LevelTransitioner instance;
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        sr = GetComponent<SpriteRenderer>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerMovement>().isActive)
            {
                player = p;
            }
        }

        //this.transform.position = player.transform.position;
        DisableTransition();
        
    }

    public void EnableTransition()
    {
        this.isActive = true;
        this.sr.enabled = true;
    }

    void DisableTransition()
    {
        this.isActive = false;
        this.sr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<PlayerController>().DisableMovement();
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        yield return MovePlayer();
        yield return Fade(1);
        SceneManager.LoadScene(nextLevel);
        DisableTransition();
        yield return Fade(0);
        Destroy(this.gameObject);
    }

    IEnumerator MovePlayer()
    {
        float elapsedTime = 0;
        Vector3 endPos = new Vector3(this.transform.position.x, player.transform.position.y, player.transform.position.z);
        while (player.transform.position != endPos)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, endPos, elapsedTime/levelTransitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Fade(float newAlpha)
    {
        float elapsedTime = 0;
        Color endcolor = Color.black;
        endcolor.a = newAlpha;
        while (blackOverlay.color != endcolor)
        {
            blackOverlay.color = Color.Lerp(blackOverlay.color, endcolor, elapsedTime/timeToFade);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    
}
