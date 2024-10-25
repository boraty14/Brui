using Brui.Components;
using UnityEditor;
using UnityEngine;

namespace Brui.Editor
{
    public class NodeContextEditor
    {
        [MenuItem("GameObject/Create Node", false, 0)]
        static void CustomAction()
        {
            if (Selection.activeGameObject != null)
            {
                // Create a new empty GameObject
                GameObject newChild = new GameObject("NewNode");

                // Set the newly created object as a child of the selected GameObject
                newChild.transform.parent = Selection.activeGameObject.transform;

                // Reset the child's local position to (0,0,0)
                newChild.transform.localPosition = Vector3.zero;

                // Optionally, select the newly created GameObject in the Hierarchy
                Selection.activeGameObject = newChild;

                newChild.AddComponent<NodeTransform>();
            }
        }
    }
}
