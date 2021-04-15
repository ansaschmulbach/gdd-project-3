using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePlatform : MonoBehaviour, IPlatform
{

    #region Inspector Variables

    [SerializeField] [Tooltip("The direction to move in from this position")]
    private Vector3 m_Displacement;
    
    [SerializeField] [Tooltip("The speed the platform should move at")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("Time before the platform begins moving")]
    private float offset;

    #endregion

    #region Private variables

    private Vector3 p_OriginalPos;
    private Vector3 p_FinalPos;
    private Vector3 vel;
    private Direction p_Direction;
    private float halfDistance;

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
        halfDistance = m_Displacement.magnitude / 2;
        vel = Vector3.zero;
    }

    void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
            return;
        }
        float smoothing = (Mathf.Abs((this.transform.position - p_FinalPos).magnitude - halfDistance)) / (halfDistance);
        vel = m_Displacement * ((int) p_Direction * (m_Speed - (0.88f * smoothing)) * Time.deltaTime);
        this.transform.position += vel;
        if ((this.transform.position - p_FinalPos).sqrMagnitude < 0.02 * m_Speed)
        {
            this.p_Direction = Direction.Backward;
        } else if ((this.transform.position - p_OriginalPos).sqrMagnitude < 0.02 * m_Speed)
        {
            this.p_Direction = Direction.Forward;
        }
    }

    public Vector3 velocity()
    {
        return vel;
    }
}
