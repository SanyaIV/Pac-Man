using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This class is used to expand the Inspector UI for Nodes in order to add a button that when pressed runs the method to get neighbouring nodes.
/// </summary>
[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Get Node Neighbours"))
        {
            foreach(Node obj in targets)
                obj.FindNeighbours();

            Debug.Log("Finished building node neighbours");
        }
    }

}
