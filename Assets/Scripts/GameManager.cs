using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public int MainTickRateStates;
    public float MainTickRate;

    [SerializeField]
    private Tilemap map;

    public Vector3Int randomxyz, randomxyz2, randomxyz3, randomxyz4, randomxyz5, randomxyz6, randomxyz7;

    public Tile TreeCentre, City, Wasteland, TreeCut, Trees;

    [SerializeField]
    private List<TileData> tileDatas;
    public List<Vector3Int> GrowthCentres;
    public List<Vector3Int> ToBeAddedToGrowthCentres;
    public List<Vector3Int> TreeCutees;

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


    [SerializeField]

    public bool SelectedWasteland, SelectedTreeCutting, SelectedCity;

    public float WastelandsAvailable, TreeCutttingAvailable, SelecttedCityAvailable, TreeEarnings;

    public bool RunPrizepool = false;
    public float Earnings;

    public Text NumberOfTreeCutting, NumberOfWastelands, NumberOfCities, NumberOfTreeEarnings;

    public bool CheckToChangeCutsToTrees, CheckingList;

    public float internaltimer;
    public float yourtime;

    public Text InternalTimerText, TickRateText, YourTimeText;
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
    
    void Start()
    {
        MainTickRate = 5;
        MainTickRateStates = 0;

        WastelandsAvailable = 1f;
        TreeCutttingAvailable = 3f;
        SelecttedCityAvailable = 0f;

        internaltimer = 11f;

        SelectedWasteland = false;
        SelectedTreeCutting = false;
        SelectedCity = false;
        CheckToChangeCutsToTrees = false;
        CheckingList = false;

        yourtime = 0;

        PlacingTheCentreTiles();
        PlacingCityTile();
        PlacingWastelandTiles();
        
    }

   
    void Update()
    {
        yourtime += Time.deltaTime;

        if (MainTickRateStates == 0)
        {
            MainTickRate -= Time.deltaTime;
        }

        if (MainTickRate <= 0)
        {
            MainTickRateStates = 1;
            RunTreeGrowth = true;
            RunPrizepool = true;

            if (CheckingList == false)
            {
                CheckToChangeCutsToTrees = true;
            }
            
        }

        if (MainTickRateStates == 1)
        {

            MainTickRate = 5;

            MainTickRateStates = 0;

            
        }

        if (RunTreeGrowth == true)
        {
            TreeGrowth();
        }

        if (RunPrizepool == true)
        {
            DrawPrizefromPool();
        }

        if (CheckToChangeCutsToTrees == true && CheckingList == false)
        {
            internaltimer -= Time.deltaTime;

            if (internaltimer <= 0)
            {
                
                CheckingList = true;
                
            }


        }

        if (CheckToChangeCutsToTrees == true && CheckingList == true)
        {
            internaltimer = 11f;
            RunListChangeProgramForCuts();
            
        }

       /* if (internaltimer <= 0)
        {
            RunListChangeProgramForCuts();
            CheckToChangeCutsToTrees = false;
            CheckingList = false;
            internaltimer = 16f;
        } */

        if (TreeEarnings >= 15)
        {
            SelecttedCityAvailable = 1f;
            TreeEarnings = 0f;
        }

        NumberOfTreeCutting.text = TreeCutttingAvailable.ToString();
        NumberOfWastelands.text = WastelandsAvailable.ToString();
        NumberOfCities.text = SelecttedCityAvailable.ToString();
        NumberOfTreeEarnings.text = TreeEarnings.ToString();
        InternalTimerText.text = internaltimer.ToString("F0");
        TickRateText.text = MainTickRate.ToString("F0");
        YourTimeText.text = yourtime.ToString("F0");


        if (SelectedTreeCutting == true && SelectedWasteland == false && SelectedCity == false && TreeCutttingAvailable >= 1)
        {
            if(Input.GetMouseButtonUp(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.WorldToCell(mousePosition);

                TileBase clickedTile = map.GetTile(gridPosition);

                if (dataFromTiles[clickedTile].Tree == true)
                {
                    map.SetTile(gridPosition, TreeCut);
                    TreeCutttingAvailable -= 1f;
                    TreeEarnings += 2f;

                    TreeCutees.Add(gridPosition);

                    /*if (GrowthCentres.Contains(gridPosition))
                    {
                        GrowthCentres.Remove(gridPosition);
                    } */
                }
                else
                {
                    Debug.Log("Cant Cut this");
                }

                

            }
            
        }
        if (SelectedTreeCutting == false && SelectedWasteland == true && SelectedCity == false && WastelandsAvailable >= 1)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.WorldToCell(mousePosition);

                TileBase clickedTile = map.GetTile(gridPosition);

                if (dataFromTiles[clickedTile].Tree == true)
                {
                    map.SetTile(gridPosition, Wasteland);
                    WastelandsAvailable -= 1f;

                    if (TreeCutees.Contains(gridPosition))
                    {
                        TreeCutees.Remove(gridPosition);
                    }

                    if (GrowthCentres.Contains(gridPosition))
                    {
                        GrowthCentres.Remove(gridPosition);
                    }
                }
                else
                {
                    Debug.Log("Cant Destroy this");
                }

            }
        }
        else if (SelectedTreeCutting == false && SelectedWasteland == false && SelectedCity == true && SelecttedCityAvailable >= 1)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.WorldToCell(mousePosition);

                TileBase clickedTile = map.GetTile(gridPosition);

                if (dataFromTiles[clickedTile].Tree == true || dataFromTiles[clickedTile].TreeCut == true)
                {
                    map.SetTile(gridPosition, City);
                    SelecttedCityAvailable -= 1f;

                    if (TreeCutees.Contains(gridPosition))
                    {
                        TreeCutees.Remove(gridPosition);
                    }
                }
                else
                {
                    Debug.Log("Cant build this here");
                }

            }
        }



    }

    public void PlacingWastelandTiles()
    {
        randomxyz3 = new Vector3Int(Random.Range(-1, 1), Random.Range(-4, 3), 0);
        map.SetTile(randomxyz3, Wasteland);

        randomxyz4 = new Vector3Int(Random.Range(-2, 2), Random.Range(-4, 3), 0);
        map.SetTile(randomxyz4, Wasteland);

        randomxyz5 = new Vector3Int(Random.Range(-2, 2), Random.Range(-4, 3), 0);
        map.SetTile(randomxyz5, Wasteland);

        randomxyz6 = new Vector3Int(Random.Range(-2, 2), Random.Range(-4, 3), 0);
        map.SetTile(randomxyz6, Wasteland);

        //randomxyz7 = new Vector3Int(Random.Range(-2, 2), Random.Range(-4, 3), 0);
       // map.SetTile(randomxyz7, Wasteland);
    }

    public void PlacingTheCentreTiles()
    {
        randomxyz = new Vector3Int(Random.Range(-9, -3), Random.Range(-4, 3), 0);

        map.SetTile(randomxyz, TreeCentre);

      
        TileBase choseTile = map.GetTile(randomxyz);

        GrowthCentres.Add(randomxyz);

        

    }

    public void PlacingCityTile()
    {
        randomxyz2 = new Vector3Int(Random.Range(3, 8), Random.Range(-4, 3), 0);

        map.SetTile(randomxyz2, City);


        TileBase choseTile = map.GetTile(randomxyz2);


        
    }

    public void TreeGrowth()
    {
        foreach (Vector3Int position in GrowthCentres)
        {
            TileBase tile = map.GetTile(position);

            if (tile != null)
            {
                Debug.Log("Tile Position: " + position);
                
                TheLeftVec3 = new Vector3Int(position.x - 1, position.y, 0);
                LeftOne = map.GetTile(TheLeftVec3);
                

                if (GrowthCentres.Contains(TheLeftVec3) || dataFromTiles[LeftOne].BarrenWasteland == true || dataFromTiles[LeftOne].TreeCut == true)
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

                if (GrowthCentres.Contains(TheRightVec3) || dataFromTiles[RightOne].BarrenWasteland == true || dataFromTiles[RightOne].TreeCut == true)
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

                if (GrowthCentres.Contains(TheUpVec3) || dataFromTiles[UpOne].BarrenWasteland == true || dataFromTiles[UpOne].TreeCut == true)
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

                if (GrowthCentres.Contains(TheDownVec3) || dataFromTiles[DownOne].BarrenWasteland == true || dataFromTiles[DownOne].TreeCut == true)
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

    public void DrawPrizefromPool()
    {
        Earnings = Random.Range(1, 100);

        if (Earnings >= 26)
        {
            
            TreeCutttingAvailable += Random.Range(1, 4);
        }
        else if (Earnings <= 25)
        {
            WastelandsAvailable += 1;
            
        }

        RunPrizepool = false;
    }

    public void SelectTreeCutting()
    {
        SelectedWasteland = false;
        SelectedTreeCutting = true;
        SelectedCity = false;
    }

    public void SelectWasteland()
    {
        SelectedWasteland = true;
        SelectedTreeCutting = false;
        SelectedCity = false;
    }

    public void SelectCityBase()
    {
        SelectedWasteland = false;
        SelectedTreeCutting = false;
        SelectedCity = true;
    }

    public void ClearSelection()
    {
        SelectedWasteland = false;
        SelectedTreeCutting = false;
        SelectedCity = false;
    } 

    public void RunListChangeProgramForCuts()
    {
        CheckingList = true;

        foreach (Vector3Int position in TreeCutees)
        {
            TileBase baseposition = map.GetTile(position);

            if (dataFromTiles[baseposition].TreeCut == true)
            {
                map.SetTile(position, Trees);
                //GrowthCentres.Add(position);
            }
        }
       
        SortOutCutList();
        CheckingList = false;
    }

    public void SortOutCutList()
    {
        TreeCutees = TreeCutees.Distinct().ToList();

        
    }
}
