using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class GenerateGround : MonoBehaviour
{
    public bool save = false;
    public Texture2D inputImage;
    public Vector2Int mapPointsCount;
    Mesh mesh;
    public float yScale = 1.0f;
    public float a = 1.0f;
    float h;


    private void Start()
    {
        if (save)
        {
            Vector3[] vertex = new Vector3[mapPointsCount.x * mapPointsCount.y];
            Vector2[] uvs = new Vector2[mapPointsCount.x * mapPointsCount.y];
            Vector2[] uvs2 = new Vector2[mapPointsCount.x * mapPointsCount.y];
            Vector3 currentPoint = Vector3.zero;
            Vector2 currentPointInUV = Vector2.zero;
            h = Mathf.Sqrt(3.0f) * a * 0.5f;
            float halfa = a * 0.5f;

            Vector2 black = Vector2.zero;
            Vector2 white = Vector2.one;


            for (int x = 0; x < mapPointsCount.x; x++)
            {
                for (int z = 0; z < mapPointsCount.y; z++)
                {
                    currentPoint.x = x * a + (z % 2) * halfa;
                    currentPoint.z = z * h;
                    currentPointInUV.x = currentPoint.x / ((float)mapPointsCount.x * a);
                    currentPointInUV.y = currentPoint.z / ((float)mapPointsCount.y * a);
                    int coordX = Mathf.RoundToInt(currentPointInUV.x * inputImage.width);
                    int coordY = Mathf.RoundToInt(currentPointInUV.y * inputImage.height);
                    currentPoint.y = inputImage.GetPixel(coordX, coordY).r * yScale;
                    vertex[x * mapPointsCount.y + z] = currentPoint;
                    uvs[x * mapPointsCount.y + z] = currentPointInUV;
                }
            }

            for (int x = 0; x < mapPointsCount.x; x++)
            {
                for (int z = 0; z < mapPointsCount.y; z++)
                {
                    //if ((x % 3 == 1 && z % 2 == 1) || (x % 3 == 0 && z % 2 == 0))
                    if ((x % 3 == 1 && z % 2 == 0) || (x % 3 == 2 && z % 2 == 1))
                    {
                        uvs2[x * mapPointsCount.y + z] = white;
                    }
                    else
                    {
                        uvs2[x * mapPointsCount.y + z] = black;
                    }
                }
            }


            int[] tris = new int[mapPointsCount.x * mapPointsCount.y * 6];
            for (int x = 0; x < mapPointsCount.x - 1; x++)
            {
                for (int z = 0; z < mapPointsCount.y - 2; z += 2)
                {
                    tris[(x * mapPointsCount.y + z) * 6 + 0] = x * mapPointsCount.y + z + 1;
                    tris[(x * mapPointsCount.y + z) * 6 + 1] = (x + 1) * mapPointsCount.y + z;
                    tris[(x * mapPointsCount.y + z) * 6 + 2] = x * mapPointsCount.y + z;

                    tris[(x * mapPointsCount.y + z) * 6 + 3] = x * mapPointsCount.y + z + 1;
                    tris[(x * mapPointsCount.y + z) * 6 + 4] = (x + 1) * mapPointsCount.y + z + 1;
                    tris[(x * mapPointsCount.y + z) * 6 + 5] = (x + 1) * mapPointsCount.y + z + 0;

                    tris[(x * mapPointsCount.y + z) * 6 + 6] = x * mapPointsCount.y + z + 2;
                    tris[(x * mapPointsCount.y + z) * 6 + 7] = (x + 1) * mapPointsCount.y + z + 2;
                    tris[(x * mapPointsCount.y + z) * 6 + 8] = x * mapPointsCount.y + z + 1;

                    tris[(x * mapPointsCount.y + z) * 6 + 9] = x * mapPointsCount.y + z + 1;
                    tris[(x * mapPointsCount.y + z) * 6 + 10] = (x + 1) * mapPointsCount.y + z + 2;
                    tris[(x * mapPointsCount.y + z) * 6 + 11] = (x + 1) * mapPointsCount.y + z + 1;
                }
            }


            mesh = new Mesh();
            mesh.vertices = vertex;
            mesh.triangles = tris;
            mesh.uv = uvs;
            mesh.uv3 = uvs2;
            mesh.RecalculateNormals();
            GetComponent<MeshFilter>().sharedMesh = mesh;


            UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/tmp.asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }

}
