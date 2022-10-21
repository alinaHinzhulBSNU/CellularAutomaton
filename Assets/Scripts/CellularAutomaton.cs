using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellularAutomaton : MonoBehaviour
{
    // Camera manager
    CameraManager cameraManager;

    // Manager UI
    ManagerUI managerUI;

    // Generation number
    int generation;

    // Colony metrics
    float minX;
    float maxX;

    float minY;
    float maxY;

    // One cell prefab
    [SerializeField] GameObject cellPrefab;

    // All alive cells
    Dictionary<Vector2, GameObject> cells;

    // Direction to neighbours
    List<Vector2> neighboursRelativePositions;

    // Cell states
    Dictionary<Vector2, bool> cellStates;


    //---------SINGLETON---------

    public static CellularAutomaton Instance;

    private void Awake()
    {
        Instance = this;
    }


    //---------EVENT FUNCTIONS---------

    void Start()
    {

        managerUI = ManagerUI.Instance;
        cameraManager = CameraManager.Instance;

        generation = 1;

        cells = new Dictionary<Vector2, GameObject>();
        cellStates = new Dictionary<Vector2, bool>();

        InitNeighbours();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeGeneration();
            managerUI.UpdateGenerationText();
            cameraManager.CameraInColonyCenter();
        }
    }


    //---------CHANGE GENERATION LOGIC---------

    void ChangeGeneration()
    {
        // All cells to check (including empty)
        CalcColonySize();

        // Set state
        for (float x = minX; x <= maxX; x++)
        {
            for (float y = minY; y <= maxY; y++)
            {
                Vector2 position = new Vector2(x, y);
                cellStates.Add(position, IsCellAlive(position));
            }
        }

        // Update real objects according to state
        foreach (KeyValuePair<Vector2, bool> state in cellStates)
        {
            bool isAlive = state.Value;
            Vector2 position = state.Key;

            if (isAlive)
            {
                InstantiateCell(position);
            }
            else
            {
                DestroyCell(position);
            }
        }

        cellStates.Clear();

        generation++;
    }


    //---------CELL FUNCTIONS---------

    void InitNeighbours()
    {
        neighboursRelativePositions = new List<Vector2>();

        // Top
        neighboursRelativePositions.Add(new Vector2(-1, 1));
        neighboursRelativePositions.Add(new Vector2(0, 1));
        neighboursRelativePositions.Add(new Vector2(1, 1));

        // Right
        neighboursRelativePositions.Add(new Vector2(1, 0));

        // Bottom
        neighboursRelativePositions.Add(new Vector2(-1, -1));
        neighboursRelativePositions.Add(new Vector2(0, -1));
        neighboursRelativePositions.Add(new Vector2(1, -1));

        // Left
        neighboursRelativePositions.Add(new Vector2(-1, 0));
    }

    bool IsCellAlive(Vector2 position)
    {
        // Quantity of neighbours
        int quantityOfNeighbours = 0;

        foreach (Vector2 neighbourPosition in neighboursRelativePositions)
        {
            if (CellExists(position + neighbourPosition))
            {
                quantityOfNeighbours++;
            }
        }

        // Existing cell still alive
        if (quantityOfNeighbours > 1 && quantityOfNeighbours < 4 && CellExists(position))
        {
            return true;
        }

        // Newborn cell
        if (quantityOfNeighbours == 3 && !CellExists(position))
        {
            return true;
        }

        return false;
    }

    public bool CellExists(Vector2 position)
    {
        if (cells.TryGetValue(position, out var oldCell))
        {
            return true;
        }

        return false;
    }

    public void InstantiateCell(Vector2 position)
    {
        if (!cells.TryGetValue(position, out var oldCell))
        {
            GameObject newCell = Instantiate(cellPrefab, position, cellPrefab.transform.rotation);
            cells.Add(position, newCell);
        }
    }

    public void DestroyCell(Vector2 position)
    {
        if (cells.TryGetValue(position, out var oldCell))
        {
            Destroy(oldCell);
            cells.Remove(position);
        }
    }


    //---------METRICS FUNCTIONS---------

    public Vector2 GetMaxPosition()
    {
        Vector2 maxKey = cells.Keys.First();

        foreach (Vector2 key in cells.Keys)
        {
            maxKey = Vector2.Max(key, maxKey);
        }

        return maxKey;
    }

    public Vector2 GetMinPosition()
    {
        Vector2 minKey = cells.Keys.First();

        foreach (Vector2 key in cells.Keys)
        {
            minKey = Vector2.Min(key, minKey);
        }

        return minKey;
    }

    void CalcColonySize()
    {
        Vector2 minPosition = GetMinPosition();
        Vector2 maxPosition = GetMaxPosition();

        minX = minPosition.x - 1;
        maxX = maxPosition.x + 1;

        minY = minPosition.y - 1;
        maxY = maxPosition.y + 1;
    }


    //---------PROPERTIES---------

    public int Generation
    {
        get
        {
            return generation;
        }
    }
}
