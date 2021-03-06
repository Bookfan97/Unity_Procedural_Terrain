﻿using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor
{
    //Properties
    private SerializedProperty randomHeightRange;
    private SerializedProperty heightMapScale;
    private SerializedProperty heightMapImage;
    private SerializedProperty perlinXScale;
    private SerializedProperty perlinYScale;
    private SerializedProperty perlinOffsetX;
    private SerializedProperty perlinOffsetY;
    private SerializedProperty perlinOctaves;
    private SerializedProperty perlinPersistance;
    private SerializedProperty perlinHeightScale;
    private SerializedProperty resetTerrain;
    private SerializedProperty voronoiPeaks;
    private SerializedProperty voronoiFallOff;
    private SerializedProperty voronoiDropOff;
    private SerializedProperty voronoiMinHeight;
    private SerializedProperty voronoiMaxHeight;
    private SerializedProperty voronoiType;
    private SerializedProperty MPDheightMin;
    private SerializedProperty MPDheightMax;
    private SerializedProperty MPDheightDampenerPower;
    private SerializedProperty MPDroughness;
    private SerializedProperty smoothAmount;
    private SerializedProperty vegetation;
    private SerializedProperty maxTrees;
    private SerializedProperty treeSpacing;
    private SerializedProperty splatHeights;
    private SerializedProperty perlinParameters;
    private SerializedProperty details;
    private SerializedProperty maxDetails;
    private SerializedProperty detailSpacing;
    private SerializedProperty waterHeight;    
    private SerializedProperty waterGameObject;
    private SerializedProperty shoreLineMaterial;
    private SerializedProperty erosionType;
    private SerializedProperty erosionStrength;
    private SerializedProperty springsPerRiver;
    private SerializedProperty solubility;
    private SerializedProperty droplets;
    private SerializedProperty erosionSmoothAmount;
    private SerializedProperty erosionAmount;
    private SerializedProperty numClouds;
    private SerializedProperty particlesPerCloud;
    private SerializedProperty cloudStartSize;
    private SerializedProperty cloudMinScale;
    private SerializedProperty cloudMaxScale;
    private SerializedProperty cloudMaterial;
    private SerializedProperty cloudShadowMaterial;
    private SerializedProperty cloudColor;
    private SerializedProperty cloudLining;
    private SerializedProperty cloudMinSpeed;
    private SerializedProperty cloudMaxSpeed;
    private SerializedProperty cloudRange;

    //Tables
    private GUITableState vegMapTable;    
    private GUITableState splatMapTable;
    private GUITableState perlinParameterTable;
    private GUITableState detailMapTable;
    
    //Fold outs
    private bool showRandom = false;
    private bool showLoadHeights = false;
    private bool showPerlinNoise = false;
    private bool showMultiplePerlin = false;
    private bool showVoronoi = false;
    private bool showMPD = false;
    private bool showSmooth = false;
    private bool showSplatMaps = false;
    private bool showVeg = false;
    private bool showHeights = false;
    private bool showDetails = false;
    private bool showWater = false;
    private bool showErosion = false;
    private bool showClouds = false;

    private Texture2D hmTexture;

    private void OnEnable()
    {
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        heightMapScale = serializedObject.FindProperty("heightMapScale");
        heightMapImage = serializedObject.FindProperty("heightMapImage");
        perlinXScale = serializedObject.FindProperty("perlinXScale");
        perlinYScale = serializedObject.FindProperty("perlinYScale");
        perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
        perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
        perlinOctaves = serializedObject.FindProperty("perlinOctaves");
        perlinPersistance = serializedObject.FindProperty("perlinPersistance");
        perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");
        resetTerrain = serializedObject.FindProperty("resetTerrain");
        perlinParameterTable = new GUITableState("perlinParameterTable");
        perlinParameters = serializedObject.FindProperty("perlinParameters");
        voronoiDropOff = serializedObject.FindProperty("voronoiDropOff");
        voronoiFallOff = serializedObject.FindProperty("voronoiFallOff");
        voronoiMinHeight = serializedObject.FindProperty("voronoiMinHeight");
        voronoiMaxHeight = serializedObject.FindProperty("voronoiMaxHeight");
        voronoiPeaks = serializedObject.FindProperty("voronoiPeaks");
        voronoiType = serializedObject.FindProperty("voronoiType");
        MPDheightMin = serializedObject.FindProperty("MPDheightMin");
        MPDheightMax = serializedObject.FindProperty("MPDheightMax");
        MPDheightDampenerPower = serializedObject.FindProperty("MPDheightDampenerPower");
        MPDroughness = serializedObject.FindProperty("MPDroughness");
        smoothAmount = serializedObject.FindProperty("smoothAmount");
        splatMapTable = new GUITableState("splatMapTable");
        splatHeights = serializedObject.FindProperty("splatHeights");
        vegMapTable = new GUITableState("vegMapTable");
        vegetation = serializedObject.FindProperty("vegetation");
        maxTrees = serializedObject.FindProperty("maxTrees");
        treeSpacing = serializedObject.FindProperty("treeSpacing");
        detailMapTable = new GUITableState("detailMapTable");
        details = serializedObject.FindProperty("details");
        maxDetails = serializedObject.FindProperty("maxDetails");
        detailSpacing = serializedObject.FindProperty("detailSpacing");
        waterHeight = serializedObject.FindProperty("waterHeight");
        waterGameObject = serializedObject.FindProperty("waterGameObject");
        shoreLineMaterial = serializedObject.FindProperty("shoreLineMaterial");
        erosionType = serializedObject.FindProperty("erosionType");
        erosionStrength = serializedObject.FindProperty("erosionStrength");
        springsPerRiver = serializedObject.FindProperty("springsPerRiver");
        solubility = serializedObject.FindProperty("solubility");
        droplets = serializedObject.FindProperty("droplets");
        erosionSmoothAmount = serializedObject.FindProperty("erosionSmoothAmount");
        erosionAmount = serializedObject.FindProperty("erosionAmount");
        numClouds = serializedObject.FindProperty("numClouds");
        particlesPerCloud = serializedObject.FindProperty("particlesPerCloud");
        cloudStartSize = serializedObject.FindProperty("cloudStartSize");
        cloudMinScale = serializedObject.FindProperty("cloudMinScale");
        cloudMaxScale = serializedObject.FindProperty("cloudMaxScale");
        cloudMaterial = serializedObject.FindProperty("cloudMaterial");
        cloudShadowMaterial = serializedObject.FindProperty("cloudShadowMaterial");
        cloudColor = serializedObject.FindProperty("cloudColor");
        cloudLining = serializedObject.FindProperty("cloudLining");
        cloudMinSpeed = serializedObject.FindProperty("cloudMinSpeed");
        cloudMaxSpeed = serializedObject.FindProperty("cloudMaxSpeed");
        cloudRange = serializedObject.FindProperty("cloudRange");
        hmTexture = new Texture2D(513, 513, TextureFormat.ARGB32, false);
    }
    private Vector2 scrollPos;

    public override void OnInspectorGUI()
    {
        //Start
        serializedObject.Update();
        //Start

        CustomTerrain terrain = (CustomTerrain)target;
        Rect r = EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(r.width),
            GUILayout.Height(r.height));
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(resetTerrain);

        showRandom = EditorGUILayout.Foldout(showRandom, "Random");
        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set Heights Between Random Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);
            if (GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }
        }
        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Load Heights");
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights from Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightMapImage);
            EditorGUILayout.PropertyField(heightMapScale);
            if (GUILayout.Button("Load Texture"))
            {
                terrain.LoadTexture();
            }
        }
        showPerlinNoise = EditorGUILayout.Foldout(showPerlinNoise, "Single Perlin Noise");
        if (showPerlinNoise)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Perlin Noise Scale", EditorStyles.boldLabel);
            EditorGUILayout.Slider(perlinXScale, 0, 1, new GUIContent("X Scale"));
            EditorGUILayout.Slider(perlinYScale, 0, 1, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinOffsetX, 0, 10000, new GUIContent("Offset X"));
            EditorGUILayout.IntSlider(perlinOffsetY, 0, 10000, new GUIContent("Offset Y"));
            EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(perlinPersistance, 0.1f, 10, new GUIContent("Persistance"));
            EditorGUILayout.Slider(perlinHeightScale, 0, 1, new GUIContent("Height Scale"));

            if (GUILayout.Button("Run Perlin Noise"))
            {
                terrain.Perlin();
            }
        }
        showMultiplePerlin = EditorGUILayout.Foldout(showMultiplePerlin, "Multiple Perlin Noise");
        if (showMultiplePerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Mulitple Perlin Noise", EditorStyles.boldLabel);
            perlinParameterTable = GUITableLayout.DrawTable(perlinParameterTable, serializedObject.FindProperty("perlinParameters"));
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                terrain.AddNewPerlin();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemovePerlin();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Multiple Perlin"))
            {
                terrain.MultiplePerlinTerrain();
            }
        }
        showVoronoi = EditorGUILayout.Foldout(showVoronoi, "Voronoi");
        if (showVoronoi)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Voronoi", EditorStyles.boldLabel);
            EditorGUILayout.IntSlider(voronoiPeaks, 1, 10, new GUIContent("Peak Count"));
            EditorGUILayout.Slider(voronoiFallOff, 0, 10, new GUIContent("Falloff"));
            EditorGUILayout.Slider(voronoiDropOff, 0, 10, new GUIContent("Dropoff"));
            EditorGUILayout.Slider(voronoiMinHeight, 0, 1, new GUIContent("Min Height"));
            EditorGUILayout.Slider(voronoiMaxHeight, 0, 1, new GUIContent("Max Height"));
            EditorGUILayout.PropertyField(voronoiType);
            if (GUILayout.Button("Voronoi"))
            {
                terrain.Voronoi();
            }
        }
        showMPD = EditorGUILayout.Foldout(showMPD, "Midpoint Displacement");
        if (showMPD)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Mid Point Displacement", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(MPDheightMin);
            EditorGUILayout.PropertyField(MPDheightMax);
            EditorGUILayout.PropertyField(MPDheightDampenerPower);
            EditorGUILayout.PropertyField(MPDroughness);
            if (GUILayout.Button("Calculate Mid Point Displacement"))
            {
                terrain.MidPointDisplacement();
            }
        }
        showSmooth = EditorGUILayout.Foldout(showSmooth, "Smooth Terrain");
        if (showSmooth)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Smooth Terrain", EditorStyles.boldLabel);
            EditorGUILayout.IntSlider(smoothAmount, 1, 10, new GUIContent("smoothAmount"));
            if (GUILayout.Button("Smooth Terrain"))
            {
                terrain.Smooth();
            }
        }
        showSplatMaps = EditorGUILayout.Foldout(showSplatMaps, "Splat Maps");
        if (showSplatMaps)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Splat Maps", EditorStyles.boldLabel);
            splatMapTable = GUITableLayout.DrawTable(splatMapTable, serializedObject.FindProperty("splatHeights"));
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                terrain.AddNewSplatHeight();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemoveSplatHeight();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Splatmaps"))
            {
                terrain.SplatMaps();
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        showVeg = EditorGUILayout.Foldout(showVeg, "Vegetation");
        if (showVeg)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Trees", EditorStyles.boldLabel);
            EditorGUILayout.IntSlider(maxTrees, 0, 10000, new GUIContent("Maximum Trees"));
            EditorGUILayout.IntSlider(treeSpacing, 2, 20, new GUIContent("Tree Spacing"));
            vegMapTable = GUITableLayout.DrawTable(vegMapTable, serializedObject.FindProperty("vegetation"));
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                terrain.AddNewVegetation();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemoveVegetation();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Vegetation"))
            {
                terrain.PlantVegetation();
            }
        }
        showDetails = EditorGUILayout.Foldout(showDetails, "Details");
        if (showDetails)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Detail", EditorStyles.boldLabel);
            EditorGUILayout.IntSlider(maxDetails, 0, 10000, new GUIContent("Maximum Details"));
            EditorGUILayout.IntSlider(detailSpacing, 2, 20, new GUIContent("Detail Spacing"));
            detailMapTable = GUITableLayout.DrawTable(detailMapTable, serializedObject.FindProperty("details"));
            terrain.GetComponent<Terrain>().detailObjectDistance = maxDetails.intValue;
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                terrain.AddNewDetails();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemoveDetails();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Details"))
            {
                terrain.AddDetails();
            }
        }
        showWater = EditorGUILayout.Foldout(showWater, "Water");
        if (showWater)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Water", EditorStyles.boldLabel);
            EditorGUILayout.Slider(waterHeight, 0, 1, new GUIContent("Water Height"));
            EditorGUILayout.PropertyField(waterGameObject);
            if (GUILayout.Button("Add Water"))
            {
                terrain.AddWater();
            }
            EditorGUILayout.PropertyField(shoreLineMaterial);
            if (GUILayout.Button("Draw Shoreline"))
            {
                terrain.DrawShoreline();
            }
        }
        showErosion = EditorGUILayout.Foldout(showErosion, "Erosion");
        if (showErosion)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Erosion", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(erosionType);
            EditorGUILayout.Slider(erosionStrength, 0, 1, new GUIContent("Erosion Strength"));
            EditorGUILayout.Slider(erosionAmount, 0, 1, new GUIContent("Erosion Amount"));
            EditorGUILayout.IntSlider(droplets, 0, 500, new GUIContent("Droplets"));
            EditorGUILayout.Slider(solubility, 0.01f, 1, new GUIContent("Solubility"));
            EditorGUILayout.IntSlider(springsPerRiver, 0, 20, new GUIContent("Springs Per River"));
            EditorGUILayout.IntSlider(erosionSmoothAmount, 0, 10, new GUIContent("Smooth Amount"));
            if (GUILayout.Button("Add Erosion"))
            {
                terrain.Erode();
            }
        }
        showClouds = EditorGUILayout.Foldout(showClouds, "Clouds");
        if (showClouds)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Clouds", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(numClouds, new GUIContent("Number of Clouds"));
            EditorGUILayout.PropertyField(particlesPerCloud, new GUIContent("Particles per Cloud"));
            EditorGUILayout.PropertyField(cloudStartSize, new GUIContent("Cloud Particle Size"));
            EditorGUILayout.PropertyField(cloudMinScale, new GUIContent("Min Size"));
            EditorGUILayout.PropertyField(cloudMaxScale, new GUIContent("Max Size"));
            EditorGUILayout.PropertyField(cloudMaterial, true);
            EditorGUILayout.PropertyField(cloudShadowMaterial, true);
            EditorGUILayout.PropertyField(cloudColor, new GUIContent("Cloud Color"));
            EditorGUILayout.PropertyField(cloudLining, new GUIContent("Lining Color"));
            EditorGUILayout.PropertyField(cloudMinSpeed, new GUIContent("Min Speed"));
            EditorGUILayout.PropertyField(cloudMaxSpeed, new GUIContent("Max Speed"));
            EditorGUILayout.PropertyField(cloudRange, new GUIContent("Distance Travelled"));
            if (GUILayout.Button("Add Clouds"))
            {
                terrain.GenerateClouds();
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        showHeights = EditorGUILayout.Foldout(showHeights, "Height Map");
        if (showHeights)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            int hmtSize = (int)(EditorGUIUtility.currentViewWidth - 100);
            GUILayout.Label(hmTexture, GUILayout.Width(hmtSize), GUILayout.Height(hmtSize));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh", GUILayout.Width(hmtSize)))
            {
                float[,] heightMap = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution,
                                                                    terrain.terrainData.heightmapResolution);
                for (int y = 0; y < terrain.terrainData.alphamapHeight; y++)
                {
                    for (int x = 0; x < terrain.terrainData.alphamapWidth; x++)
                    {
                        hmTexture.SetPixel(x, y, new Color(heightMap[x, y], heightMap[x, y], heightMap[x, y], 1));
                    }
                }
                hmTexture.Apply();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Reset Terrain"))
        {
            terrain.ResetTerrain();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        //End
        serializedObject.ApplyModifiedProperties();
        //End
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}