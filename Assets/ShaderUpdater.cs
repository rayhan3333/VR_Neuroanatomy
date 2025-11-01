using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderUpdater : MonoBehaviour
{
    public Vector3 position;
    public Vector3 normal;
    public Vector3 previousPosition;
    public Vector3 plane1Position;
    public Vector3 plane2Position;
    public Vector3 plane1Normal;
    public Vector3 plane2Normal;
    public Vector3 previousNormal;
    public Material crossSectionMat;

    public GameObject positionManager;
    public GlobalVariables variables;
    private bool Updated3D= false;
    private bool UpdatedShader = false;
    private bool Updated2D = false;

    //public GameObject view1;
    //public GameObject view2;
   // public GameObject view3;
   // private Dictionary<GameObject, bool> gameObjectStates = new Dictionary<GameObject, bool>();
    //public bool prevState1;
    //public bool prevState2;
    //public int shaderChange;
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        previousNormal = transform.up;
        normal = transform.up;
        variables = positionManager.GetComponent<GlobalVariables>();

       // gameObjectStates.Add(view1, false); // Object1 is not filled initially
       // gameObjectStates.Add(view2, false);  // Object2 is filled initially
       // gameObjectStates.Add(view3, false); // Object3 is not filled initially
    }

    // Update is called once per frame
    void Update()
    {


        //GameObject firstNotFilled = FindFirstNotFilled();

        if (variables.Annot3d && !Updated3D)
        {
            //Debug.Log("entered 3d manipulation");
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Contains("Plane") || child.gameObject.name.Contains("Pressable")) {
                    continue;
                }
                child.gameObject.GetComponent<Renderer>().enabled = true;
                Color color = child.GetChild(0).GetChild(0).GetComponent<ToolTipOverride>().color;
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
            Debug.Log("All objects updated to 3d Annotation");
            Updated3D = true;
            UpdatedShader = false;
            Updated2D = false;

        }

        else if (variables.Annot2d)
        {
            //Debug.Log("entered 2d manipulation");
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            normal = transform.up;
            float angleDifference = Vector3.Angle(normal, previousNormal);
            if (distanceMoved > .02f || angleDifference > 0.5f || !Updated2D)
            {
                plane1Position = transform.position + normal * .02f;
                plane2Position = transform.position - normal * .02f;
                plane2Normal = transform.up;
                plane1Normal = -transform.up;
                previousPosition = transform.position;
                previousNormal = transform.up;

                //Debug.Log("MOVED or ROTATED");
                foreach (Transform child in transform)
                {
                    if (child.gameObject.name.Contains("Plane") || child.gameObject.name.Contains("Pressable"))
                    {
                        continue;
                    }
                    child.gameObject.GetComponent<Renderer>().enabled = true;
                    Color color = child.GetChild(0).GetChild(0).GetComponent<ToolTipOverride>().color;
                    Renderer renderer = child.GetComponent<Renderer>();
                    //Color color = renderer.material.GetColor("_Color");



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
                Debug.Log("All objects updated to 2d Annotation");
                Updated3D = false;
                UpdatedShader = false;
                Updated2D = true;

            }
        }
        if (!variables.Annot2d && !variables.Annot3d && !UpdatedShader)
        {
            
            foreach (Transform child in transform) 
            {
                if (child.gameObject.name.Contains("Plane") || child.gameObject.name.Contains("Pressable"))
                {
                    continue;
                }
                child.gameObject.GetComponent<Renderer>().enabled = false;
                
            
            }
            UpdatedShader = true;
            Updated3D = false;
            Updated2D = false;
            //Debug.Log("Objects updated to no Annotations");
        }


    }
}
