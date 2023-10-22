using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AdjacentsData : MonoBehaviour
{
    public Tilemap Themap;

    public Vector3Int TheOnesVec3;
    public TileBase TheOne;
    public Tile TreeGrowthOne;

    private Vector3Int TheLeftVec3;
    private TileBase LeftOne;
  

    private Vector3Int TheRightVec3;
    private TileBase RightOne;

    private Vector3Int TheUpVec3; 
    private TileBase UpOne;

    private Vector3Int TheDownVec3;
    private TileBase DownOne;

    // Start is called before the first frame update
    void Start()
    {
        TheOnesVec3 = new Vector3Int(Random.Range(-9, 8), Random.Range(-4, 3), 0);
        TheOne = Themap.GetTile(TheOnesVec3);

        TheLeftVec3 = new Vector3Int(TheOnesVec3.x - 1, TheOnesVec3.y, 0);
        LeftOne = Themap.GetTile(TheLeftVec3);

        TheRightVec3 = new Vector3Int(TheOnesVec3.x + 1, TheOnesVec3.y, 0);
        RightOne = Themap.GetTile(TheRightVec3);

        TheUpVec3 = new Vector3Int(TheOnesVec3.x, TheOnesVec3.y + 1, 0);
        UpOne = Themap.GetTile(TheUpVec3);

        TheDownVec3 = new Vector3Int(TheOnesVec3.x, TheOnesVec3.y - 1, 0);
        DownOne = Themap.GetTile(TheDownVec3);

    }

    // Update is called once per frame
    void Update()
    {



    }
}
