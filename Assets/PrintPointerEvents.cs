using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
//using VolumeRendering;


namespace VolumeRendering
{
    public class PrintPointerEvents : MonoBehaviour, IMixedRealityPointerHandler
    {
        [SerializeField] protected VolumeRendering volume;
        /*private BoxCollider BoxCollider;
        private ObjectManipulator ObjectManipulator;
        private NearInteractionGrabbable NearInteractionGrabbable;*/
        public GameObject volume_obj;
        string objName;
        float min;
        float max;
        float norm;
        Vector3 oldPos;
        float newPos;
        public Transform parent;
        private bool isX;
        private bool isY;
        private bool isZ;
        private Vector3 initialOffset;
        public GameObject mriPlane;
        public Renderer mriPlane_render;
        public Material slicematerial;
        //public bool is3d = false;
        //public bool Annot3d = false;
        //public bool Annot2d = true;
        GameObject clonePlane;
        //Plane clonePlaneData;
        public Material crossSectionMat;
        public Plane CrossSectionPlane;
        public float crossSectionSize = .05f;
        public GameObject spherePrefab;
        //public bool isLabel;
        public GameObject positionManager;
        public GlobalVariables variables;



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
            variables = positionManager.GetComponent<GlobalVariables>();
            
            objName = gameObject.name;
            Debug.Log(objName);
            if (objName == "axial_top")
            {
                min = 5;
                max = 188;
            }
            if (objName == "axial_bottom")
            {
                min = 5;
                max = 188;
            }
            if (objName == "sagittal_left")
            {
                min = -41;
                max = 139;
            }
            if (objName == "sagittal_right")
            {
                min = -41;
                max = 139;
            }
            if (objName == "coronal_front")
            {

                min = -124;
                max = 58;
            }
            if (objName == "coronal_back")
            {
                min = -124;
                max = 58;
            }
        }
        
        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            oldPos = eventData.Pointer.Position;
            if (eventData.Pointer is SpherePointer)
            {
                Vector3 pos = parent.InverseTransformPoint(eventData.Pointer.Position);
                if (variables.is3d)
                {
                    GameObject clone = Instantiate(volume_obj, parent);
                    clone.SetActive(true);
                    volume = clone.GetComponent<VolumeRendering>();
                    clone.GetComponent<BoxCollider>().enabled = true;
                    clone.GetComponent<ObjectManipulator>().enabled = true;
                    clone.GetComponent<NearInteractionGrabbable>().enabled = true;
                    //volume_obj.SetActive(true);
                    


                    if (objName == "axial_top")
                    {
                        norm = (pos.z - min) / (max - min);
                        volume.sliceZMin = Mathf.Max(norm - 0.05f, 0f);
                        volume.sliceZMax = Mathf.Min(norm + 0.05f, 1f);
                        
                        isY = true;

                    }
                    else if (objName == "axial_bottom")
                    {
                        norm = (pos.z - min) / (max - min);
                        volume.sliceZMin = Mathf.Max(norm - 0.05f, 0f);
                        volume.sliceZMax = Mathf.Min(norm + 0.05f, 1f);
                        
                        isY = true;
                    }
                    else if (objName == "sagittal_left")
                    {
                        norm = 1 - (pos.y - min) / (max - min);
                        volume.sliceYMin = Mathf.Max(norm - 0.05f, 0f);
                        volume.sliceYMax = Mathf.Min(norm + 0.05f, 1f);
                        
                        isX = true;
                    }
                    else if (objName == "sagittal_right")
                    {
                        norm = 1 - (pos.y - min) / (max - min);
                        volume.sliceYMin = Mathf.Max(norm - 0.05f, 0f);
                        volume.sliceYMax = Mathf.Min(norm + 0.05f, 1f);
                        
                        isX = true;
                    }
                    else if (objName == "coronal_front")
                    {
                        norm = (pos.x - min) / (max - min);
                        volume.sliceXMin = Mathf.Max(norm - 0.05f, 0f);
                        volume.sliceXMax = Mathf.Min(norm + 0.05f, 1f);
                       
                        isZ = true;
                    }
                    else if (objName == "coronal_back")
                    {
                        norm = (pos.x - min) / (max - min);
                        volume.sliceXMin = Mathf.Max(norm - 0.05f, 0f);
                        volume.sliceXMax = Mathf.Min(norm + 0.05f, 1f);
                        
                        isZ = true;
                    }

                    Debug.Log(norm);


                    initialOffset = volume.transform.position - eventData.Pointer.Position;


                    clone.transform.position = volume_obj.transform.position;

                    Debug.Log($"Grab start from {eventData.Pointer.PointerName}");
                } else
                {
                    GameObject clone = Instantiate(mriPlane, parent);
                    clonePlane = clone;
                    mriPlane_render = clone.GetComponent<Renderer>();
                    slicematerial = mriPlane_render.material;
                    clone.SetActive(true);
                    if (objName == "axial_top")
                    {
                        norm = (pos.z - min) / (max - min);
                        slicematerial.SetInt("_Axis", 0);
                        slicematerial.SetFloat("_SliceIndex", Mathf.Min(norm, 1f));
                        clone.transform.rotation = Quaternion.Euler(-90f, 180f, 0f);
                        clone.transform.position = new Vector3(volume_obj.transform.position.x, volume_obj.transform.position.y, eventData.Pointer.Position.z);
                        isY = true;

                    }
                    else if (objName == "axial_bottom")
                    {
                        norm = (pos.z - min) / (max - min);
                        slicematerial.SetInt("_Axis", 0);
                        slicematerial.SetFloat("_SliceIndex", Mathf.Min(norm, 1f));
                        clone.transform.rotation = Quaternion.Euler(-90f, 180f, 0f);
                        clone.transform.position = new Vector3(volume_obj.transform.position.x, volume_obj.transform.position.y, eventData.Pointer.Position.z);
                        isY = true;
                    }
                    else if (objName == "sagittal_left")
                    {
                        norm = 1 - (pos.y - min) / (max - min);
                        slicematerial.SetInt("_Axis", 1);
                        slicematerial.SetFloat("_SliceIndex", Mathf.Min(norm, 1f));
                        clone.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        clone.transform.position = new Vector3(volume_obj.transform.position.x, eventData.Pointer.Position.y, volume_obj.transform.position.z);
                        isX = true;
                    }
                    else if (objName == "sagittal_right")
                    {
                        norm = 1 - (pos.y - min) / (max - min);
                        slicematerial.SetInt("_Axis", 1);
                        slicematerial.SetFloat("_SliceIndex", Mathf.Min(norm, 1f));
                        clone.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        clone.transform.position = new Vector3(volume_obj.transform.position.x, eventData.Pointer.Position.y, volume_obj.transform.position.z);
                        isX = true;
                    }
                    else if (objName == "coronal_front")
                    {
                        norm = (pos.x - min) / (max - min);
                        slicematerial.SetInt("_Axis", 2);
                        slicematerial.SetFloat("_SliceIndex", Mathf.Min(norm, 1f));
                        clone.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        clone.transform.position = new Vector3(eventData.Pointer.Position.x, volume_obj.transform.position.y, volume_obj.transform.position.z);
                        isZ = true;
                    }
                    else if (objName == "coronal_back")
                    {
                        norm = (pos.x - min) / (max - min);
                        slicematerial.SetInt("_Axis", 2);
                        slicematerial.SetFloat("_SliceIndex", Mathf.Min(norm, 1f));
                        clone.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        clone.transform.position = new Vector3(eventData.Pointer.Position.x, volume_obj.transform.position.y, volume_obj.transform.position.z);
                        isZ = true;
                    }


                    //clone.transform.position = eventData.Pointer.Position;

                    //ADDING ANNOTATIONS
                    BoxCollider boxCollider = clone.GetComponent<BoxCollider>();
                    Vector3 center = clone.transform.TransformPoint(boxCollider.center);
                    Vector3 halfExtents = boxCollider.size / 2;
                    Collider[] intersections = Physics.OverlapBox(center , halfExtents, clone.transform.rotation);

                    Vector3 planeNormal = clone.transform.up;
                    //Vector3 planeNormal = clone.transform.rotation.eulerAngles;
                    Vector3 plane2Normal = clone.transform.up * -1; 
                    //Vector3 planePosition = clone.transform.position +planeNormal * crossSectionSize/4;
                    Vector3 planePosition = clone.transform.position;
                    Vector3 plane2Position = clone.transform.position + plane2Normal * crossSectionSize/4;
                    //GameObject newSphere = Instantiate(spherePrefab, planePosition, Quaternion.identity);
                    Debug.Log(planeNormal);
                        

                    GameObject cloneBackface = Instantiate(clone);
                    cloneBackface.transform.SetParent(clone.transform);
                    foreach (Transform child in cloneBackface.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    cloneBackface.GetComponent<BoxCollider>().enabled = false;
                    cloneBackface.transform.localPosition = Vector3.zero;
                    cloneBackface.transform.localRotation = Quaternion.identity;


                    
                    
                    cloneBackface.transform.localScale = new Vector3(1, -1, 1);
                    



                    foreach (Collider intersection in intersections)
                    {


                        if (intersection.gameObject.transform.parent != null)
                        {
                            if (intersection.gameObject.transform.parent.name == "normal_segments")
                            {
                                GameObject annotClone = Instantiate(intersection.gameObject, clone.transform);

                                annotClone.GetComponent<NearInteractionGrabbable>().enabled = false;
                                annotClone.GetComponent<ObjectManipulator>().enabled = false;
                                annotClone.GetComponent<Interactable>().enabled = false;
                                annotClone.GetComponent<ConstraintManager>().enabled = false;
                                Renderer renderer = annotClone.GetComponent<Renderer>();
                                UnityEngine.Color prevColor = renderer.material.color;
                                annotClone.layer = 2;

                                if (variables.Annot2d) {
                                        
                                    UnityEngine.Color transPrevColor = prevColor;
                                    transPrevColor.a = 0.2f;

                                    Material newMat = new Material(crossSectionMat);
                                    newMat.SetColor("_Color", transPrevColor);
                                    newMat.SetColor("_CrossColor", prevColor);

                                        
                                    newMat.SetVector("_Plane1Normal", new Vector4(planeNormal.x, planeNormal.y, planeNormal.z, 0.0f));
                                    newMat.SetVector("_Plane1Position", new Vector4(plane2Position.x, plane2Position.y, plane2Position.z, 1.0f));
                                    newMat.SetVector("_Plane2Normal", new Vector4(plane2Normal.x, plane2Normal.y, plane2Normal.z, 0.0f));
                                    newMat.SetVector("_Plane2Position", new Vector4(planePosition.x, planePosition.y, planePosition.z, 1.0f));
                                    newMat.SetVector("_Plane3Normal", new Vector4(plane2Normal.x, plane2Normal.y, plane2Normal.z, 0.0f));
                                    newMat.SetVector("_Plane3Position", new Vector4(planePosition.x, planePosition.y, planePosition.z, 1.0f));
                                    renderer.material = newMat;
                                }
                                else if (variables.Annot3d)
                                {
                                    Material material = new Material(renderer.sharedMaterial);


                                    material.shader = Shader.Find("Standard");
                                    material.SetFloat("_Mode", 3);
                                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                    material.SetInt("_ZWrite", 0);
                                    material.DisableKeyword("_ALPHATEST_ON");
                                    material.DisableKeyword("_ALPHABLEND_ON");
                                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                                    material.EnableKeyword("_EMISSION");
                                    material.renderQueue = 3000;
                                    
                                    

                                    material.SetColor("_EmissionColor", material.color * 1.0f);

                                    UnityEngine.Color newCol = material.color;
                                    newCol.a = 0.5f; // Set alpha to 0.2
                                    material.color = newCol;


                                    renderer.material = material;
                                }
                                /*if (isX)
                                {
                                    annotClone.transform.position = new Vector3(clone.transform.position.x, intersection.gameObject.transform.position.y, clone.transform.position.z);
                                }
                                if (isY)
                                {
                                    annotClone.transform.position = new Vector3(clone.transform.position.x,  clone.transform.position.y, intersection.gameObject.transform.position.z);
                                }
                                if (isZ)
                                {
                                    annotClone.transform.position = new Vector3(intersection.gameObject.transform.position.x, clone.transform.position.y, clone.transform.position.z);
                                }*/
                                //annotClone.transform.position = new Vector3(intersection.gameObject.transform.position.x, intersection.gameObject.transform.position.y, clone.transform.position.z);
                                annotClone.transform.position = intersection.gameObject.transform.position;
                                annotClone.transform.rotation = intersection.gameObject.transform.rotation;
                                Vector3 parentScale = clone.transform.lossyScale;
                                Vector3 worldScale = intersection.gameObject.transform.lossyScale;

                                annotClone.transform.localScale = new Vector3(
                                        worldScale.x / parentScale.x,
                                    worldScale.y / parentScale.y,
                                        worldScale.z / parentScale.z
                                );
                                Transform tooltip = annotClone.transform.GetChild(0);
                                ToolTip toolScript = tooltip.GetComponent<ToolTip>();
                                toolScript.ToolTipText = CapitalizeAfterDash(intersection.gameObject.name);
                                Transform anchor = annotClone.transform.GetChild(0).GetChild(0);
                                Transform pivot = annotClone.transform.GetChild(0).GetChild(1);
                                pivot.GetChild(0).gameObject.AddComponent<ConstraintManager>();
                                pivot.GetChild(0).gameObject.AddComponent<ObjectManipulator>();
                                

                                Vector3 normalizedNormal = anchor.InverseTransformDirection(planeNormal.normalized);
                                //normalizedNormal = annotClone.transform.InverseTransformDirection(normalizedNormal);





                                /*
                                Vector3 localPivotPos = pivot.localPosition;
                                
                                Vector3 localPointToPlane = localPivotPos - annotClone.transform.InverseTransformPoint(planePosition);
                               

                                float localDistance = Vector3.Dot(localPointToPlane, normalizedNormal);
                                Vector3 localClosestPoint = localPivotPos - normalizedNormal * localDistance;

                                Vector3 localAnchorPos = anchor.localPosition;
                                
                                Vector3 localAnchorPlane = localAnchorPos - annotClone.transform.InverseTransformPoint(planePosition);
                                float localAnchorDistance = Vector3.Dot(localAnchorPlane, normalizedNormal);
                                Vector3 localAnchorPoint = localAnchorPos - normalizedNormal * localAnchorDistance;
                                */
                                /*
                                Vector3 localPivotPos = pivot.localPosition;

                                Vector3 localPointToPlane = localPivotPos - clone.transform.InverseTransformPoint(planePosition);


                                float localDistance = Vector3.Dot(localPointToPlane, normalizedNormal);
                                Vector3 localClosestPoint = localPivotPos - normalizedNormal * localDistance;

                                Vector3 localAnchorPos = anchor.localPosition;

                                Vector3 localAnchorPlane = localAnchorPos - clone.transform.InverseTransformPoint(planePosition);
                                float localAnchorDistance = Vector3.Dot(localAnchorPlane, normalizedNormal);
                                Vector3 localAnchorPoint = localAnchorPos - normalizedNormal * localAnchorDistance;

                               
                                Debug.Log("Local Pivot: "+ localPivotPos);
                                Debug.Log("local ClosestPoint: " + localClosestPoint);

                                // Get the object's current world position
                                Vector3 currentWorldPosition = pivot.TransformPoint(localClosestPoint);

                                // Set the x-coordinate to the target world value
                                Vector3 targetWorldPosition = new Vector3(clone.transform.position.x, currentWorldPosition.y, currentWorldPosition.z);

                                // Convert the target world position to the object's local space
                                Vector3 targetLocalPosition = transform.InverseTransformPoint(targetWorldPosition);

                                // Set the object's local position to achieve the desired world x-coordinate
                                localClosestPoint = targetLocalPosition;
                                */


                                pivot.GetChild(0).GetChild(1).localScale = new Vector3(0.25f, 0.10f, 1.0f);
                                pivot.parent.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                                //pivot.GetChild(0).AddComponent<BoxCollider>();
                                //pivot.GetChild(0).AddComponent<NearInteractionGrabbable>();

                                pivot.parent.gameObject.SetActive(true);

                                //Vector3 direction = planePosition - anchor.TransformPoint(localAnchorPoint);
                                //localAnchorPoint = localAnchorPoint + anchor.InverseTransformDirection(direction);
                                //Vector3 direction2 = planePosition - pivot.TransformPoint(localClosestPoint);
                                //localClosestPoint = localClosestPoint + pivot.InverseTransformDirection(direction2);
                                Vector3 localAnchorPoint;
                                Vector3 localClosestPoint;
                                if (isZ)
                                {
                                    localAnchorPoint = new Vector3(clone.transform.position.x, anchor.position.y, anchor.position.z);
                                    localClosestPoint = new Vector3(clone.transform.position.x - .05f, pivot.position.y, pivot.position.z);
                                }
                                else if (isX)
                                {
                                    localAnchorPoint = new Vector3( anchor.position.x, clone.transform.position.y, anchor.position.z);
                                    localClosestPoint = new Vector3( pivot.position.x, clone.transform.position.y + .05f, pivot.position.z);
                                }
                                else
                                {
                                    localAnchorPoint = new Vector3(anchor.position.x, anchor.position.y, clone.transform.position.z);
                                    localClosestPoint = new Vector3(pivot.position.x, pivot.position.y, clone.transform.position.z + .05f);
                                }



                                if (anchor.gameObject.GetComponent<ToolTipOverride>() != null)
                                {
                                    ToolTipOverride overrider = anchor.gameObject.GetComponent<ToolTipOverride>();
                                    overrider.segMaterial = crossSectionMat;
                                    overrider.change = true;
                                    overrider.toolTipPos = localAnchorPoint;
                                    overrider.pivotPos = localClosestPoint;
                                    overrider.color = prevColor;
                                    overrider.planeNormal = planeNormal;
                                    overrider.planePosition = planePosition;
                                    overrider.plane2Normal = plane2Normal;
                                    overrider.plane2Position = plane2Position;
                                        
                                        
                                    //Debug.Log("Start Override");
                                }


                                //Debug.Log($"Closest Point: {closestPoint}");

                                //anchor.localPosition = anchor.parent.InverseTransformPoint(closestPoint);
                                //toolScript.AnchorPosition = closestPoint;
                                //tooltip.position = closestPoint;
                                //Debug.DrawLine(anchor.position, closestPoint, UnityEngine.Color.red, 2f);
                                //Debug.DrawLine(planePosition, planePosition + normalizedNormal, UnityEngine.Color.blue, 2f);

                            }

                        }
                            

                    }
                    










                    

                    Debug.Log(norm);


                    initialOffset = clone.transform.position - eventData.Pointer.Position;
                }
            }

        }

