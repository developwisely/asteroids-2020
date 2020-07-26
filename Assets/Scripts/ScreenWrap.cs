using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    public float offset;
    private bool _active = false;

    private void Awake()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float cameraHeight = Camera.main.orthographicSize;
        //Debug.Log(cameraPosition);
        //Debug.Log(cameraHeight);
        //Debug.Log(Screen.width);
        //Debug.Log(Screen.height);
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(cameraPosition.x, 0, cameraHeight)));
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, cameraPosition.y)));
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(-cameraPosition.x, cameraPosition.y, -cameraPosition.z)));
        //Vector3 bottomLeftCameraPoint = Camera.main.ScreenToWorldPoint(new Vector3(cameraPosition.x, 0, cameraHeight));
        //Vector3 topRightCameraPoint = Camera.main.ScreenToWorldPoint(new Vector3(cameraPosition.x, 0, cameraPosition.z));
    }
    private void Update()
    {
        if (!_active) return;

        // Normalized position in world based on viewport (0.0 - 1.0)
        Vector3 currentPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Crossed into the wrap zone positive X-axis (Right)
        if (currentPosition.x > 1.0)
        {

        }

        // Crossed into the wrap zone negative X-axis (Left)
        if (currentPosition.x < 0.0)
        {

        }

        // Crossed into the wrap zone positive Z-axis (Top)
        if (currentPosition.z > 1.0)
        {

        }

        // Crossed into the wrap zone negative Z-axis (Bottom)
        if (currentPosition.z < 1.0)
        {

        }
    }

    public bool IsActive()
    {
        return _active;
    }

    public void EnableScreenWrap()
    {
        _active = true;
    }

    public void DisableScreenWrap()
    {
        _active = false;
    }
}
