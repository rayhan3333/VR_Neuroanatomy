using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Unity.VisualScripting;
using UnityEngine;

public class PointerRaycastCheck : MonoBehaviour, IMixedRealityPointerHandler
{

    private GameObject prevObj;
    private GameObject hitObject;
    public bool expansion = false;
    public GameObject tooltipPrefab;  // Assign your Tooltip prefab here
    private GameObject currentTooltip;
    
    private bool isScalingUp = false;
    public float minAlpha = 0.2f; // 50% opacity
    public float maxAlpha = 1.0f; // 100% opacity
    public float scaleFactor = 1.5f; // Target scale multiplier
    public float duration = 2f; // Time to scale over
    public GameObject expandedObj;
    private float time;
    private float pulseSpeed = 0.3f;
    public float pulseWidth = 0.02f;
    public class TractLine
    {
        public LineRenderer lineRenderer;
        public Gradient baseGradient;
        public float pulseOffset; // optional for offsetting pulses
        public float time = 0;
    }
    public List<TractLine> tracts = new List<TractLine>();

    // Start is called before the first frame update
    void Start()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
        

    }

    // Update is called once per frame
    void Update()
    {
        var focusProvider = CoreServices.InputSystem?.FocusProvider;
        if (focusProvider == null)
        {
            Debug.Log("Null focus system");
            return;
        }

        var pointer = focusProvider.PrimaryPointer;
        if (pointer.Result?.CurrentPointerTarget != null)
        {
            hitObject = pointer.Result.CurrentPointerTarget;
            //Debug.Log($"Pointer {pointer.PointerName} hit: {hitObject.name}");

            // Check if the hit object is a fiber object
            if (hitObject.name.Contains("json"))
            {
                if (hitObject == prevObj)
                {


                }
                else if (hitObject != prevObj || prevObj == null)
                {
                    foreach (Transform fiber in hitObject.transform)
                    {
                        LineRenderer lineRenderer = fiber.GetComponent<LineRenderer>();

                        SetHoverColors(lineRenderer);
                    }
                    if (prevObj != null)
                    {
                        foreach (Transform fiber in prevObj.transform)
                        {
                            LineRenderer lineRenderer = fiber.GetComponent<LineRenderer>();

                            ResetFiberColors(lineRenderer);
                        }
                    }
                }

              
            }
        }

        if (hitObject != null)
        {
            prevObj = hitObject;
        }



        if (expansion && expandedObj != null && tracts!=null)
        {
            Debug.Log("Entering Animation");
            
           
            foreach (TractLine tract in tracts)
            {
                Gradient gradient = new Gradient();
                List<GradientColorKey> colorKeys = new List<GradientColorKey>(tract.baseGradient.colorKeys);
                List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey>();
               
                if (colorKeys.Count == 6)
                {
                   

                    
                    if (tract.time + tract.pulseOffset > colorKeys[2].time && tract.time + tract.pulseOffset < colorKeys[3].time)
                    {
                        tract.time += Time.deltaTime * pulseSpeed/8;
                        tract.time %= 1;
                    } else
                    {
                        tract.time += Time.deltaTime * pulseSpeed;
                        tract.time %= 1;
                    }
                } else
                {
                    tract.time += Time.deltaTime * pulseSpeed;
                    tract.time %= 1;
                }
                float start = tract.time - pulseWidth / 2f;
                float end = tract.time + pulseWidth / 2f;

                alphaKeys.Add(new GradientAlphaKey(0.2f, 0f));
                alphaKeys.Add(new GradientAlphaKey(0.2f, Mathf.Clamp01(start - 0.001f+ tract.pulseOffset)));

                alphaKeys.Add(new GradientAlphaKey(1f, Mathf.Clamp01(start + tract.pulseOffset)));
                alphaKeys.Add(new GradientAlphaKey(1f, Mathf.Clamp01(end + tract.pulseOffset)));
                alphaKeys.Add(new GradientAlphaKey(0.2f, Mathf.Clamp01(end + 0.001f + tract.pulseOffset)));
                alphaKeys.Add(new GradientAlphaKey(0.2f, 1f));



              

                gradient.SetKeys(
                    colorKeys.ToArray(),
                    alphaKeys.OrderBy(k => k.time).ToArray()
                );

                tract.lineRenderer.colorGradient = gradient;
                Debug.Log("Gradient Set");

                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0f, 0.02f);
                curve.AddKey(colorKeys[2].time - 0.05f, 0.02f);

                curve.AddKey(colorKeys[2].time, 0.005f);
                curve.AddKey(colorKeys[3].time, 0.005f);

                curve.AddKey(colorKeys[3].time + 0.05f, 0.02f);
                curve.AddKey(1f, 0.02f);


                tract.lineRenderer.widthCurve = curve;
            }

        }
    }

    // Check if the pointer is hovering over a specific fiber
    bool IsPointerHovering(IMixedRealityPointer pointer, Transform bundle)
    {
        // Assuming pointer.Result.CurrentPointerTarget contains the object hit by the pointer
        return pointer.Result.CurrentPointerTarget == bundle.gameObject;
    }

    // Set the hover colors to cyan and yellow
    void SetHoverColors(LineRenderer lineRenderer)
    {
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = lineRenderer.colorGradient.colorKeys;
        GradientAlphaKey[] alphaKeys = lineRenderer.colorGradient.alphaKeys;

        // Modify color keys to cyan and yellow for hover effect
        if (colorKeys.Length == 2)
        {
            //Debug.Log("Color Changed");
            colorKeys[0].color = Color.cyan;
            colorKeys[1].color = Color.cyan;
        }
        if (colorKeys.Length == 6)
        {
            //Debug.Log("Color Changed");

            colorKeys[0].color = Color.cyan;
            colorKeys[1].color = Color.cyan;
            colorKeys[2].color = Color.yellow;
            colorKeys[3].color = Color.yellow;
            colorKeys[4].color = Color.cyan;
            colorKeys[5].color = Color.cyan;
        }
        // Apply the modified color keys back to the LineRenderer
        gradient.SetKeys(colorKeys, alphaKeys);

        // Assign the new gradient to the LineRenderer
        lineRenderer.colorGradient = gradient;
    }

    // Reset the fiber colors to their original state
    void ResetFiberColors(LineRenderer lineRenderer)
    {
        


        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = lineRenderer.colorGradient.colorKeys;
        GradientAlphaKey[] alphaKeys = lineRenderer.colorGradient.alphaKeys;

        // Modify color keys to cyan and yellow for hover effect
        if (colorKeys.Length == 2)
        {
            colorKeys[0].color = Color.blue;
            colorKeys[1].color = Color.blue;
        }
        if (colorKeys.Length == 6)
        {
            colorKeys[0].color = Color.blue;
            colorKeys[1].color = Color.blue;
            colorKeys[2].color = Color.red;
            colorKeys[3].color = Color.red;
            colorKeys[4].color = Color.blue;
            colorKeys[5].color = Color.blue;
        }
        // Apply the modified color keys back to the LineRenderer
        gradient.SetKeys(colorKeys, alphaKeys);

        // Assign the new gradient to the LineRenderer
        lineRenderer.colorGradient = gradient;
    }


    

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("Pointer Clicked");
        if (hitObject != null) {
            if (expansion)
            {
                //SCALE DOWN
                expansion = false;
                expandedObj = null;
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.Contains("json"))
                    {
                       // Debug.Log("Scaling Down");


                        foreach (Transform fiber in obj.transform)
                        {
                            LineRenderer lineRenderer = fiber.GetComponent<LineRenderer>();
                            Gradient originalGradient = lineRenderer.colorGradient;
                            int pointCount = lineRenderer.positionCount;
                            Vector3[] originalPositions = new Vector3[pointCount];
                            lineRenderer.GetPositions(originalPositions);
                            if (obj != hitObject)
                            {
                                ScaleDown(lineRenderer, originalPositions, originalGradient, false);

                            } else
                            {
                                ScaleDown(lineRenderer, originalPositions, originalGradient, true);

                            }
                           // UpdateLineAlpha(1f, lineRenderer, originalGradient);


                        }
                    }
                }

            } else
            {
                //SCALE UP
                expansion = true;
                expandedObj = hitObject;
                foreach (Transform obj in expandedObj.transform)
                {
                    Debug.Log("Adding objects to Tracts");
                    TractLine newTract = new TractLine();
                    newTract.lineRenderer = obj.GetComponent<LineRenderer>();
                    newTract.baseGradient = obj.GetComponent<LineRenderer>().colorGradient;
                    newTract.pulseOffset = Random.Range(0.0f, 1.0f);
                    tracts.Add(newTract);
                }
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.Contains("json"))
                    {

                        //Debug.Log("Scaling Up");

                        foreach (Transform fiber in obj.transform)
                        {

                            LineRenderer lineRenderer = fiber.GetComponent<LineRenderer>();
                            Gradient originalGradient = lineRenderer.colorGradient;
                            int pointCount = lineRenderer.positionCount;
                            Vector3[] originalPositions = new Vector3[pointCount];
                            lineRenderer.GetPositions(originalPositions);
                            if (obj != hitObject)
                            {
                                ScaleUp(lineRenderer, originalPositions, originalGradient, false);

                            }
                            else
                            {
                                ScaleUp(lineRenderer, originalPositions, originalGradient, true);

                            }

                           // UpdateLineAlpha(1f, lineRenderer, originalGradient);
                            

                        }

                    }
                }
            }
           
            SpawnTooltip(hitObject.transform);
        
        
        }
    }
    public void ScaleUp(LineRenderer lineRenderer, Vector3[] originalPositions, Gradient grad, bool ignoreAlpha)
    {
        /*if (!isScalingUp) // Prevent overlapping calls
        {
            isScalingUp = true;
            StartCoroutine(ScaleLineOverTime(scaleFactor, duration, maxAlpha, minAlpha, lineRenderer, originalPositions, grad));
        }*/
        StartCoroutine(ScaleLineOverTime(scaleFactor, duration, maxAlpha, minAlpha, lineRenderer, originalPositions, grad, ignoreAlpha));
    }

    public void ScaleDown(LineRenderer lineRenderer, Vector3[] originalPositions, Gradient grad, bool ignoreAlpha)
    {
       /* if (isScalingUp) // Prevent overlapping calls
        {
            isScalingUp = false;
            StartCoroutine(ScaleLineOverTime(1f / scaleFactor, duration, minAlpha, maxAlpha, lineRenderer, originalPositions, grad));
        }*/

        StartCoroutine(ScaleLineOverTime(1f / scaleFactor, duration, minAlpha, maxAlpha, lineRenderer, originalPositions, grad, ignoreAlpha));
    }
    IEnumerator ScaleLineOverTime(float targetScale, float time, float startAlpha, float endAlpha, LineRenderer lineRenderer, Vector3[] originalPositions, Gradient originalGradient, bool ignoreAlpha)
    {
       // Debug.Log("ScaleLineoverTimeStart");

        float elapsedTime = 0f;
        int pointCount = lineRenderer.positionCount;
        Vector3[] startPositions = new Vector3[pointCount];
        lineRenderer.GetPositions(startPositions);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;
            float scale = Mathf.Lerp(1f, targetScale, t);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            Vector3[] newPositions = new Vector3[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                newPositions[i] = originalPositions[i] * scale;
            }

            lineRenderer.SetPositions(newPositions);
            if (!ignoreAlpha)
            {
                UpdateLineAlpha(alpha, lineRenderer, originalGradient);
            }
            yield return null;
        }

        // Ensure final values match exactly
        for (int i = 0; i < pointCount; i++)
        {
            originalPositions[i] *= targetScale;
        }
        lineRenderer.SetPositions(originalPositions);
       // Debug.Log("NewPositions Set");
        if (!ignoreAlpha)
        {
            UpdateLineAlpha(endAlpha, lineRenderer, originalGradient);
        }
    }

    void UpdateLineAlpha(float alpha, LineRenderer lineRenderer, Gradient originalGradient)
    {
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = originalGradient.colorKeys;
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[originalGradient.alphaKeys.Length];

        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i] = new GradientAlphaKey(alpha, originalGradient.alphaKeys[i].time);
        }

        gradient.SetKeys(colorKeys, alphaKeys);
        lineRenderer.colorGradient = gradient;
    }
    void SpawnTooltip(Transform hitObject)
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);  // Destroy previous tooltip if it exists
        }
        BoxCollider boxCollider = hitObject.GetComponent<BoxCollider>();
        Vector3 pos = boxCollider.center;
        // Instantiate the tooltip
        currentTooltip = Instantiate(tooltipPrefab, pos, Quaternion.identity);
        // Set the text on the tooltip (assuming the prefab has a TextMesh or UI Text)
        ToolTip script = currentTooltip.GetComponent<ToolTip>();

        script.ToolTipText = "Low Axial Diffusitivity Integrity";
        // Optionally, you can set the position and other properties of the tooltip
       // currentTooltip.transform.position = transform.position + Vector3.up * 0.5f;  // Adjust position above the object

        // You can make the tooltip move with the camera if needed or use MRTK's pointer system
    }
    public void OnPointerDown(MixedRealityPointerEventData eventData) {

        //Debug.Log("Pointer Down");
    }
    public void OnPointerUp(MixedRealityPointerEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
}
