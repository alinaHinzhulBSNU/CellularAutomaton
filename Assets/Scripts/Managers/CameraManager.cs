using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GridManager gridManager;
    CellularAutomaton cellularAutomaton;

    Camera cam;


    //---------SINGLETON---------

    public static CameraManager Instance;

    void Awake()
    {
        Instance = this;
    }


    //---------EVENT FUNCTIONS---------

    void Start()
    {
        gridManager = GridManager.Instance;
        cellularAutomaton = CellularAutomaton.Instance;

        cam = gameObject.GetComponent<Camera>();
    }


    //---------SET CAMERA POSITION---------

    public void CameraInGridCenter()
    {
        // POSITION
        float cameraX = gridManager.Width / 2f - 0.5f;
        float cameraY = gridManager.Height / 2f - 0.5f;
        float cameraZ = gameObject.transform.position.z;

        transform.position = new Vector3(cameraX, cameraY, cameraZ);

        // ZOOM
        float size = gridManager.Height / 2f;
        
        cam.orthographicSize = size;
    }

    public void CameraInColonyCenter()
    {
        // COLONY SIZE
        Vector2 maxPosition = cellularAutomaton.GetMaxPosition();
        Vector2 minPosition = cellularAutomaton.GetMinPosition();

        float height = maxPosition.y - minPosition.y + 1;
        float width = maxPosition.x - minPosition.x + 1;

        // POSITION LOGIC
        Vector2 center = (minPosition + maxPosition) / 2;

        float cameraX = center.x;
        float cameraY = center.y;
        float cameraZ = gameObject.transform.position.z;

        transform.position = new Vector3(cameraX, cameraY, cameraZ);

        // ZOOM LOGIC

        // Camera can not see all the cells
        bool heightOverflow = height / 2f > cam.orthographicSize;
        bool widthOverflow = width > cam.orthographicSize * 2f * cam.aspect;

        // Camera see blank space
        bool heightLack = height / 2f < cam.orthographicSize;
        bool widthLack = width < cam.orthographicSize * 2f * cam.aspect;

        // New camera size
        float sizeBasedOnHeight = height / 2f;
        float sizeBasedOnWidth = width / (2f * cam.aspect);

        // Zoom
        if(heightLack && widthLack || heightOverflow || widthOverflow)
        {
            float size = Math.Max(sizeBasedOnHeight, sizeBasedOnWidth);
            cam.orthographicSize = size;
        }
    }
}
