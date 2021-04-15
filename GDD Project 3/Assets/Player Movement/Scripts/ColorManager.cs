using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Layer one objects (static platforms)")]
    // public GameObject[] layerOne;
    public List<GameObject> layerOne;

    [SerializeField]
    [Tooltip("Layer two objects")]
    // public GameObject[] layerTwo;
    public List<GameObject> layerTwo;

    [SerializeField]
    [Tooltip("Layer three objects")]
    //public GameObject[] layerThree;
    public List<GameObject> layerThree;

    [SerializeField]
    [Tooltip("Layer one, two, and three treble colors")]
    private Color[] trebleColor;

    [SerializeField]
    [Tooltip("Layer one, two, and three bass colors")]
    private Color[] bassColor;

    [SerializeField]
    [Tooltip("Transition time for layer one")]
    private float transitionTime1;

    [SerializeField]
    [Tooltip("Transition time for layer two")]
    private float transitionTime2;

    [SerializeField]
    [Tooltip("Transition time for layer three")]
    private float transitionTime3;

    public static ColorManager instance = null;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator switchColor() {
        float elapsed = 0f;
        while (layerOne[0].GetComponent<SpriteRenderer>().color != bassColor[0]) {
            //for (int i = 0; i < layerOne.Length; i++)
            for (int i = 0; i < layerOne.Count; i++)
            {
                SpriteRenderer sr = layerOne[i].GetComponent<SpriteRenderer>();
                sr.color = Color.Lerp(trebleColor[0], bassColor[0], Mathf.Min(transitionTime3, elapsed / transitionTime1));
            }

            for (int i = 0; i < layerTwo.Count; i++)
            {
                SpriteRenderer sr = layerTwo[i].GetComponent<SpriteRenderer>();
                sr.color = Color.Lerp(trebleColor[1], bassColor[1], Mathf.Min(transitionTime3, elapsed / transitionTime2));
            }

            for (int i = 0; i < layerThree.Count; i++)
            {
                SpriteRenderer sr = layerThree[i].GetComponent<SpriteRenderer>();
                sr.color = Color.Lerp(trebleColor[2], bassColor[2], Mathf.Min(transitionTime3, elapsed / transitionTime3));
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void SwitchColor() {
        StartCoroutine(switchColor());
    }

}
