using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using System.Text;


public class FindTooltipObjects : MonoBehaviour
{
    //REPLACE W EDITORWINDOW
    
    /*private List<GameObject> tooltipObjects = new List<GameObject>();

    [MenuItem("Tools/Find Tooltip Objects")]
    public static void ShowWindow()
    {
        GetWindow<FindTooltipObjects>("Find Tooltip Objects");
    }

    public static string CapitalizeAfterDash(string input)
    {
        
        input = input.Replace('-', ' ');

        
        StringBuilder result = new StringBuilder(input.Length);

        
        bool capitalizeNext = false;

        
        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];

            if (capitalizeNext && char.IsLetter(currentChar))
            {
                result.Append(char.ToUpper(currentChar)); // Capitalize the letter
                capitalizeNext = false; // Reset the flag
            }
            else
            {
                result.Append(currentChar);
            }

            // Set flag if the current character is a space (replaced dash)
            if (currentChar == ' ')
            {
                capitalizeNext = true;
            }
        }

        return result.ToString();
    }
    private Vector3 GetClosestPointOnSphere(Vector3 center, float radius, Vector3 point)
    {
        
        Vector3 direction = point - center;

        
        if (direction == Vector3.zero)
        {
            return center;
        }

        
        Vector3 normalizedDirection = direction.normalized;

        
        Vector3 closestPoint = center + normalizedDirection * radius;

        return closestPoint;
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Find Tooltip Objects"))
        {
            FindAllTooltipObjects();
        }

        
        if (tooltipObjects.Count > 0)
        {
            GUILayout.Label("Found Tooltip Objects:", EditorStyles.boldLabel);


            foreach (var obj in tooltipObjects)
            {
                
                Transform newchild = obj.transform.Find("Pivot/ContentParent/Label");
                RectTransform rectTransform = newchild.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(40f, 5f);
                Transform parentTransform = obj.transform.parent;
                Transform main = parentTransform.parent;
                CapsuleCollider capsule = main.gameObject.GetComponent<CapsuleCollider>();

                Transform pivotTransform = obj.transform.Find("Pivot");

                

                Transform anchorTransform = obj.transform.Find("Anchor");

                BoxCollider collide = parentTransform.gameObject.GetComponent<BoxCollider>();
                anchorTransform.gameObject.transform.position = parentTransform.TransformPoint(collide.center);

                Debug.Log(capsule.ClosestPointOnBounds(parentTransform.TransformPoint(collide.center)));

                pivotTransform.gameObject.transform.position = capsule.ClosestPointOnBounds(parentTransform.TransformPoint(collide.center));

                Vector3 closestPoint = capsule.ClosestPoint(anchorTransform.gameObject.transform.position);

                
                Debug.Log("Closest Point on CapsuleCollider: " + closestPoint);

                pivotTransform.gameObject.transform.position = closestPoint;

                pivotTransform.gameObject.transform.position = GetClosestPointOnSphere(new Vector3(-0.22f, -0.3f, 9.48f), 5.0f, anchorTransform.gameObject.transform.position);

                ToolTip script = obj.GetComponent<ToolTip>();

                script.ToolTipText = CapitalizeAfterDash(parentTransform.gameObject.name);

                //obj.SetActive(false);
                //obj.transform.localScale = Vector3.zero;
                
                
                if (obj.transform.GetChild(0).gameObject.GetComponent<ToolTipOverride>() == null)
                {
                    obj.transform.GetChild(0).gameObject.AddComponent<ToolTipOverride>();
                }
                ToolTipOverride[] overriders = obj.GetComponents<ToolTipOverride>();

                foreach (ToolTipOverride overrider in overriders)
                {
                    DestroyImmediate(overrider);
                }

            }
        }
        else
        {
            GUILayout.Label("No Tooltip Objects found.");
        }
    }

    private void FindAllTooltipObjects()
    {
        tooltipObjects.Clear(); // Clear previous results
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (var obj in allObjects)
        {
            if (obj.name.Contains("Tooltip"))
            {
                tooltipObjects.Add(obj);
            }
        }

        Debug.Log($"Found {tooltipObjects.Count} Tooltip objects.");
    }*/
}
