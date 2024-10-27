using Brui.Components;
using UnityEditor;
using UnityEngine;

namespace Brui.Editor
{
    public class NodeContextEditor
    {
        [MenuItem("GameObject/Node/Create Node", false, 0)]
        static void CreateNode()
        {
            if (Selection.activeGameObject != null)
            {
                // Create a new empty GameObject
                GameObject newChild = new GameObject("Node");

                // Set the newly created object as a child of the selected GameObject
                newChild.transform.parent = Selection.activeGameObject.transform;

                // Reset the child's local position to (0,0,0)
                newChild.transform.localPosition = Vector3.zero;

                // Optionally, select the newly created GameObject in the Hierarchy
                Selection.activeGameObject = newChild;

                newChild.AddComponent<NodeTransform>();
            }
        }
        
        [MenuItem("GameObject/Node/Create Node Image", false, 0)]
        static void CreateNodeImage()
        {
            if (Selection.activeGameObject != null)
            {
                // Create a new empty GameObject
                GameObject newChild = new GameObject("NodeImage");

                // Set the newly created object as a child of the selected GameObject
                newChild.transform.parent = Selection.activeGameObject.transform;

                // Reset the child's local position to (0,0,0)
                newChild.transform.localPosition = Vector3.zero;

                // Optionally, select the newly created GameObject in the Hierarchy
                Selection.activeGameObject = newChild;

                newChild.AddComponent<NodeImage>();
            }
        }
        
        [MenuItem("GameObject/Node/Create Node Button", false, 0)]
        static void CreateNodeButton()
        {
            if (Selection.activeGameObject != null)
            {
                // Create a new empty GameObject
                GameObject newChild = new GameObject("NodeButton");

                // Set the newly created object as a child of the selected GameObject
                newChild.transform.parent = Selection.activeGameObject.transform;

                // Reset the child's local position to (0,0,0)
                newChild.transform.localPosition = Vector3.zero;

                // Optionally, select the newly created GameObject in the Hierarchy
                Selection.activeGameObject = newChild;

                newChild.AddComponent<NodeButton>();
            }
        }
        
        [MenuItem("GameObject/Node/Create Node Scroll", false, 0)]
        static void CreateNodeScroll()
        {
            if (Selection.activeGameObject != null)
            {
                // Create a new empty GameObject
                GameObject newChild = new GameObject("NodeScroll");

                // Set the newly created object as a child of the selected GameObject
                newChild.transform.parent = Selection.activeGameObject.transform;

                // Reset the child's local position to (0,0,0)
                newChild.transform.localPosition = Vector3.zero;

                // Optionally, select the newly created GameObject in the Hierarchy
                Selection.activeGameObject = newChild;

                newChild.AddComponent<NodeScroll>();
            }
        }
        
        [MenuItem("GameObject/Node/Create Node Layout", false, 0)]
        static void CreateNodeLayout()
        {
            if (Selection.activeGameObject != null)
            {
                // Create a new empty GameObject
                GameObject newChild = new GameObject("NodeLayout");

                // Set the newly created object as a child of the selected GameObject
                newChild.transform.parent = Selection.activeGameObject.transform;

                // Reset the child's local position to (0,0,0)
                newChild.transform.localPosition = Vector3.zero;

                // Optionally, select the newly created GameObject in the Hierarchy
                Selection.activeGameObject = newChild;

                newChild.AddComponent<NodeLayout>();
            }
        }
        
        [MenuItem("GameObject/Node/Create Node Text", false, 0)]
        static void CreateNodeText()
        {
            if (Selection.activeGameObject != null)
            {
                // Create a new empty GameObject
                GameObject newChild = new GameObject("NodeText");

                // Set the newly created object as a child of the selected GameObject
                newChild.transform.parent = Selection.activeGameObject.transform;

                // Reset the child's local position to (0,0,0)
                newChild.transform.localPosition = Vector3.zero;

                // Optionally, select the newly created GameObject in the Hierarchy
                Selection.activeGameObject = newChild;

                newChild.AddComponent<NodeText>();
            }
        }
    }
}
