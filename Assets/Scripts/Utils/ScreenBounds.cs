using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ScreenBounds : MonoBehaviour
{
    static private ScreenBounds S; // Private but unprotected singleton

    public float zScale = 10;

    private Camera _cam;
    private BoxCollider _collider;
    private float _cachedOrthographicSize, _cachedAspect;
    private Vector3 _cachedCamScale;

    private void Start()
    {
        _cam = GetComponentInParent<Camera>();

        if (!_cam.orthographic)
        {
            Debug.LogError("Camera.main needs to be orthographic.");
        }

        _collider = GetComponent<BoxCollider>();
        _collider.size = Vector3.one;

        transform.position = Vector3.zero;
        ScaleSelf();
    }

    private void Update()
    {
        ScaleSelf();
    }

    private void ScaleSelf()
    {
        if (_cam.orthographicSize != _cachedOrthographicSize
            || _cam.aspect != _cachedAspect
            || _cam.transform.localScale != _cachedCamScale)
        {
            transform.localScale = CalculateScale();
        }
    }

    private Vector3 CalculateScale()
    {
        _cachedOrthographicSize = _cam.orthographicSize;
        _cachedAspect = _cam.aspect;
        _cachedCamScale = _cam.transform.localScale;

        Vector3 scaledDesired, scaleCollider;

        scaledDesired.z = zScale;
        scaledDesired.y = _cam.orthographicSize * 2;
        scaledDesired.x = scaledDesired.y * _cam.aspect;

        scaleCollider = scaledDesired.ComponentDivide(_cachedCamScale);

        return scaleCollider;
    }

    // Gets a random on screen point
    static public Vector3 RANDOM_ON_SCREEN_LOC
    {
        get
        {
            Vector3 min = S._collider.bounds.min;
            Vector3 max = S._collider.bounds.max;
            Vector3 loc = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
            return loc;
        }
    }

    // Returns the bounds of the box collider
    static public Bounds BOUNDS
    {
        get
        {
            if (S == null)
            {
                return new Bounds();
            }

            if (S._collider == null)
            {
                return new Bounds();
            }

            return S._collider.bounds;
        }
    }

    static public bool OOB(Vector3 worldPosition)
    {
        Vector3 localPosition = S.transform.InverseTransformPoint(worldPosition);
        float maxDist = Mathf.Max(Mathf.Abs(localPosition.x), Mathf.Abs(localPosition.y), Mathf.Abs(localPosition.z));
        return (maxDist > 0.5f);
    }
    static public int OOB_X(Vector3 worldPosition)
    {
        Vector3 localPosition = S.transform.InverseTransformPoint(worldPosition);
        return OOB_(localPosition.x);
    }
    static public int OOB_Y(Vector3 worldPosition)
    {
        Vector3 localPosition = S.transform.InverseTransformPoint(worldPosition);
        return OOB_(localPosition.y);
    }
    static public int OOB_Z(Vector3 worldPosition)
    {
        Vector3 localPosition = S.transform.InverseTransformPoint(worldPosition);
        return OOB_(localPosition.z);
    }
    static private int OOB_(float num)
    {
        if (num > 0.5f) return 1;
        if (num < 0.5f) return -1;
        return 0;
    }

}
