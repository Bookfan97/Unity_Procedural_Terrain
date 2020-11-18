using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]

public class CustomTerrain : MonoBehaviour 
{

    public Vector2 randomHeightRange = new Vector2(0, 0.1f);
    public Texture2D heightMapImage;
    public Vector3 heightMapScale = new Vector3(1, 1, 1);

    public bool resetTerrain = true;

    //Perlin Noise
    public float perlinXScale = 0.01f;
    public float perlinYScale = 0.01f;
    public int perlinOffsetX = 0;
    public int perlinOffsetY = 0;
    public int perlinOctaves = 3;
    public float perlinPersistance = 8;
    public float perlinHeightScale = 0.09f;

    //Multiple Perlin
    [System.Serializable]
    public class PerlinParameters
    {
        public float mPerlinXScale = 0.01f;
        public float mPerlinYScale = 0.01f;
        public int mPerlinOctaves = 3;
        public float mPerlinPersistance = 8;
        public float mPerlinHeightScale = 0.09f;
        public int mPerlinOffsetX = 0;
        public int mPerlinOffsetY = 0;
        public bool remove = false;
    }

    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
        new PerlinParameters()
    };
    
    //Voronoi
    public float voronoiFallOff = 0.2f;
    public float voronoiDropOff = 0.6f;
    public float voronoiMinHeight = 0.1f;
    public float voronoiMaxHeight = 0.5f;
    public int voronoiPeaks = 5;
    public enum VoronoiType
    {
        Linear = 0,
        Power = 1,
        Combined = 2,
        SinPow = 3
    }

    public VoronoiType voronoiType = VoronoiType.Linear;

    public Terrain terrain;
    public TerrainData terrainData;
    
    float[,] GetHeightMap()
    {
        if (!resetTerrain)
        {
            return terrainData.GetHeights(0, 0, terrainData.heightmapResolution,
                                                terrainData.heightmapResolution);
        }
        else
            return new float[terrainData.heightmapResolution,
                             terrainData.heightmapResolution];
    }

    public void MidPointDisplacement()
    {
        float[,] heightMap = GetHeightMap();
        int width = terrainData.heightmapResolution - 1;
        int squareSize = width;
        int cornerX, cornerY;
        int midX, midY;
        int pmidXL, pmidXR, pmidYU, pmidYD;
        float height = (float) squareSize / 2.0f * 0.01f;
        float roughness = 2.0f;
        float heightDampner = (float) Mathf.Pow(2, -1 * roughness);

        heightMap[0, 0] = UnityEngine.Random.Range(0f, 0.2f);
        heightMap[0, terrainData.heightmapResolution - 2] = UnityEngine.Random.Range(0f, 0.2f);
        heightMap[terrainData.heightmapResolution - 2, 0] = UnityEngine.Random.Range(0f, 0.2f);
        heightMap[terrainData.heightmapResolution - 2, terrainData.heightmapResolution - 2] = UnityEngine.Random.Range(0f, 0.2f);
        while (squareSize > 0)
        {
            for (int x = 0; x < width; x += squareSize)
            {
                for (int y = 0; y < width; y += squareSize)
                {
                    cornerX = (x + squareSize);
                    cornerY = (y + squareSize);
                    midX = (int) (x + squareSize / 2.0f);
                    midY = (int) (y + squareSize / 2.0f);
                    heightMap[midX, midY] = (float) (
                        (
                            heightMap[x, y] +
                            heightMap[cornerX, y] +
                            heightMap[x, cornerY] +
                            heightMap[cornerX, cornerY]
                        ) / 4.0f + UnityEngine.Random.Range(-height, height));
                }
            }
            squareSize = (int) (squareSize / 2.0f);
            height *= heightDampner;
        }
        terrainData.SetHeights(0,0, heightMap);
    }
    
    
    public void Voronoi()
    {
        float[,] heightMap = GetHeightMap();
        for (int p = 0; p < voronoiPeaks; p++)
        {
            //Vector3 peak = new Vector3(terrainData.heightmapResolution/2, 0.5f, terrainData.heightmapResolution/2);
            Vector3 peak = new Vector3(
                UnityEngine.Random.Range(0, terrainData.heightmapResolution),
                UnityEngine.Random.Range(voronoiMinHeight, voronoiMaxHeight),
                UnityEngine.Random.Range(0, terrainData.heightmapResolution)
            );
            if (heightMap[(int) peak.x, (int) peak.z] < peak.y)
            {
                heightMap[(int) peak.x, (int) peak.z] = peak.y;
            }
            else
            {
                continue;
            }
            Vector2 peakLocation = new Vector2(peak.x, peak.z);
            float maxDistance = Vector2.Distance(
                new Vector2(0, 0),
                new Vector2(terrainData.heightmapResolution, terrainData.heightmapResolution)
            );
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                for (int x = 0; x < terrainData.heightmapResolution; x++)
                {
                    if (!(x == peak.x && y == peak.z))
                    {
                        float distanceToPeak = Vector2.Distance(
                            peakLocation,
                            new Vector2(x, y)) / maxDistance;
                        float h;
                        if (voronoiType == VoronoiType.Combined)
                        {
                            h = peak.y - distanceToPeak * voronoiFallOff - Mathf.Pow(distanceToPeak, voronoiDropOff);
                        }
                        else if(voronoiType == VoronoiType.Power)
                        {
                            h = peak.y - Mathf.Pow(distanceToPeak, voronoiDropOff) * voronoiFallOff;
                        }
                        else if(voronoiType == VoronoiType.SinPow)
                        {
                            h = peak.y - Mathf.Pow(distanceToPeak*3, voronoiFallOff) - Mathf.Sin(distanceToPeak*2*Mathf.PI)/voronoiDropOff;
                        }
                        else
                        {
                            h = peak.y - distanceToPeak * voronoiFallOff;
                        }
                        if (heightMap[x,y]<h)
                        {
                            heightMap[x, y] = h;                            
                        }
                    }
                }
            }
        }
        terrainData.SetHeights(0,0, heightMap);
    }
    
    public void Perlin()
    {
        float[,] heightMap = GetHeightMap();

        for (int y = 0; y < terrainData.heightmapResolution; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                heightMap[x, y] += Utils.fBM(
                    (x+perlinOffsetX) * perlinXScale, 
                    (y+perlinOffsetY) * perlinYScale, 
                    perlinOctaves, 
                    perlinPersistance) * perlinHeightScale;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void MultiplePerlinTerrain()
    {
        float[,] heightMap = GetHeightMap();
        for (int y = 0; y < terrainData.heightmapResolution; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                foreach (PerlinParameters p in perlinParameters)
                {
                    heightMap[x, y] += Utils.fBM((x + p.mPerlinOffsetX) * p.mPerlinXScale,
                                                 (y + p.mPerlinOffsetY) * p.mPerlinYScale,
                                                    p.mPerlinOctaves,
                                                    p.mPerlinPersistance) * p.mPerlinHeightScale;
                }
            }
        }
        terrainData.SetHeights(0,0, heightMap);
    }

    public void AddNewPerlin()
    {
        perlinParameters.Add(new PerlinParameters());
    }

    public void RemovePerlin()
    {
        List<PerlinParameters> keptPerlinParameters = new List<PerlinParameters>();
        for (int i = 0; i < perlinParameters.Count; i++)
        {
            if (!perlinParameters[i].remove)
            {
                keptPerlinParameters.Add(perlinParameters[i]);
            }
        }
        if (keptPerlinParameters.Count == 0) //don't want to keep any
        {
            keptPerlinParameters.Add(perlinParameters[0]); //add at least 1
        }
        perlinParameters = keptPerlinParameters;
    }

    public void RandomTerrain()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += UnityEngine.Random.Range(randomHeightRange.x, randomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    
    }
    public void ResetTerrain()
    {
        float[,] heightMap;
        heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                heightMap[x, z] = 0;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void LoadTexture()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += heightMapImage.GetPixel((int)(x * heightMapScale.x), 
                                                          (int)(z * heightMapScale.z)).grayscale 
                                                            * heightMapScale.y;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    void OnEnable()
    {
        Debug.Log("Initialising Terrain Data");
        terrain = this.GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;
    }

    void Awake()
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
            );
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        AddTag(tagsProp, "Terrain");
        AddTag(tagsProp, "Cloud");
        AddTag(tagsProp, "Shore");
        tagManager.ApplyModifiedProperties();
        this.gameObject.tag = "Terrain";
    }

    void AddTag(SerializedProperty tagsProp, string newTag)
    {
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag)) { found = true; break; }
        }
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
            newTagProp.stringValue = newTag;
        }
	}
}