        public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
        public void OnPointerDragged(MixedRealityPointerEventData eventData) {
            if (variables.is3d)
            {
                if (eventData.Pointer is SpherePointer)
                {

                    Vector3 targetPosition = eventData.Pointer.Position + initialOffset;
                    if (isX)
                    {

                        //newPos = eventData.Pointer.Position.x - oldPos.x;
                        volume.transform.position = new Vector3(Mathf.Lerp(volume.transform.position.x, targetPosition.x, 5.0f * Time.deltaTime), volume.transform.position.y, volume.transform.position.z);
                    }
                    if (isY)
                    {
                        //newPos = eventData.Pointer.Position.y - oldPos.y;
                        volume.transform.position = new Vector3(volume.transform.position.x, Mathf.Lerp(volume.transform.position.y, targetPosition.y, 5.0f * Time.deltaTime), volume.transform.position.z);
                    }
                    if (isZ)
                    {
                        //newPos = eventData.Pointer.Position.z - oldPos.z;
                        volume.transform.position = new Vector3(volume.transform.position.x, volume.transform.position.y, Mathf.Lerp(volume.transform.position.z, targetPosition.z, 5.0f * Time.deltaTime));
                    }
                    oldPos = eventData.Pointer.Position;
                }
            } else
            {
                if (eventData.Pointer is SpherePointer)
                {

                    Vector3 targetPosition = eventData.Pointer.Position + initialOffset;
                    if (isX)
                    {

                        //newPos = eventData.Pointer.Position.x - oldPos.x;
                        clonePlane.transform.position = new Vector3(Mathf.Lerp(clonePlane.transform.position.x, targetPosition.x, 5.0f * Time.deltaTime), clonePlane.transform.position.y, clonePlane.transform.position.z);
                    }
                    if (isY)
                    {
                        //newPos = eventData.Pointer.Position.y - oldPos.y;
                        clonePlane.transform.position = new Vector3(clonePlane.transform.position.x, Mathf.Lerp(clonePlane.transform.position.y, targetPosition.y, 5.0f * Time.deltaTime), clonePlane.transform.position.z);
                    }
                    if (isZ)
                    {
                        //newPos = eventData.Pointer.Position.z - oldPos.z;
                        clonePlane.transform.position = new Vector3(clonePlane.transform.position.x, clonePlane.transform.position.y, Mathf.Lerp(clonePlane.transform.position.z, targetPosition.z, 5.0f * Time.deltaTime));
                    }
                    oldPos = eventData.Pointer.Position;
                }
            }
            
        }
        public void OnPointerUp(MixedRealityPointerEventData eventData) { }


    }

}
