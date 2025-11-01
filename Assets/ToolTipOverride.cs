using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.ARSubsystems;
using static UnityEngine.GraphicsBuffer;

public class ToolTipOverride : MonoBehaviour
{
    public Vector3 toolTipPos;
    public bool change;
    public UnityEngine.Color color;
    private MixedRealityLineRenderer lineRend;
    private TextMeshPro text;
    public Vector3 prevPos = Vector3.zero;
    public Vector3 planePos;
    private GameObject plane;
    public Vector3 pivotPos;
    public Transform pivot;
    public Vector3 planeNormal;
    public Vector3 planePosition;
    public Vector3 plane2Normal;
    public Vector3 plane2Position;
    private GameObject seg;
    private Renderer segRender;
    public Material segMaterial;
    private Quaternion previousRotation;
    public Vector3 movement;
    public int frameSkip = 0;
    public bool optionChanged = false;
    public bool optionChanged2 = false;
    public GameObject positionManager;
    public GlobalVariables variables;
    public bool updateShader;
    public bool isView;
    public Vector3 moveReset;
    public Vector3 childToGrand;
    public Vector3 initialOffset;
    public string annot_name;
    public Quaternion currentRotation;
    public Quaternion prevRotation;
    public Quaternion deltaRotation;
    public Vector3 something;
    public Vector3 something2;
    private Material lineRendMat;
    //Transform grandparent1;
    // Start is called before the first frame update
    void Start()
    {
        lineRend = transform.parent.GetComponent<MixedRealityLineRenderer>();
        text = transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshPro>();
        plane = transform.parent.parent.parent.gameObject;
        seg = transform.parent.parent.gameObject;
        segRender = seg.GetComponent<Renderer>();
        pivot = transform.parent.GetChild(1);
        //Debug.Log(pivot.gameObject.name);
        lineRendMat = new Material(Shader.Find("Unlit/Color"));
        
        GameObject positionManager = GameObject.Find("PositionManager");
        variables = positionManager.GetComponent<GlobalVariables>();

        if (transform.parent.parent.parent.parent != null)
        {
            if (transform.parent.parent.parent.parent.name.Contains("View"))
            {
                isView = true;
                //grandparent1 = transform.parent.parent.parent.parent;
            }
        } else
        {
            isView = false;
        }
        if (transform.parent.parent.name.Contains("View")) {

            isView = true;



        } else
        {
            isView = false;
        }



        prevPos = plane.transform.position;
        previousRotation = plane.transform.rotation;
        //initialOffset = grandparent1.InverseTransformPoint(transform.position);



        // transform.parent.SetParent(grandparent1);

    }

    // Update is called once per frame
    void Update()
    {
        /*movement = plane.transform.localPosition - prevPos;
        movement = plane.transform.TransformPoint(movement);
        Debug.Log("Movement: " + movement);
        prevPos = plane.transform.localPosition;*/

        movement = plane.transform.position - prevPos;
        currentRotation = plane.transform.rotation;
        deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);
        //Debug.Log("World Movement: " + movement);
        // Still track local position to calculate relative movement


