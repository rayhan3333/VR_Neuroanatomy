using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using System.Text;
using System.Linq;


public class ToolTipFOV : MonoBehaviour
    {

    /*public float detectionRadius = 10.0f;  
       // public float fieldOfViewAngle = 1.0f;
    public Camera camera;
    public List<GameObject> allObjects;
    


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
    private void Start()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()) {
            if (obj.name.Contains("Tooltip") && obj.hideFlags != HideFlags.HideInHierarchy) {
                allObjects.Add(obj);            
            
            }
        
        
        }

        Debug.Log("all obj  " + allObjects.Count);

    }


    void Update() { 
            
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        List<GameObject> detectedObjects = new List<GameObject>();

        foreach (Collider collider in colliders)
        {
            if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
            {
                foreach (Transform child in collider.gameObject.transform)
                {
                    if (child.name.Contains("Tooltip"))
                    {
                        


                        float distance = Vector3.Distance(transform.position, collider.transform.position);
                        
                        // Logic for tooltip spawning based on radius
                        ToolTip script = child.gameObject.GetComponent<ToolTip>();
                        if (distance <= 1.0f && !child.parent.name.Contains("View"))
                        {
                            detectedObjects.Add(child.gameObject);
                            if (child.localScale == Vector3.zero)
                            {
                                child.gameObject.SetActive(true);
                                script.ToolTipText = CapitalizeAfterDash(collider.gameObject.name);

                                StartCoroutine(SpawnTooltip(child.gameObject));
                            }
                        }
                        if (child.GetChild(0).GetComponent<ToolTipOverride>().updateShader)
                        {
                            if (distance <= detectionRadius && child.parent.name.Contains("View"))
                            {
                                detectedObjects.Add(child.gameObject);
                                if (child.localScale == Vector3.zero)
                                {
                                    child.gameObject.SetActive(true);
                                    script.ToolTipText = CapitalizeAfterDash(child.GetChild(0).GetComponent<ToolTipOverride>().annot_name);

                                    StartCoroutine(SpawnTooltip(child.gameObject));
                                }
                            }
                            
                           
                        }
                    }
                }
            }
        }
        foreach (GameObject obj in allObjects) { 
        
            if (!detectedObjects.Contains(obj) && obj.transform.localScale == new Vector3(0.4f, 0.4f, 0.4f))
                    {
                        Debug.Log("Not in Frustum and nonzero");

                        
                        StartCoroutine(Deactivate(obj));
                    }
        
        }
        //Debug.Log("detected obj:  " + detectedObjects.Count);
    }









    
        private IEnumerator SpawnTooltip(GameObject obj)
    {
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = new Vector3(0.4f, 0.4f, 0.4f);
        if (obj.transform.GetChild(0).GetComponent<ToolTipOverride>().isView)
        {
            targetScale = new Vector3(2.15f, 2.15f, 2.15f);
        }
        float duration = .25f;
        float elapsedTime = 0f;
        Transform newchild = obj.transform.Find("Pivot/ContentParent/TipBackground");
        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            newchild.localScale = Vector3.Lerp(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.25f, 0.10f, 1.0f), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
        newchild.localScale = new Vector3(0.25f, 0.10f, 1.0f);
        
    }
        private IEnumerator Deactivate(GameObject obj)
        {
        Vector3 initialScale = new Vector3(0.4f, 0.4f, 0.4f);
        Vector3 targetScale = Vector3.zero;
        float duration = .25f;
        float elapsedTime = 0f;
        Transform newchild = obj.transform.Find("Pivot/ContentParent/TipBackground");
        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            newchild.localScale = Vector3.Lerp(new Vector3(0.25f, 0.10f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
        newchild.localScale = new Vector3(0.0f, 0.0f, 1.0f);

        obj.SetActive(false);

        }


    private void deactivateObject(GameObject obj) { 
    
        StartCoroutine(Deactivate(obj));
    }
    */
    public float detectionRadius = 0.5f;
    // public float fieldOfViewAngle = 1.0f;
    public Camera camera;
    public List<GameObject> allObjects;



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
    private void Start()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name.Contains("Tooltip") && obj.hideFlags != HideFlags.HideInHierarchy)
            {
                allObjects.Add(obj);

            }


        }

        Debug.Log("all obj  " + allObjects.Count);

    }


    void Update()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        List<GameObject> detectedObjects = new List<GameObject>();

        foreach (Collider collider in colliders)
        {
            if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
            {
                foreach (Transform child in collider.gameObject.transform)
                {
                    if (child.name.Contains("Tooltip") && !child.parent.name.Contains("View"))
                    {
                        detectedObjects.Add(child.gameObject);
                        if (child.localScale == Vector3.zero)
                        {
                            //Debug.Log("In Frustum and Zero");

                            child.gameObject.SetActive(true);
                            // Activate tooltip logic
                            ToolTip script = child.gameObject.GetComponent<ToolTip>();
                            script.ToolTipText = CapitalizeAfterDash(collider.gameObject.name);
                            StartCoroutine(SpawnTooltip(child.gameObject));
                        }
                    }
                }
            }
        }
        foreach (GameObject obj in allObjects)
        {

            if (!detectedObjects.Contains(obj) && obj.transform.localScale == new Vector3(0.4f, 0.4f, 0.4f) && obj.transform.parent != null)
            {
                if (!obj.transform.parent.name.Contains("View")) {
                    //Debug.Log("Not in Frustum and nonzero");


                    StartCoroutine(Deactivate(obj));
                }
            }

        }
        //Debug.Log("detected obj:  " + detectedObjects.Count);
    }


    






    /*private void ActivateObject(GameObject obj)
    {
           




    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

    foreach (Transform child in obj.transform)
    {
        if (child.name.Contains("Tooltip"))
        {
            // Check if the child is within the frustum
            if (GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds) && child.localScale == Vector3.zero)
            {
                Debug.Log("In Frustum and Zero");
                child.gameObject.SetActive(true);
                // Activate tooltip logic
                ToolTip script = child.gameObject.GetComponent<ToolTip>();
                script.ToolTipText = CapitalizeAfterDash(obj.name);
                StartCoroutine(SpawnTooltip(child.gameObject));

            }
              

            else if (!GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds) && child.localScale == new Vector3(0.4f, 0.4f, 0.4f))
            {
                Debug.Log("Not in Frustum and nonzero");
                    
                    
                StartCoroutine(Deactivate(child.gameObject));

            }
                
        }
    }
    }*/
    private IEnumerator SpawnTooltip(GameObject obj)
    {
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = new Vector3(0.4f, 0.4f, 0.4f);
        float duration = 0.25f;
        float elapsedTime = 0f;
        Transform newchild = obj.transform.Find("Pivot/ContentParent/TipBackground");
        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            newchild.localScale = Vector3.Lerp(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.25f, 0.10f, 1.0f), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
        newchild.localScale = new Vector3(0.25f, 0.10f, 1.0f);

    }
    private IEnumerator Deactivate(GameObject obj)
    {
        Vector3 initialScale = new Vector3(0.4f, 0.4f, 0.4f);
        Vector3 targetScale = Vector3.zero;
        float duration = 0.25f;
        float elapsedTime = 0f;
        Transform newchild = obj.transform.Find("Pivot/ContentParent/TipBackground");
        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            newchild.localScale = Vector3.Lerp(new Vector3(0.25f, 0.10f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
        newchild.localScale = new Vector3(0.0f, 0.0f, 1.0f);

        obj.SetActive(false);

    }


    private void deactivateObject(GameObject obj)
    {

        StartCoroutine(Deactivate(obj));
    }

}