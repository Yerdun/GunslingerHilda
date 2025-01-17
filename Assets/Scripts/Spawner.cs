using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public MapGenerator mapGen;
    public MeshFilter meshVert;
    public GameObject player;
    public GameObject testSpawn;

    public int amountTestObj;
    public bool runTestSpawn;

    private float[,] heightMap;
    private Vector3[] points;
    private List<Vector3> validPoints;

    [Range(0,1)]
    public float spawnGroundLevel;

    // Start is called before the first frame update
    void Start()
    {
        // Obtain noise map for comparison
        heightMap = mapGen.GenerateMapData(Vector2.zero).heightMap;
        // Gets directly from mesh object
        points = meshVert.mesh.vertices;

        // Gets list of valid points
        validPoints = ValidLocations(points, spawnGroundLevel);
        Debug.Log("NOISE MAP SIZE: " + (heightMap.GetLength(0) * heightMap.GetLength(1)));
        Debug.Log("POINTS ARRAY SIZE: " + points.Length);
        Debug.Log("VALID POINTS SIZE: " + validPoints.Count);

        // Sets player location
        SpawnEntity(player, points);

        // Debugging to check where the spawn points are
        if (runTestSpawn)
        {
            for (int i = 0; i < amountTestObj; i++)
            {
                SpawnEntity(testSpawn, points);
            }
        }
    }

    private void SpawnEntity(GameObject entity, Vector3[] points)
    {

        // int randomPoint = Random.Range(0, points.Length);

        // Vector3 selectedPoint = points[randomPoint];

        int randomPoint = Random.Range(0, validPoints.Count);

        Vector3 selectedPoint = validPoints[randomPoint];
        Debug.Log("Location: " + selectedPoint);

        // If player just change location, that way you dont need to readd everything
        // Else just spawn object
        if (entity.gameObject.tag == "Player")
        {
            entity.transform.position = selectedPoint * mapGen.terrainData.uniformScale;
        }
        else
        {
            GameObject instObj = Instantiate(entity);
            instObj.transform.position = selectedPoint * mapGen.terrainData.uniformScale;
        }
        
    }

    // Looks through each pixel on the noise map and sees if its less than ground level value
    // If less then add corresponding vertex to the valid location list
    private List<Vector3> ValidLocations(Vector3[] points, float groundLevel)
    {
        List<Vector3> validPoints = new List<Vector3>();

        int row = heightMap.GetLength(0);
        int col = heightMap.GetLength(1);

        for (int i = 0; i < points.Length; i++)
        {
            float canyonRatio = points[i].y / mapGen.terrainData.meshHeightMultipler;

            if (canyonRatio < groundLevel)
            {
                validPoints.Add(points[i]);
            }
        }

        // Old ALGO didnt work
        // for (int i = 0; i < row; i++)
        // {
        //     for (int j = 0; j < col; j++)
        //     {
        //         if (heightMap[i, j] < groundLevel)
        //         {
        //             validPoints.Add(points[(row * i) + j]);
        //             // validPoints.Add(points[(col * j) + i]);
        //         }
        //     }
        // }

        return validPoints;
    }
}
