using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositionHandler : MonoBehaviour
{
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();

    public Transform parentObject;

    public float moveSpeed = 30.0f;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    
    void Awake()
    {
        foreach (Transform child in parentObject)
        {
            originalPositions[child] = child.localPosition;
            originalScales[child] = child.localScale;
            originalRotations[child] = child.localRotation;
        }


        // Initialize the array to store child objects

        foreach (Transform child in GameObject.Find("Interactable").transform.GetChild(0))
        {
           
            Renderer rend = child.gameObject.GetComponent<Renderer>();
            Color color = rend.material.color;
            Material newMaterial = new Material(Shader.Find("Standard"));
            
            newMaterial.EnableKeyword("_EMISSION");
            //newMaterial.renderQueue = 3000;

            newMaterial.SetColor("_EmissionColor", color * .8f);
            color.a = 1f;
            newMaterial.color = color;
            child.gameObject.GetComponent<Renderer>().material = newMaterial;

           
        }
       



    }

    public void ResetPositions()
    {
        foreach (var entry in originalPositions)
        {
            StartCoroutine(MoveToPosition(entry.Key.gameObject, entry.Value));
          
        }
        foreach (var entry in originalScales)
        {
            
            entry.Key.localScale = entry.Value;
        }
        foreach (var entry in originalRotations)
        {
            
            entry.Key.localRotation = entry.Value;
        }

    }

    public void lastReset()
    {
        foreach (var entry in originalPositions)
        {

            if (entry.Key.gameObject.GetComponent<Interactable>().GetStateValue(InteractableStates.InteractableStateEnum.Pressed) == 1)
            {
                StartCoroutine(MoveToPosition(entry.Key.gameObject, entry.Value));

            }
        }
    }

    IEnumerator MoveToPosition(GameObject obj, Vector3 targetPosition)
    {
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.01f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(
                obj.transform.localPosition,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    public void mriRemover()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if(obj.name.Contains("Clone") && obj.name.Contains("VolumeRendering") || obj.name.Contains("Plane") && obj.name.Contains("Clone"))
            {
                Destroy(obj);
            }
        }

    }

    public void view1(GameObject obj) {

        foreach (Transform child in obj1.transform)
        {
            if (child.gameObject.name.Contains("Plane"))
            {
                Destroy(child.gameObject);
            }
        }
        obj.transform.position = obj1.transform.position;
        
        obj.transform.SetParent(obj1.transform);

        obj.transform.localPosition = new Vector3(0f, -.47f, 0f);
        obj.transform.localRotation = Quaternion.Euler(0f, -90f, 90f);

       
    }

    public void view2(GameObject obj)
    {

        foreach(Transform child in obj2.transform)
        {
            if (child.gameObject.name.Contains("Plane"))
            {
                Destroy(child.gameObject);
            }
        }
        obj.transform.position = obj2.transform.position;
        
        obj.transform.SetParent(obj2.transform);
        obj.transform.localPosition = new Vector3(0f, -.47f, 0f);
        obj.transform.localRotation = Quaternion.Euler(0f, -90f, 90f);


    }
    public void view3(GameObject obj)
    {
        foreach (Transform child in obj3.transform)
        {
            if (child.gameObject.name.Contains("Plane"))
            {
                Destroy(child.gameObject);
            }
        }
        obj.transform.position = obj3.transform.position;
        
        obj.transform.SetParent(obj3.transform);
        obj.transform.localPosition = new Vector3(0f, -.47f, 0f);
        obj.transform.localRotation = Quaternion.Euler(0f, -90f, 90f);
    }
    public void delete(GameObject obj)
    {

        Destroy(obj);
    }
}
