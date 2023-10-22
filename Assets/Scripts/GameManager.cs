using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public int MainTickRateStates;
    public float MainTickRate;

    [SerializeField]
    private Tilemap map;

    public Vector3Int randomxyz, randomxyz2;

    public Tile TreeCentre, City;

    [SerializeField]
    private List<TileData> tileDatas;
    public List<Vector3Int> GrowthCentres;
    public List<Vector3Int> ToBeAddedToGrowthCentres;

    private Dictionary<TileBase, TileData> dataFromTiles;


    [SerializeField]

    public Tile TreeGrowthOne;

    private Vector3Int TheLeftVec3;
    private TileBase LeftOne;


    private Vector3Int TheRightVec3;
    private TileBase RightOne;

    private Vector3Int TheUpVec3;
    private TileBase UpOne;

    private Vector3Int TheDownVec3;
    private TileBase DownOne;


    public bool RunTreeGrowth = false;
    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData); 
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MainTickRate = 10;
        MainTickRateStates = 0;

        PlacingTheCentreTiles();
        PlacingCityTile();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (MainTickRateStates == 0)
        {
            MainTickRate -= Time.deltaTime;
        }

        if (MainTickRate <= 0)
        {
            MainTickRateStates = 1;
            RunTreeGrowth = true;
        }

        if (MainTickRateStates == 1)
        {

            MainTickRate = 10;

            MainTickRateStates = 0;

            
        }

        if (RunTreeGrowth == true)
        {
            TreeGrowth();
        }
        
    }

    public void PlacingTheCentreTiles()
    {
        randomxyz = new Vector3Int(Random.Range(-9, -2), Random.Range(-4, 3), 0);

        map.SetTile(randomxyz, TreeCentre);

      
        TileBase choseTile = map.GetTile(randomxyz);

        GrowthCentres.Add(randomxyz);

        //Debug.Log(choseTile + dataFromTiles[choseTile].tiles.ToString());

    }

    public void PlacingCityTile()
    {
        randomxyz2 = new Vector3Int(Random.Range(1, 8), Random.Range(-4, 3), 0);

        map.SetTile(randomxyz2, City);


        TileBase choseTile = map.GetTile(randomxyz2);


        //Debug.Log(choseTile + dataFromTiles[choseTile].tiles.ToString());
    }

    public void TreeGrowth()
    {
        foreach (Vector3Int position in GrowthCentres)
        {
            TileBase tile = map.GetTile(position);

            if (tile != null)
            {
                Debug.Log("Tile Position: " + position);
                //we take all the adjacent ones and change them to growth centres and then add them back
                TheLeftVec3 = new Vector3Int(position.x - 1, position.y, 0);
                LeftOne = map.GetTile(TheLeftVec3);
                

                if (GrowthCentres.Contains(TheLeftVec3) || dataFromTiles[LeftOne].BarrenWasteland == true)
                {
                    Debug.Log("Cant Assimilate this");
                }
                else
                {
                    ToBeAddedToGrowthCentres.Add(TheLeftVec3);
                    map.SetTile(TheLeftVec3, TreeGrowthOne);
                }

                TheRightVec3 = new Vector3Int(position.x + 1, position.y, 0);
                RightOne = map.GetTile(TheRightVec3);

                if (GrowthCentres.Contains(TheRightVec3) || dataFromTiles[RightOne].BarrenWasteland == true)
                {
                    Debug.Log("Cant Assimilate this");
                }
                else
                {
                    map.SetTile(TheRightVec3, TreeGrowthOne);
                    ToBeAddedToGrowthCentres.Add(TheRightVec3);
                }
                

                TheUpVec3 = new Vector3Int(position.x, position.y + 1, 0);
                UpOne = map.GetTile(TheUpVec3);

                if (GrowthCentres.Contains(TheUpVec3) || dataFromTiles[UpOne].BarrenWasteland == true)
                {
                    Debug.Log("Cant Assimilate this");
                }
                else
                {
                    map.SetTile(TheUpVec3, TreeGrowthOne);
                    ToBeAddedToGrowthCentres.Add(TheUpVec3);
                }
                

                TheDownVec3 = new Vector3Int(position.x, position.y - 1, 0);
                DownOne = map.GetTile(TheDownVec3);

                if (GrowthCentres.Contains(TheDownVec3) || dataFromTiles[DownOne].BarrenWasteland == true)
                {
                    Debug.Log("Cant Assimilate this");
                }
                else
                {
                    map.SetTile(TheDownVec3, TreeGrowthOne);
                    ToBeAddedToGrowthCentres.Add(TheDownVec3);
                }
                
            }
            

        }
        
        GrowthCentres.AddRange(ToBeAddedToGrowthCentres);
        SortOutList();
        ToBeAddedToGrowthCentres.Clear();
        RunTreeGrowth = false;
        
    }

    public void SortOutList()
    {

        GrowthCentres = GrowthCentres.Distinct().ToList();

        /*List<Vector3Int> uniqueGrowthCentres = new List<Vector3Int>();
        HashSet<Vector3Int> seenPositions = new HashSet<Vector3Int>();

        foreach (Vector3Int position in GrowthCentres)
        {
            if (seenPositions.Add(position))
            {
                uniqueGrowthCentres.Add(position);
            }
        } */
    }


}
