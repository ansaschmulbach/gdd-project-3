using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{

    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        float xMov = Input.GetAxis("Horizontal");
        float yMov = Input.GetAxis("Vertical");
        this.transform.position += speed * new Vector3(xMov, yMov) * Time.deltaTime;
    }
}
