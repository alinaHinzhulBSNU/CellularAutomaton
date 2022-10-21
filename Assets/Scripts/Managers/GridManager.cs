using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour // grid is used for initial user input
{
    // Camera manger
    CameraManager cameraManager;

    // Input grid size
    [SerializeField] int height;
    [SerializeField] int width;

    // Prefab
    [SerializeField] GameObject tilePrefab;

    // All tiles
    List<GameObject> tiles = new List<GameObject>();


    //---------SINGLETON---------

    public static GridManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cameraManager = CameraManager.Instance;

        FillGrid();
        cameraManager.CameraInGridCenter();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && tiles.Count != 0)
        {
            ClearGrid();
        }
    }


    //---------GRID FUNCTIONS---------

    public void FillGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), tilePrefab.transform.rotation);
                tile.GetComponent<Tile>().X = x;
                tile.GetComponent<Tile>().Y = y;

                tiles.Add(tile);
            }
        }
    }

    public void ClearGrid()
    {
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }

        tiles.Clear();
    }


    //---------PROPERTIES---------

    public int Height
    {
        get
        {
            return height;
        }
    }

    public int Width
    {
        get
        {
            return width;
        }
    }
}
