using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToColorManager : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Describe which layer of coloring to add me to")]
    private int layer;

    // Start is called before the first frame update
    void Start()
    {
        ColorManager c = ColorManager.instance;
        if (layer == 1)
        {
            c.layerOne.Add(gameObject);
            c.layerOne.RemoveAt(0);
        } else if (layer == 2)
        {
            c.layerTwo.Add(gameObject);
            c.layerTwo.RemoveAt(0);
        } else if (layer == 3)
        {
            c.layerThree.Add(gameObject);
            c.layerThree.RemoveAt(0);
        }
    }

}
