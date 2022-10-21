using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Help user to see where this tile is actually located
    [SerializeField] GameObject hover;

    // Colony manger
    CellularAutomaton cellularAutomaton;

    // Tile position
    int x;
    int y;


    //---------EVENT FUNCTIONS---------

    private void Start()
    {
        cellularAutomaton = CellularAutomaton.Instance;
    }

    private void OnMouseEnter() // highlight tile
    {
        hover.SetActive(true);
    }

    private void OnMouseExit() // remove highlight
    {
        hover.SetActive(false);
    }

    private void OnMouseDown() // add initial cells
    {
        Vector2 position = new Vector2(x, y);

        if (cellularAutomaton.CellExists(position))
        {
            cellularAutomaton.DestroyCell(position);
        }
        else
        {
            cellularAutomaton.InstantiateCell(position);
        }
    }


    //---------PROPERTIES---------

    public int X
    {
        get { return x; }
        set { x = value; }
    }

    public int Y
    {
        get { return y; }
        set { y = value; }
    }
}
