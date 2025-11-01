using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPositionTracker : MonoBehaviour
{
    private Vector3 previousPosition;
     
    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // Compare current position with the last frame
        if (transform.localPosition != previousPosition)
        {
            // Log which script is modifying the position
            Debug.Log($"Position changed by: {GetStackTrace()}, New Position: {transform.localPosition}");
            previousPosition = transform.localPosition;
        }
    }

    // Method to get the stack trace of where the position was last modified
    private string GetStackTrace()
    {
        var stackTrace = new System.Diagnostics.StackTrace(true);
        var stackFrame = stackTrace.GetFrame(1); // Get the calling method's frame
        return stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name;
    }
}
