using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public bool EnableSpawn;

    public List<GameObject> obstacleList = new List<GameObject>();
    public List<float> obstaclePositionYList = new List<float>();
    public List<GameObject> obstacleSpawnedList = new List<GameObject>();

    public GameObject player;

    public GameObject ceiling;
    public GameObject flooring;

    public float startX;

    public float minDistanceBetweenObstacle;

    public float minXDistanceRandom;
    public float maxXDistanceRandom;

    public float minYDistanceRandom;
    public float maxYDistanceRandom;

    public int maxObstacleNum;

    public float destroyDistanceOffset;

    private List<int> randomNumSeens = new List<int>();

    public AnimationCurve selectRandomCurve;

    public AnimationCurve yDistanceRandomCurve;

    public List<int> debugList = new List<int>();
    public int debugCount;

    void Start()
    {
        obstaclePositionYList.Add(ceiling.transform.position.y - obstacleList[0].GetComponent<Renderer>().bounds.size.y / 2);
        obstaclePositionYList.Add(flooring.transform.position.y + obstacleList[1].GetComponent<Renderer>().bounds.size.y / 2);
        obstaclePositionYList.Add(ceiling.transform.position.y - obstacleList[2].GetComponent<Renderer>().bounds.size.y / 2);
    }
    void FixedUpdate()
    {
        if(EnableSpawn) {
            SpawnObstacle();
            DestroyObstacle();
        }
        // int s = GetRandomNumberWithMaxRepaet(2);
        // debugCount++;
        // debugList.Add(s);
        // Debug.Log("0: " + (float)debugList.Count(v => v == 0) / (float)debugCount);
        // Debug.Log("1: " + (float)debugList.Count(v => v == 1) / (float)debugCount);
        // Debug.Log("2: " + (float)debugList.Count(v => v == 2) / (float)debugCount);
    }

    void SpawnObstacle()
    {
        if(obstacleSpawnedList.Count < maxObstacleNum)
        {
            // 要產生亂數的範圍要在Inspector的AnimatorCurve調整
            int select = GetRandomNumberWithMaxRepaet(2);

            float x = obstacleSpawnedList.Count == 0 ? player.transform.position.x + startX : obstacleSpawnedList[obstacleSpawnedList.Count-1].transform.position.x;
            x += minDistanceBetweenObstacle;
            x += Random.Range(minXDistanceRandom, maxXDistanceRandom);

            float y = obstaclePositionYList[select];
            switch (select)
            {
                case 0:
                    y += GetYDistanceRandomNumberWithWeight();
                    break;
                case 1:
                    y -= GetYDistanceRandomNumberWithWeight();
                    break;
                default:
                    break;
            }

            Vector3 v = new Vector3(x, y, 0);
            GameObject newObstacle = (GameObject)Instantiate(obstacleList[select], v, Quaternion.identity);
            obstacleSpawnedList.Add(newObstacle);
        }
    }

    void DestroyObstacle()
    {
        GameObject oldestObstacle = obstacleSpawnedList[0];
        if(player.transform.position.x > oldestObstacle.transform.position.x + oldestObstacle.GetComponent<Renderer>().bounds.size.x + destroyDistanceOffset)
        {
            Destroy(oldestObstacle);
            obstacleSpawnedList.RemoveAt(0);
        }
    }

    int GetRandomNumberWithMaxRepaet(int maxRepeatNum)
    {
        int r;

        while(true)
        {
            r = GetSelectRandomNumberWithWeight();

            if(!randomNumSeens.Contains(r))
            {
                randomNumSeens.Clear();
                randomNumSeens.Add(r);
                return r;
            }
            else if(randomNumSeens.Count(v => v == r) == maxRepeatNum)
            {
                continue;
            }
            else
            {
                randomNumSeens.Add(r);
                return r;
            }
        }
    }

    int GetSelectRandomNumberWithWeight()
    {
        // 這邊要跟SelectRandomCurve一起動
        int lastPointTime = (int)selectRandomCurve.keys[selectRandomCurve.keys.Length - 1].time;
        return (int)selectRandomCurve.Evaluate(Random.Range(0, lastPointTime + 1));
    }

    float GetYDistanceRandomNumberWithWeight()
    {
        // 這邊要跟YDistanceRandomCurve一起動
        float lastPointTime = yDistanceRandomCurve.keys[yDistanceRandomCurve.keys.Length - 1].time;
        return (float)yDistanceRandomCurve.Evaluate(Random.Range(0, lastPointTime));
    }

    private void DestroyAll()
    {
        foreach(GameObject o in obstacleSpawnedList) {
            Destroy(o);
        }
    }

    public void Reset()
    {
        DestroyAll();
        obstacleSpawnedList.Clear();
    }
}
