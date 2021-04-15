using UnityEngine;

public class GridContainer : MonoBehaviour
{
    //Meybe usefull to pathfind
    public static GridContainer instance;
    //Node[,,] grid;

    private void Awake()
    {
        instance = this;
    }
}
