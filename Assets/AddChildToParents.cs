using UnityEngine;
using UnityEditor;

public class AddChildToParents : MonoBehaviour
{
   /* public GameObject childPrefab; // The prefab or GameObject to be added as a child
    public GameObject[] parentObjects; // The parents to which the child will be added

    [MenuItem("Tools/Add Child to Parents")]
    public static void ShowWindow()
    {
        GetWindow<AddChildToParents>("Add Child to Parents");
    }

    private void OnGUI()
    {
        GUILayout.Label("Add Child to Parents", EditorStyles.boldLabel);

        // Field to set the child prefab
        childPrefab = (GameObject)EditorGUILayout.ObjectField(childPrefab, typeof(GameObject), true);

        // Button to select parents from the current selection in the Hierarchy
        if (GUILayout.Button("Select Parents"))
        {
            parentObjects = Selection.gameObjects; // Get selected GameObjects in the editor
            if (parentObjects.Length == 0)
            {
                Debug.LogWarning("No parent objects selected. Please select parent objects in the Hierarchy.");
            }
        }

        // Button to add child to parents
        if (GUILayout.Button("Add Child to Selected Parents"))
        {
            if (childPrefab == null)
            {
                Debug.LogWarning("Please assign a child prefab.");
                return; // Exit the method early
            }

            if (parentObjects.Length == 0)
            {
                Debug.LogWarning("No parent objects selected. Please select parent objects in the Hierarchy.");
                return; // Exit the method early
            }

            foreach (GameObject parent in parentObjects)
            {
                // Instantiate the child prefab and set it as a child of the parent
                GameObject newChild = (GameObject)PrefabUtility.InstantiatePrefab(childPrefab);

                if (newChild != null)
                {
                    // Set the parent properly using SetParent
                    newChild.transform.SetParent(parent.transform, false); // The second parameter ensures local position is preserved
                    newChild.transform.localPosition = Vector3.zero; // Reset position if needed
                }
                else
                {
                    Debug.LogError($"Failed to instantiate child prefab for parent: {parent.name}");
                }
            }

            Debug.Log("Child added to selected parents.");
        }
    }*/
}
