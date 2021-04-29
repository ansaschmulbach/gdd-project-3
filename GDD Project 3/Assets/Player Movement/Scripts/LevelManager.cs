using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    public GameObject[] checkpoints;
    private AudioManager am;

    /* TODO: Menu & level selection */

    /* Level progress */
    private int lastCheckpointIndex;
    public GameObject lastCheckpoint;
    [SerializeField]
    [Tooltip("Order in which players are used. Auto-filled; set the order on PlayerController.")]
    public GameObject[] playerOrder;

    public int lastPlayerIndex;
    public GameObject lastPlayer;

    [SerializeField]
    [Tooltip("Initial player for the current level")]
    public GameObject initialPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (instance == this) return;
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        am = AudioManager.instance;
        checkpoints = am.checkpoints;
        playerOrder = new GameObject[2];
        lastCheckpoint = null;
        lastCheckpointIndex = -1;
        lastPlayerIndex = 0;
        lastPlayer = initialPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLastCheckpoint(GameObject checkpoint)
    {
        lastCheckpoint = checkpoint;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] == checkpoint)
            {
                lastCheckpointIndex = i;
            }
        }
    }

    public void UpdateLastPlayer(GameObject player)
    {
        for (int i = 0; i < playerOrder.Length; i++)
        {
            if (playerOrder[i] == player)
            {
                lastPlayerIndex = i;
            }
        }
        lastPlayer = player;
    }

    public void MovePlayerToCheckpoint()
    {
        if (lastCheckpointIndex == -1)
        {
            return;
        }
        if (lastPlayerIndex == 1)
        {
            ColorManager.instance.SwitchColor();
        }
        lastPlayer = playerOrder[lastPlayerIndex];
        lastCheckpoint = checkpoints[lastCheckpointIndex];
        lastPlayer.transform.position = lastCheckpoint.transform.position + new Vector3(0, lastCheckpoint.transform.localPosition.y) / 2;
    }
}
