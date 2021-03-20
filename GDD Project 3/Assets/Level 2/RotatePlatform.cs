using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RotatePlatform : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] [Tooltip("Degrees per second to rotate")] 
    private float m_DegreeSpeed;

    [SerializeField] [Tooltip("The Direction to go in")]
    private Direction m_Direction;
    
    #endregion

    #region Direction Enum

    [Serializable]
    enum Direction
    {
        Clockwise=-1,
        CounterClockwise=1
    }


    #endregion
    
    
    void Start()
    {
        this.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currEuler = this.transform.rotation.eulerAngles;
        Vector3 direction = (int) this.m_Direction * this.m_DegreeSpeed * Time.deltaTime * new Vector3(0, 0, 1);
        Vector3 newEuler = currEuler + direction;
        this.transform.rotation = Quaternion.Euler(newEuler);
    }
}
