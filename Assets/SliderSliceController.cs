using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class SliderSliceController : MonoBehaviour
{
    public float bottomPos;
    public float topPos;
    public float bottomSlice;
    public float topSlice;
    public float sliderValue;
    public GameObject slider;
    PinchSlider pinchSlider;
    public float prevVal;
    public int axis;
    public Material material;
    public GameObject view;
    public float bottomAnnot;
    public float topAnnot;
    public GameObject positionManager;
    public GlobalVariables variables;
    private Vector3 plane1Position;
    private Vector3 plane2Position;
    private Vector3 plane1Normal;
    private Vector3 plane2Normal;
    private Vector3 previousPosition;
    private Vector3 previousNormal;
    public Material crossSectionMat;
    public Vector3 position;
    public Vector3 normal;
    public bool prev2d;
    public bool prev3d;
    public bool prevNone;
    public bool prevSliceView = false;
    private float val;
    private bool firstIt = true;
    private Transform normal_segments;
    public Dictionary<GameObject, GameObject> parentChildMap;
    // Start is called before the first frame update
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
                result.Append(char.ToUpper(currentChar));
                capitalizeNext = false;
            }
            else
            {
                result.Append(currentChar);
            }


            if (currentChar == ' ')
            {
                capitalizeNext = true;
            }
        }

        return result.ToString();
    }
    void Start()
    {
        pinchSlider = slider.GetComponent<PinchSlider>();
        material = gameObject.GetComponent<Renderer>().sharedMaterial;
        variables = positionManager.GetComponent<GlobalVariables>();
        previousPosition = view.transform.position;
        previousNormal = view.transform.up;
        normal = view.transform.up;


        parentChildMap = new Dictionary<GameObject, GameObject>();

        normal_segments = view.transform.GetChild(1);
        foreach (Transform annotation in normal_segments)
        {
            parentChildMap[annotation.gameObject] = annotation.GetChild(0).gameObject;
            Debug.Log(annotation.name);
            Debug.Log(annotation.GetChild(0).name);
        }
        Debug.Log(parentChildMap.Count);





        if (gameObject.name == "Plane_sagittal")
        {
            bottomPos = .343f;
            topPos = -.332f;
            bottomSlice = .12f;
            topSlice = .87f;
            axis = 0;
            bottomAnnot = 3.65f;
            topAnnot = -3.68f;


        }

        if (gameObject.name == "Plane_axial")
        {
            bottomPos = .082f;
            topPos = .718f;
            bottomSlice = .87f;
            topSlice = .17f;
            axis = 1;
            bottomAnnot = 3.78f;
            topAnnot = -3.11f;

        }

        if (gameObject.name == "Plane_coronal")
        {
            bottomPos = -.659f;
            topPos = .144f;
            bottomSlice = .08f;
            topSlice = .95f;
            axis = 2;
            bottomAnnot = 5.18f;
            topAnnot = -3.58f;

        }
        foreach (Transform child in view.transform.GetChild(1))
        {

            UnityEngine.Color prevColor = child.GetComponent<Renderer>().material.color;
            Transform anchor = child.GetChild(0).GetChild(0);
            Transform pivot = child.GetChild(0).GetChild(1);
            pivot.GetChild(0).gameObject.AddComponent<ConstraintManager>();
            pivot.GetChild(0).gameObject.AddComponent<ObjectManipulator>();

            plane1Position = view.transform.position;
            Destroy(child.GetComponent<NearInteractionGrabbable>());
            Destroy(child.GetComponent<ObjectManipulator>());
            Destroy(child.GetComponent<ConstraintManager>());

            Vector3 normalizedNormal = child.InverseTransformDirection(normal).normalized;
            Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

            // Apply the rotation
            normalizedNormal = rotation * normalizedNormal;

            //Debug.Log("Normal: "+normalizedNormal);
            //Debug.Log("position:  "+plane1Position);



            Vector3 localPivotPos = pivot.localPosition;

            Vector3 localPointToPlane = localPivotPos - child.InverseTransformPoint(plane1Position);


            float localDistance = Vector3.Dot(localPointToPlane, normalizedNormal);
            Vector3 localClosestPoint = localPivotPos - normalizedNormal * localDistance;

            Vector3 localAnchorPos = anchor.localPosition;

            Vector3 localAnchorPlane = localAnchorPos - child.InverseTransformPoint(plane1Position);
            float localAnchorDistance = Vector3.Dot(localAnchorPlane, normalizedNormal);
            Vector3 localAnchorPoint = localAnchorPos - normalizedNormal * localAnchorDistance;


            //Vector3 prevVector1 = anchor.TransformPoint(localAnchorPoint);
            //Vector3 prevVector2 = pivot.TransformPoint(localClosestPoint);
           
            if (axis == 0) {
                localAnchorPoint = new Vector3(localAnchorPoint.x+1.4f, localAnchorPoint.y, localAnchorPoint.z);
                localClosestPoint = new Vector3(localClosestPoint.x+1.4f, localClosestPoint.y, localClosestPoint.z);
            }
            else if (axis == 1) {
                localAnchorPoint = new Vector3(localAnchorPoint.x, localAnchorPoint.y-1.9f, localAnchorPoint.z);
                localClosestPoint = new Vector3(localClosestPoint.x, localClosestPoint.y-1.9f, localClosestPoint.z);
            }
            else {
                localAnchorPoint = new Vector3(localAnchorPoint.x, localAnchorPoint.y, localAnchorPoint.z+1.75f);
                localClosestPoint = new Vector3(localClosestPoint.x, localClosestPoint.y, localClosestPoint.z + 1.75f);
            }
            anchor.localPosition = localAnchorPoint;
            pivot.localPosition = localClosestPoint;
            //pivot.GetChild(0).AddComponent<BoxCollider>();
            //pivot.GetChild(0).AddComponent<NearInteractionGrabbable>();
            //Vector3 originalLocalScale = new Vector3(0.4f, 0.4f, 0.4f);

            // Get the lossy scale of the current and new parents
            //Vector3 originalParentScale = anchor.parent.parent != null ? anchor.parent.parent.lossyScale : Vector3.one;
            //Vector3 newParentScale = view.transform != null ? view.transform.lossyScale : Vector3.one;

            // Calculate the new local scale
            //Vector3 newLocalScale = new Vector3(
            //originalLocalScale.x * originalParentScale.x / newParentScale.x,
            //originalLocalScale.y * originalParentScale.y / newParentScale.y,
            //originalLocalScale.z * originalParentScale.z / newParentScale.z
            //);
            //Debug.Log(newLocalScale);
            anchor.GetComponent<ToolTipOverride>().annot_name = anchor.parent.parent.name;
           

            Vector3 worldAnchorPoint = anchor.position;
            Vector3 worldClosestPoint = pivot.position;

            //Debug.Log("First: "+ anchor.localPosition);
            //Debug.Log(pivot.localPosition);
            ToolTip script = child.GetChild(0).gameObject.GetComponent<ToolTip>();
            script.ToolTipText = CapitalizeAfterDash(child.name);
            Debug.Log("parent scale: " + anchor.parent.lossyScale);
            Debug.Log("anchor scale: " + anchor.lossyScale);

            anchor.parent.SetParent(view.transform, true);


            Debug.Log("parent scale2: " + anchor.parent.lossyScale);
            Debug.Log("anchor scale2: " + anchor.lossyScale);
            //Debug.Log(anchor.localPosition);
            //Debug.Log(pivot.localPosition);

            //localAnchorPoint = anchor.localPosition;
            //localClosestPoint = pivot.localPosition;


            if (anchor.gameObject.GetComponent<ToolTipOverride>() != null)
            {
                ToolTipOverride overrider = anchor.gameObject.GetComponent<ToolTipOverride>();
                overrider.segMaterial = crossSectionMat;
                overrider.change = true;
                overrider.toolTipPos = localAnchorPoint;
                overrider.pivotPos = localClosestPoint;
                overrider.color = prevColor;
                overrider.planeNormal = plane1Normal;
                overrider.planePosition = plane1Position;
                overrider.updateShader = false;
                


                //Debug.Log("Start Override");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (variables.sliceView)
        {
            if (variables.sliceView && !prevSliceView)
            {
                foreach (Transform child in transform.parent)
                {
                    if (child.name.Contains("Pinch"))
                    {
                        child.gameObject.SetActive(true);
                    }
                    if (child.name.Contains("Plane"))
                    {

                        child.GetComponent<Renderer>().enabled = true;
                        child.GetChild(0).GetComponent<Renderer>().enabled = true;

                    }
                    if (child.name.Contains("View"))
                    {
                        child.GetComponent<Renderer>().enabled = true;
                        child.GetChild(0).GetComponent<Renderer>().enabled = true;

                        child.GetChild(1).gameObject.SetActive(true);
                    }
                    if(child.name.Contains("Tooltip"))
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            normal = view.transform.up;
            float angleDifference = Vector3.Angle(normal, previousNormal);
            if (((distanceMoved > .02f || angleDifference > 0.5f) && variables.Annot2d) || (variables.Annot2d == true && prev2d == false))
            {
                plane1Position = view.transform.position + normal * .02f;
                plane2Position = view.transform.position - normal * .02f;
                plane2Normal = view.transform.up;
                plane1Normal = -view.transform.up;
                previousPosition = view.transform.position;
                previousNormal = view.transform.up;

                foreach (Transform child in view.transform.GetChild(1))
                {

                    child.gameObject.GetComponent<Renderer>().enabled = true;
                    Renderer renderer = child.GetComponent<Renderer>();

                    Color color = renderer.material.color;


                    Material newMat = new Material(crossSectionMat);
                    newMat.SetColor("_Color", color);
                    newMat.SetColor("_CrossColor", color);


                    newMat.SetVector("_Plane1Normal", new Vector4(plane1Normal.x, plane1Normal.y, plane1Normal.z, 0.0f));
                    newMat.SetVector("_Plane1Position", new Vector4(plane1Position.x, plane1Position.y, plane1Position.z, 1.0f));
                    newMat.SetVector("_Plane2Normal", new Vector4(plane2Normal.x, plane2Normal.y, plane2Normal.z, 0.0f));
                    newMat.SetVector("_Plane2Position", new Vector4(plane2Position.x, plane2Position.y, plane2Position.z, 1.0f));
                    newMat.SetVector("_Plane3Normal", new Vector4(plane2Normal.x, plane2Normal.y, plane2Normal.z, 0.0f));
                    newMat.SetVector("_Plane3Position", new Vector4(plane2Position.x, plane2Position.y, plane2Position.z, 1.0f));
                    renderer.material = newMat;
                }


            }
            if (variables.Annot3d == true && prev3d == false)
            {
                foreach (Transform child in view.transform.GetChild(1))
                {

                    Color color = child.GetComponent<Renderer>().material.color;
                    Material newMaterial = new Material(Shader.Find("Standard"));
                    newMaterial.SetFloat("_Mode", 3);
                    newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    newMaterial.SetInt("_ZWrite", 0);
                    newMaterial.DisableKeyword("_ALPHATEST_ON");
                    newMaterial.DisableKeyword("_ALPHABLEND_ON");
                    newMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    newMaterial.EnableKeyword("_EMISSION");
                    newMaterial.renderQueue = 3000;

                    newMaterial.SetColor("_EmissionColor", color * 1.0f);

                    color.a = 0.5f;
                    newMaterial.color = color;
                    child.gameObject.GetComponent<Renderer>().material = newMaterial;

                }

            }
            if (!variables.Annot3d && !variables.Annot2d && prevNone == false)
            {


                foreach (Transform child in view.transform.GetChild(1))
                {
                    child.GetComponent<Renderer>().enabled = false;

                }
            }

            if (pinchSlider.SliderValue != prevVal)
            {
                //Debug.Log("Slider Value Change");
                val = pinchSlider.SliderValue;
                if (axis==1)
                {
                    material.SetFloat("_SliceIndex", Mathf.Lerp(bottomSlice, topSlice, val));
                } else
                {
                    material.SetFloat("_SliceIndex", Mathf.Lerp(bottomSlice, topSlice, val));
                }
                

                if (axis == 0)
                {
                    transform.localPosition = new Vector3(Mathf.Lerp(bottomPos, topPos, val), transform.localPosition.y, transform.localPosition.z);
                }
                if (axis == 1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(bottomPos, topPos, val), transform.localPosition.z);
                }
                if (axis == 2)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(bottomPos, topPos, val));
                }
                /*
                Vector3 moveReset = new Vector3(0, Mathf.Lerp(bottomAnnot, topAnnot, val) - view.transform.localPosition.y, 0);
                Vector3 inverseMovement = -moveReset;



                foreach (Transform child in view.transform.GetChild(1))
                {
                    child.GetChild(0).GetChild(0).localPosition += inverseMovement;


                }*/
                view.transform.GetChild(1).localPosition = new Vector3(view.transform.GetChild(1).localPosition.x, Mathf.Lerp(bottomAnnot, topAnnot, val), view.transform.GetChild(1).localPosition.z);




                if (variables.Annot2d || variables.Annot3d)
                {
                    BoxCollider boxCollider = view.GetComponent<BoxCollider>();
                    Vector3 center = view.transform.TransformPoint(boxCollider.center);
                    Vector3 halfExtents = boxCollider.size / 2;
                    Collider[] intersections = Physics.OverlapBox(center, halfExtents, view.transform.rotation);
                    var intersectingColliders = new HashSet<Collider>(intersections);
                    foreach (Transform child in view.transform.GetChild(1))
                    {





                        Collider childCollider = child.GetComponent<Collider>();
                        if (childCollider != null && intersectingColliders.Contains(childCollider))
                        {

                            child.gameObject.GetComponent<Renderer>().enabled = true;

                            parentChildMap[child.gameObject].SetActive(true);
                            parentChildMap[child.gameObject].GetComponent<LineRenderer>().enabled = true;
                            parentChildMap[child.gameObject].transform.GetChild(0).GetComponent<ToolTipOverride>().updateShader = true;
                            //child.GetChild(0).GetComponent<LineRenderer>().enabled = true;
                            //child.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
                            //child.GetChild(0).GetChild(0).GetComponent<ToolTipOverride>().updateShader = true;
                            //child.GetChild(0).gameObject.SetActive(true);

                        }
                        else
                        {

                            child.gameObject.GetComponent<Renderer>().enabled = false;

                            parentChildMap[child.gameObject].GetComponent<LineRenderer>().enabled = false;
                            parentChildMap[child.gameObject].transform.GetChild(0).GetComponent<ToolTipOverride>().updateShader = false;
                            parentChildMap[child.gameObject].SetActive(false);
                            //child.GetChild(0).GetChild(0).GetComponent<ToolTipOverride>().updateShader = false;
                            //child.GetChild(0).gameObject.SetActive(false);
                            //child.GetChild(0).GetComponent<LineRenderer>().enabled = true;
                            //child.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
                        }

                    }
                }




            }
            prevVal = val;
            prev2d = variables.Annot2d;
            prev3d = variables.Annot3d;
            prevNone = !prev2d && !prev3d;

        } else if ((!variables.sliceView && prevSliceView) || firstIt)
        {
            foreach (Transform child in transform.parent)
            {
                if (child.name.Contains("Pinch"))
                {
                    child.gameObject.SetActive(false);
                }
                if (child.name.Contains("Plane")) { 
                
                    child.GetComponent<Renderer>().enabled = false;
                    child.GetChild(0).GetComponent<Renderer>().enabled = false;

                }
                if(child.name.Contains("View"))
                {
                    child.GetComponent<Renderer>().enabled = false;
                    child.GetChild(0).GetComponent<Renderer>().enabled = false;

                    child.GetChild(1).gameObject.SetActive(false);

                    foreach (Transform child2 in child)
                    {
                        if (child2.name.Contains("Tooltip"))
                        {
                            //Debug.Log(child2.name);
                            child2.gameObject.SetActive(false);
                        }
                    }
                }
                
            }
        }
        firstIt = false;
        prevSliceView = variables.sliceView;

    }
   
}
