using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private void Update()
    {
        // So unity allows enable/disable (dumb)
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        ScreenBounds bounds = other.GetComponent<ScreenBounds>();
        if (bounds == null)
        {
            return;
        }

        Wrap(bounds);
    }

    private void Wrap(ScreenBounds bounds)
    {
        Vector3 relativeLocation = bounds.transform.InverseTransformPoint(transform.position);

        if (Mathf.Abs(relativeLocation.x) > 0.5f)
        {
            relativeLocation.x *= -1;
        }

        if (Mathf.Abs(relativeLocation.y) > 0.5f)
        {
            relativeLocation.y *= -1;
        }

        transform.position = bounds.transform.TransformPoint(relativeLocation);
    }
}
