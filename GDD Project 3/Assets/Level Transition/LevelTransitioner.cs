using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour
{

    [SerializeField] private string nextLevel;
    [SerializeField] private float levelTransitionTime;
    private bool isActive;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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
        if (isActive)
        {
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        yield return new WaitForSeconds(levelTransitionTime);
        SceneManager.LoadScene(nextLevel);
    }
    
}
