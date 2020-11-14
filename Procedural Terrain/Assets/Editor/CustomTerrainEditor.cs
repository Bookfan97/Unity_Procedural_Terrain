using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;
using UnityEditor.Experimental.TerrainAPI;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]

public class CustomTerrainEditor : Editor
{
    //Properties
    SerializedProperty randomHightRange;
    
    //Fold outs
    bool showRandom = false;
    
    private void OnEnable()
    {
        randomHightRange = serializedObject.FindProperty("randomHeightRange");
    }

    public override void OnInspectorGUI()
    {
        //Start
        serializedObject.Update();
        //Start

        CustomTerrain terrain = (CustomTerrain) target;
        showRandom = EditorGUILayout.Foldout(showRandom, "Random");
        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set Heights Between Random Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHightRange);
            if (GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset Terrain"))
            {
                terrain.ResetTerrain();
            }

        //End
        serializedObject.ApplyModifiedProperties();
        //End
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
