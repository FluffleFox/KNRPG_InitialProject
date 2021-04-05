using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class HighlightMap : MonoBehaviour
{
    public Tilemap highLightTileMap;
    public Tile    highLightTileAsset;

    private Vector3Int previousCellPos;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 5; // na razie kamera jest 5 jednostek na osi Z od tilemapy, ale trzeba by zrobic cos co sprawdza odleglosc kamery od mapy
        Vector3Int currentCellPos = highLightTileMap.WorldToCell(
            Camera.main.ScreenToWorldPoint(mousePos));
        if(currentCellPos != previousCellPos)
        {
            highLightTileMap.SetTile(currentCellPos, highLightTileAsset);
            highLightTileMap.SetTile(previousCellPos, null);
            previousCellPos = currentCellPos;
        }
    }
}
