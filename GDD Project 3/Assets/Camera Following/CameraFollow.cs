using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private float m_DampTime = 0.15f;
    [SerializeField] private Vector2 m_OffSet = new Vector2(0.5f, 0.8f);
    
    #endregion

    #region Cached Values
    
    [SerializeField] private Transform cr_Player;
    [SerializeField] private PlayerMovement cr_PM;
    private Camera cr_Camera;

    #endregion

    #region Private Variables

    private Vector3 p_Velocity = Vector3.zero;

    #endregion

    #region Instantiation Methods

    void Start()
    {
        UpdatePlayer();
        cr_Camera = GetComponent<Camera>();
    }

    #endregion

    #region Update Methods

    void Update()
    {
        if (!cr_PM.isActive)
        {
            UpdatePlayer();
        }
        if (cr_Player)
        {
            Vector3 point = cr_Camera.WorldToViewportPoint(cr_Player.position);
            Vector3 delta = cr_Player.position - cr_Camera.ViewportToWorldPoint(new Vector3(m_OffSet.x, m_OffSet.y, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref p_Velocity, m_DampTime);
        }
    }

    void UpdatePlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm.isActive)
            {
                cr_Player = player.transform;
                cr_PM = pm;
            }
        }
    }

    #endregion
}
