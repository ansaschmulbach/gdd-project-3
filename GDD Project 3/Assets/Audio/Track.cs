using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Track : MonoBehaviour
{

    #region Editor Variables
    [SerializeField]
    [Tooltip("Track BPM.")]
    private int m_BPM;
    public int BPM
    {
        get
        {
            return m_BPM;
        }
    }

    [SerializeField]
    [Tooltip("Time before the first beat of the song.")]
    private float m_Offset;
    public float Offset
    {
        get
        {
            return m_Offset;
        }
    }

    [SerializeField]
    [Tooltip("Treble track for this song.")]
    private AudioClip m_TrebleMix;
    public AudioClip Treble
    {
        get
        {
            return m_TrebleMix;
        }
    }

    [SerializeField]
    [Tooltip("Bass track for this song.")]
    private AudioClip m_BassMix;
    public AudioClip Bass
    {
        get
        {
            return m_BassMix;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
