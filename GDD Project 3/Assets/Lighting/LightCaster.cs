using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCaster : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Object casting light rays. Must contain a BoxCollider2D.")]
    private GameObject lightRays;

    [SerializeField]
    [Tooltip("Objects to cast rays to")]
    private GameObject[] sceneObjects;

    private float offset = 0.1f;

    public struct angledVerts {

        public Vector3 vert;

        public float angle;

        public Vector2 uv;
    }

    private AudioManager audio;

    private BoxCollider2D coll;

    private BoxCollider2D[] sceneColliders;

    private int raycastsPerFrame;


    /* Updated every frame */
    private angledVerts[] angled;

    private Vector3[] verts;

    private Vector2[] uvs;

    private Vector3[] sceneVerts;

    private Mesh m;


    // Start is called before the first frame update
    void Start()
    {
        m = lightRays.GetComponent<MeshFilter>().mesh;
        // coll = lightRays.GetComponent<BoxCollider2D>();
        sceneColliders = new BoxCollider2D[sceneObjects.Length];
        audio = AudioManager.instance;
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            sceneColliders[i] = sceneObjects[i].GetComponent<BoxCollider2D>();
        }
        raycastsPerFrame = 4 * sceneObjects.Length;

        sceneVerts = new Vector3[raycastsPerFrame];
        angled = new angledVerts[sceneVerts.Length * 2];
        verts = new Vector3[(sceneVerts.Length * 2) + 1];
        uvs = new Vector2[(sceneVerts.Length * 2) + 1];
        Vector3 reposition = transform.position; //- (transform.localScale / 2);
        //reposition.Scale(transform.localScale);
        lightRays.transform.position -= reposition;
        lightRays.transform.position -= new Vector3(0, 0, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < raycastsPerFrame; i += 4)
        {
            Vector3[] corners = GetObjectCorners(sceneColliders[i >> 2]);
            sceneVerts[i] = corners[0];
            sceneVerts[i + 1] = corners[1];
            sceneVerts[i + 2] = corners[2];
            sceneVerts[i + 3] = corners[3];
        }

        verts[0] = transform.position; // lightRays.transform.localToWorldMatrix.MultiplyPoint3x4(transform.position);
        uvs[0] = new Vector2(verts[0].x, verts[0].y);

        int h = 0;

        float random = Random.value;
        float randomX = offset * (1f + random * audio.freqs[16] * audio.beatProgress);
        float randomY = offset * (1f + random * 25 * audio.freqs[16] * audio.beatProgress);

        //float randomX = offset;
        //float randomY = offset;

        Vector3 myLoc = verts[0];
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            Vector3[] mesh = GetObjectCorners(sceneObjects[i].GetComponent<BoxCollider2D>());
            for (int j = 0; j < mesh.Length; j++)
            {
                Vector3 off = new Vector3(randomX, randomY, 0);

                Vector2 offset1 = mesh[j] - myLoc - off;
                Vector2 offset2 = mesh[j] - myLoc + off;

                float angle1 = Mathf.Atan2(offset1.y, offset1.x);
                float angle2 = Mathf.Atan2(offset2.y, offset2.x);

                RaycastHit2D hit = Physics2D.Raycast(myLoc, offset1, 1000f);
                RaycastHit2D hit2 = Physics2D.Raycast(myLoc, offset2, 1000f);

                Debug.DrawLine(myLoc, hit.point, Color.green);
                Debug.DrawLine(myLoc, hit2.point, Color.white);

                angled[(h * 2)].vert = hit.point;
                angled[(h * 2)].angle = angle1;
                angled[(h * 2)].uv = new Vector2(angled[(h * 2)].vert.x, angled[(h * 2)].vert.y);

                angled[(h * 2) + 1].vert = hit2.point;
                angled[(h * 2) + 1].angle = angle2;
                angled[(h * 2) + 1].uv = new Vector2(angled[(h * 2) + 1].vert.x, angled[(h * 2) + 1].vert.y);

                h++;

            }
        }

        System.Array.Sort(angled, delegate (angledVerts one, angledVerts two)
        {
            return one.angle.CompareTo(two.angle);
        });

        for (int i = 0; i < angled.Length; i++)
        {
            verts[i + 1] = angled[i].vert;
            uvs[i + 1] = angled[i].uv;
        }

        Vector2 pointFive = (Vector2.one * 0.5f);

        for (int i = 0; i < angled.Length; i++)
        {
            uvs[i] += pointFive;
        }

        m.vertices = verts;
        m.uv = uvs;

        int[] triangles = new int[verts.Length * 3];

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = verts.Length - 1;

        for (int i = 3, j = 0; i < verts.Length * 3 - 3; i += 3, j += 1)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 2;
            triangles[i + 2] = j + 1;
        }

        m.triangles = triangles;


    }

    public static Vector3[] ConcatArrays(Vector3[] first, Vector3[] second)
    {
        Vector3[] concatted = new Vector3[first.Length + second.Length];

        System.Array.Copy(first, concatted, first.Length);
        System.Array.Copy(second, 0, concatted, first.Length, second.Length);

        return concatted;
    }

    Vector3[] GetObjectCorners(BoxCollider2D obj)
    {

        Vector3[] corners = new Vector3[4];

        float width = obj.size.x * obj.transform.localScale.x / 2;
        float height = obj.size.y * obj.transform.localScale.y / 2;

        corners[0] = obj.transform.position + new Vector3(+width, +height);

        corners[1] = obj.transform.position + new Vector3(-width, +height);

        corners[2] = obj.transform.position + new Vector3(+width, -height);

        corners[3] = obj.transform.position + new Vector3(-width, -height);

        /*for (int i = 0; i < 4; i++)
        {
            print(corners[i]);
        } */

        return corners;

    }

}