        if (!variables.isLabel && !optionChanged)
        {
            pivot.gameObject.SetActive(false);
            lineRend.enabled = false;
            optionChanged = true;
            optionChanged2 = false;
        }
        else if (variables.isLabel && !optionChanged2)
        {
            pivot.gameObject.SetActive(true);
            lineRend.enabled = true;
            optionChanged = false;
            optionChanged2 = true;
        }
        /*Vector3 movement = plane.transform.localPosition - prevPos;
        Debug.Log("Local Movement: " + movement);
        prevPos = plane.transform.localPosition;*/
    }

    private void LateUpdate()
    {
        if (change) {
            //Debug.Log("running");
            frameSkip++;
            if (frameSkip == 1)
            {
                if (!isView)
                {
                    transform.position = toolTipPos;
                    something = transform.localPosition;
                    pivot.position = pivotPos;
                    something2 = pivot.localPosition;
                }
                //transform.localPosition = toolTipPos;
                //initialOffset = grandparent1.InverseTransformPoint(transform.position);
            }
            if(toolTipPos != null || toolTipPos != Vector3.zero)
            {
                
                
                
                
                
                if (prevPos != Vector3.zero && frameSkip>5)
                {
                    if (!isView)
                    {
                        Debug.Log(movement);
                        //transform.localPosition = toolTipPos + movement;
                        //pivot.localPosition = pivotPos + movement;
                        /*Vector3 originalPosition = toolTipPos + movement;

                        // Apply the rotation change
                        Vector3 planeCenter = plane.transform.position;

                        // Calculate the direction from the object to the plane's center
                        Vector3 directionToPlaneCenter = originalPosition - planeCenter;

                        // Apply the plane's rotation change to the direction vector
                        directionToPlaneCenter = deltaRotation * directionToPlaneCenter;

                        // Update the object's position relative to the plane's center
                        transform.position = planeCenter + directionToPlaneCenter;*/
                        transform.localPosition = something;
                        pivot.localPosition = something2;
                        //transform.RotateAround(plane.transform.position, deltaRotation.eulerAngles, 20 * Time.deltaTime);
                        //transform.rotation = deltaRotation * transform.rotation;
                        //pivot.rotation = deltaRotation*pivot.rotation;


                        // Optionally, apply the same rotation to the object itself
                        //objectTransform.rotation = deltaRotation * objectTransform.rotation;
                        //Vector3 relativePosition = toolTipPos + movement - plane.transform.position;

                        // Apply the rotation change to the relative position (from the plane center)
                        //Vector3 rotatedRelativePosition = deltaRotation * relativePosition;

                        // Calculate the new position by adding the plane's position (center)
                        //transform.position = plane.transform.position + rotatedRelativePosition;

                        pivot.GetChild(0).GetChild(1).localScale = new Vector3(0.25f, 0.1f, 1.0f);
                    }

                    if (isView)
                    {
                        //transform.localPosition = toolTipPos + movement;
                        //pivot.localPosition = pivotPos + movement;
                        pivot.GetChild(0).GetChild(1).localScale = new Vector3(0.25f, 0.1f, 1.0f);
                        pivot.parent.localScale = new Vector3(2.15f, 2.15f, 2.15f);
                    }




                    if (movement.magnitude > 0.001)
                    {

                        Material newMat = new Material(segMaterial);
                        Quaternion currentRotation = plane.transform.rotation;

                        // Calculate the relative rotation between the current and previous rotations
                        Quaternion rotationChange = currentRotation * Quaternion.Inverse(previousRotation);

                        // Convert the quaternion rotation change to Euler angles


                        Debug.Log("Threshold Crossed");


                        /*newMat.SetColor("_Color", color);
                        newMat.SetColor("_CrossColor", color);

                        Vector3 newNormal1 = rotationChange * planeNormal;
                        Vector3 newNormal2 = rotationChange * plane2Normal;*/
                        /*newMat.SetVector("_Plane1Normal", new Vector4(newNormal1.x, newNormal1.y, newNormal1.z, 0.0f));
                        newMat.SetVector("_Plane1Position", new Vector4(plane2Position.x+ movement.x, plane2Position.y + movement.y, plane2Position.z + movement.z, 1.0f));
                        newMat.SetVector("_Plane2Normal", new Vector4(newNormal2.x, newNormal2.y, newNormal2.z, 0.0f));
                        newMat.SetVector("_Plane2Position", new Vector4(planePosition.x + movement.x, planePosition.y + movement.y, planePosition.z + movement.z, 1.0f));
                        newMat.SetVector("_Plane3Normal", new Vector4(newNormal2.x, newNormal2.y, newNormal2.z, 0.0f));
                        newMat.SetVector("_Plane3Position", new Vector4(planePosition.x + movement.x, planePosition.y + movement.y, planePosition.z + movement.z, 1.0f));*/

                        /*Vector3 localPlane2Position = plane.transform.InverseTransformPoint(plane2Position) + plane.transform.InverseTransformPoint(movement);
                        Vector3 localPlanePosition = plane.transform.InverseTransformPoint(planePosition) + plane.transform.InverseTransformPoint(movement);

                        // Convert back to world space
                        Vector3 worldPlane2Position = plane.transform.TransformPoint(localPlane2Position);
                        Vector3 worldPlanePosition = plane.transform.TransformPoint(localPlanePosition);*/

                        // Update material properties with world-space positions
                        /*newMat.SetVector("_Plane1Normal", new Vector4(newNormal1.x, newNormal1.y, newNormal1.z, 0.0f));
                        newMat.SetVector("_Plane1Position", new Vector4(worldPlane2Position.x, worldPlane2Position.y, worldPlane2Position.z, 1.0f));
                        newMat.SetVector("_Plane2Normal", new Vector4(newNormal2.x, newNormal2.y, newNormal2.z, 0.0f));
                        newMat.SetVector("_Plane2Position", new Vector4(worldPlanePosition.x, worldPlanePosition.y, worldPlanePosition.z, 1.0f));
                        newMat.SetVector("_Plane3Normal", new Vector4(newNormal2.x, newNormal2.y, newNormal2.z, 0.0f));
                        newMat.SetVector("_Plane3Position", new Vector4(worldPlanePosition.x, worldPlanePosition.y, worldPlanePosition.z, 1.0f));
                        segRender.material = newMat;*/
                    }
                        //Debug.Log("Running");
                    }
                
                previousRotation = plane.transform.rotation;
                //transform.localPosition = transform.parent.InverseTransformPoint(toolTipPos);


                // Override the position to match the dynamically updated world position
                //transform.position = dynamicWorldPosition;
                
                Gradient gradient = new Gradient();
                gradient.colorKeys = new GradientColorKey[]
                    {
                        new GradientColorKey(color, 1.0f), // Start color at 0%
                        new GradientColorKey(color, 1.0f)  // End color at 100%
                    };
                lineRend.LineColor = gradient;
                text.color = color;
                lineRend.LineMaterial = lineRendMat;
                lineRend.LineMaterial.color = color; // Change to any color





                //Debug.Log("Overriding");
            }
            
        }
    }
}
