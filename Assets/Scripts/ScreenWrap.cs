using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    public float offset;

    private bool _active = false;
    private bool _isWrappingX = false;
    private bool _isWrappingZ = false;

    private Renderer[] renderers;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
    private void Update()
    {
        //if (!_active) return;

        if (IsVisible())
        {
            _isWrappingX = false;
            _isWrappingZ = false;
            return;
        }

        if (_isWrappingX && _isWrappingZ)
        {
            return;
        }

        Wrap();
    }

    // TODO: Something is wrong with this script as it's not immediately wrapping - watch asteroids carefully to see
    private void Wrap()
    {
        // Normalized position in world based on viewport (0.0 - 1.0)
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 newPosition = transform.position;

        // Wrap X-axis (Left/Right)
        if (!_isWrappingX && (viewportPosition.x > 1.0 || viewportPosition.x < 0.0))
        {
            newPosition.x = -newPosition.x;
            _isWrappingX = true;
        }

        // Wrap Z-axis (Top/Bottom)
        else if (!_isWrappingZ && (viewportPosition.y > 1.0 || viewportPosition.y < 0.0))
        {
            newPosition.z = -newPosition.z;
            _isWrappingZ = true;
        }

        transform.position = newPosition;
    }

    private bool IsVisible()
    {
        foreach (var renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
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
