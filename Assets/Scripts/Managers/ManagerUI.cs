using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI generationText;
    CellularAutomaton cellularAutomaton;


    //---------SINGLETON---------

    public static ManagerUI Instance;

    void Awake()
    {
        Instance = this;
    }


    //---------EVENT FUNCTIONS---------

    void Start()
    {
        cellularAutomaton = CellularAutomaton.Instance;
        UpdateGenerationText();
    }


    //---------UPDATE UI---------

    public void UpdateGenerationText()
    {
        generationText.text = $"Generation: {cellularAutomaton.Generation}";
    }
}
