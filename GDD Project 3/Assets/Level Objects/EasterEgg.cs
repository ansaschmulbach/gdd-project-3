using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{

    private AudioSource asrc;

    // Start is called before the first frame update
    void Start()
    {
        asrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.CompareTag("Player"))
        {
            asrc.Play();
            AudioManager am = AudioManager.instance;
            am.volume(0f);
            asrc.volume = 0.7f;
        }
    }

    void OnTriggerExit2D(Collider2D c) {
        if (c.gameObject.CompareTag("Player"))
        {
            AudioManager am = AudioManager.instance;
            am.volume(0.78f);
            asrc.Stop();
            asrc.volume = 0f;
        }
    }
}
