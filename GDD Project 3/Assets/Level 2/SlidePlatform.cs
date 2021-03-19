using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePlatform : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] [Tooltip("The direction to move in from this position")]
    private Vector3 m_Displacement;
    
    [SerializeField] [Tooltip("The speed the platform should move at")]
    private float m_Speed;
    
    #endregion

    #region Private variables

    private Vector3 p_OriginalPos;
    private Vector3 p_FinalPos;
    private Direction p_Direction;
    
    #endregion

    #region Direction Enum

    private enum Direction
    {
        Forward=1,
        Backward=-1
    }
    
    #endregion

    void Start()
    {
        p_OriginalPos = this.transform.position;
        p_FinalPos = p_OriginalPos + m_Displacement;
        this.p_Direction = Direction.Forward;
    }

    void Update()
    {
        this.transform.position += m_Displacement * ((int)p_Direction * m_Speed * Time.deltaTime);
        if ((this.transform.position - p_FinalPos).sqrMagnitude < 0.02)
        {
            this.p_Direction = Direction.Backward;
        } else if ((this.transform.position - p_OriginalPos).sqrMagnitude < 0.02)
        {
            this.p_Direction = Direction.Forward;
        }
    }
}
