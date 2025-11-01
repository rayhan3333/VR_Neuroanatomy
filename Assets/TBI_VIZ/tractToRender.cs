using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class tractToRender : MonoBehaviour
{
    public string fileName = "streamlines_AF_L.json";
    public string fileName2 = "gradients_AF_L.json";
    public Material lineMaterial;  // Assign a material in the inspector
    public GameObject fiberViz;
    private int MAX_KEYS = 8;
    void Start()
    {
        lineMaterial = new Material(Shader.Find("Standard"));

        // Set the color to white
        lineMaterial.color = Color.white;
        foreach (string file in Directory.GetFiles(Application.streamingAssetsPath)) {
            string newfile = file.Replace("\\", "/");
            Debug.Log(newfile);
            if(file.Contains("gradients") || file.Contains("meta"))
            {
                continue;
            } else
            {
                
                LoadStreamlines(newfile);
            }





        }
        
    }

    void LoadStreamlines(string file)
    {
        string streamlinePath =file;
        if (!File.Exists(streamlinePath))
        {
            Debug.LogError("Streamlines file not found: " + file);
            return;
        }
        int fileInd = file.IndexOf("streamlines");

        string gradientPath = file.Substring(0, fileInd) + "gradients" + file.Substring(fileInd+11);
        if (!File.Exists(gradientPath))
        {
            Debug.LogError("Gradients file not found: " + gradientPath);
            return;
        }

        string json = File.ReadAllText(streamlinePath);
        string json2 = File.ReadAllText(gradientPath);
        List<List<List<float>>> streamlines = JsonConvert.DeserializeObject<List<List<List<float>>>>(json);
        List<List<List<float>>> gradients = JsonConvert.DeserializeObject<List<List<List<float>>>>(json2);
        //Debug.Log(gradients[0][0][0]);

        GameObject bundle = new GameObject(file.Substring(fileInd+12));
        Quaternion rotation = Quaternion.Euler(270, 180, 0);

        for (int i = 0; i < streamlines.Count; i++)
        {
            if (!(i % 10 == 0)) { continue; }
            for (int j = 0; j < streamlines[i].Count; j++) { 

                
                    Vector3 vecpos = new Vector3(streamlines[i][j][0], streamlines[i][j][1], streamlines[i][j][2]);
                    vecpos = rotation * vecpos;
                streamlines[i][j] = new List<float> { vecpos.x, vecpos.y, vecpos.z };
                
            
            }
            DrawStreamline(streamlines[i], gradients[i], i.ToString(), bundle);
        }
        Vector3 low;
        Vector3 high;
        if (streamlines.Count != 0)
        {
            float lowx = Mathf.Infinity;
            float lowy = Mathf.Infinity;

            float lowz = Mathf.Infinity;

            float highx = -Mathf.Infinity;
            float highy = -Mathf.Infinity;
            float highz = -Mathf.Infinity;
        
            foreach (List<float> val in streamlines[0])
            {
                if (val[0] <  lowx) lowx = val[0];
                if (val[1] < lowy) lowy = val[1];
                if (val[2] < lowz) lowz = val[2];

                if (val[0] > highx) highx = val[0];
                if (val[1] > highy) highy = val[1];
                if (val[2] > highz) highz = val[2];
            }
            low = new Vector3(lowx, lowy, lowz) / 30f;
            high = new Vector3(highx, highy, highz) / 30f;

            BoxCollider col = bundle.AddComponent<BoxCollider>();
            col.center = (high + low) / 2;

            col.size = new Vector3(
        Mathf.Abs(high.x - low.x),
        Mathf.Abs(high.y - low.y),
        Mathf.Abs(high.z - low.z)
    );

        }
    }


    void DrawStreamline(List<List<float>> streamline, List<List<float>> gradient, string index, GameObject bundle) {
        GameObject newObj = new GameObject("fiber"+index);
        newObj.transform.SetParent(bundle.transform);
        LineRenderer lineRend = newObj.AddComponent<LineRenderer>();
        Vector3[] positions = new Vector3[streamline.Count];
        lineRend.positionCount = streamline.Count;
        Shader unlitShader = Shader.Find("Unlit/Transparent");
        if (unlitShader == null)
        {
            Debug.LogWarning("Shader 'Unlit/Color' not found! Using default material.");
            lineMaterial = new Material(Shader.Find("Sprites/Default")); // Fallback shader
        }
        else
        {
            lineMaterial = new Material(unlitShader);
        }
        lineMaterial = new Material(Shader.Find("Sprites/Default"));
        // Assign the material to the LineRenderer
        lineRend.material = lineMaterial;
        lineRend.textureMode = LineTextureMode.Stretch;  // Ensure proper color rendering
        lineRend.startWidth = 0.02f;
        lineRend.endWidth = 0.02f;
        for (int i = 0; i < streamline.Count; i++)
        {
            positions[i] = new Vector3(streamline[i][0], streamline[i][1], streamline[i][2]) / 30f;
        }
        lineRend.SetPositions(positions);

        int totalPoints = lineRend.positionCount;
        if (totalPoints < 2) return;



        // Sample 8 evenly spaced points along the line
        Debug.Log(gradient);

        if (gradient.Count == 0)
        {
            Gradient gradientcols = new Gradient();
            gradientcols.SetKeys(
           new GradientColorKey[] {
                new GradientColorKey(Color.blue, 0.0f),  // Blue at start
                
                new GradientColorKey(Color.blue, 1.0f)
           },
           new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f)
           }

       );
            lineRend.colorGradient = gradientcols;
        }

        for (int i = 0; i < gradient.Count; i++)
        {

            Gradient gradientcols = new Gradient();
            //GradientColorKey[] colorKeys = new GradientColorKey[MAX_KEYS];
            //GradientAlphaKey[] alphaKeys = new GradientAlphaKey[MAX_KEYS];
            gradientcols.SetKeys(
           new GradientColorKey[] {
                new GradientColorKey(Color.blue, 0.0f),  // Blue at start
                new GradientColorKey(Color.blue, gradient[i][0]),
                new GradientColorKey(Color.red, gradient[i][0]),
               new GradientColorKey(Color.red, gradient[i][1]),
               new GradientColorKey(Color.blue, gradient[i][1]),
                new GradientColorKey(Color.blue, 1.0f)  
           },
           new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f)
           }

       );
            lineRend.colorGradient = gradientcols;

            /*if (i == MAX_KEYS - 1)
            {
                colorKeys[i] = colorKeys[i - 1];
                alphaKeys[i] = alphaKeys[i - 1];
                continue;

            }
            float t = i / (float)(MAX_KEYS - 1); // Normalize to 0-1
            int ind = Mathf.FloorToInt(t * (totalPoints - 1)); // Map to index in the LineRenderer
           */
            /*
            Vector3 direction = Vector3.zero;
            if (ind < totalPoints - 1)
                direction = (lineRend.GetPosition(ind + 1) - lineRend.GetPosition(ind)).normalized;

            // RGB based on direction
            Color tractColor = new Color(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));

            colorKeys[i] = new GradientColorKey(tractColor, t);
            alphaKeys[i] = new GradientAlphaKey(1.0f, t);*/


        }

        // gradient.SetKeys(colorKeys, alphaKeys);
        // lineRend.colorGradient = gradient;

    }


    void Update()
    {
        
    }
}
