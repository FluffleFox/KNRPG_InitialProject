using UnityEngine;

public class GenerateGridFromPrefab : MonoBehaviour
{
    public Texture2D tex;
    public float yScale = 3;
    public Vector2Int mapSize;
    public GameObject node;
    public Vector2 offset;
    void Start()
    {
        float maxX = mapSize.x * offset.x;
        float maxY = mapSize.y * offset.y;
        for (int x=0;x<mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.x; y++)
            {
                float tmpx = (x + (y % 2) * 0.5f) * offset.x;
                float tmpz = y * offset.y;
                float tmpy = tex.GetPixel(Mathf.RoundToInt(tex.width * tmpx / maxX), Mathf.RoundToInt(tex.height * tmpz / maxY)).r;
                tmpy = Mathf.FloorToInt(tmpy * yScale);
                if (tmpy == 0) continue;
                GameObject go = (GameObject)Instantiate(node, new Vector3(tmpx, 0, tmpz), Quaternion.identity);
                go.transform.localScale = new Vector3(1, tmpy, 1);
                go.transform.parent = transform;
            }
        }
    }

}
