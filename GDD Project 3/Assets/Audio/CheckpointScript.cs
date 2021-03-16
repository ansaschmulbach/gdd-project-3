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

    // Start is called before the first frame update
    void Start()
    {
        audioManager = musicController.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            print("Checkpoint");
            // audioManager.muffle(3000f);
            audioManager.muffle(gameObject);
        }

    }
}
