using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(GenerateGround))]
public class OptionalGridMenager : MonoBehaviour
{
    Vector2Int mapPointsCount;
    Mesh mesh;
    float a = 1.0f;
    float h;
    Vector2[] uvs2;

    Vector2 white = Vector2.one;
    Vector2 red = Vector2.up;
    Vector2 blue = Vector3.right;
    int lastPoint;

    private void Start()
    {
        a = GetComponent<GenerateGround>().a;
        h = Mathf.Sqrt(3.0f) * a * 0.5f;
        mesh = GetComponent<MeshFilter>().mesh;
        mapPointsCount = GetComponent<GenerateGround>().mapPointsCount;
        uvs2 = mesh.uv3;
    }

    public int GetVertexIndex(Vector3 inputPoint)
    {
        float y = (inputPoint.z / h) % 2;
        float correction = 0;
        if (y < 1.0f)
        {
            correction = y * 0.5f * h;
        }
        else
        {
            correction = h - 0.5f * y * h;
        }
        float x = ((inputPoint.x - correction) / a) % 3;
        int currenty = -1;
        int currentx = -1;
        switch (Mathf.FloorToInt(x))
        {
            case 0:
                {
                    currenty = Mathf.FloorToInt((inputPoint.z + h) / (2.0f * h)) * 2;
                    currentx = Mathf.FloorToInt((inputPoint.x - correction) / a) + 1;
                    break;
                }
            case 1:
                {
                    if (y < 1.0f)
                    {
                        if (x < 2.0f - y)
                        {
                            currenty = Mathf.FloorToInt((inputPoint.z + h) / (2.0f * h)) * 2;
                            currentx = Mathf.FloorToInt((inputPoint.x - correction) / a);
                        }
                        else
                        {
                            currenty = 1 + Mathf.FloorToInt(inputPoint.z / (2.0f * h)) * 2;
                            currentx = Mathf.FloorToInt((inputPoint.x - correction) / a) + 1;
                        }
                    }
                    else
                    {
                        if (x > y)
                        {
                            currenty = 1 + Mathf.FloorToInt(inputPoint.z / (2.0f * h)) * 2;
                            currentx = Mathf.FloorToInt((inputPoint.x - correction) / a) + 1;
                        }
                        else
                        {
                            currenty = Mathf.FloorToInt((inputPoint.z + h) / (2.0f * h)) * 2;
                            currentx = Mathf.FloorToInt((inputPoint.x - correction) / a);
                        }
                    }
                    break;
                }
            case 2:
                {
                    currenty = 1 + Mathf.FloorToInt(inputPoint.z / (2.0f * h)) * 2;
                    currentx = Mathf.FloorToInt((inputPoint.x - correction) / a);
                    break;
                }
        }
        if (currentx < 0 || currenty < 0) return -1;
        if (currentx * mapPointsCount.x + currenty >= mapPointsCount.x * mapPointsCount.y) return -1;
        return currentx * mapPointsCount.x + currenty;
    }

    public void Follow(Vector3 inputPoint)
    {
        int newPoint = GetVertexIndex(inputPoint);
        if (lastPoint != newPoint)
        {
            if (lastPoint != -1)
            { uvs2[lastPoint] = white; }
            if (newPoint != -1)
            { uvs2[newPoint] = blue; }
            lastPoint = newPoint;
            mesh.uv4 = uvs2;
        }
    }

    public void AttackFollow(Vector3 inputPoint)
    {
        int newPoint = GetVertexIndex(inputPoint);
        if (lastPoint != newPoint)
        {
            if (lastPoint != -1)
            { uvs2[lastPoint] = white; }
            if (newPoint != -1)
            { uvs2[newPoint] = red; }
            lastPoint = newPoint;
            mesh.uv4 = uvs2;
        }
    }

    public int ConvertVertexIndexToNodeIndex(int vertexIndex) // Dopiszę jeśli będzie potrzeba + nie wiem jaki system koordynatów robić
    {
        return 0;
    }
}
