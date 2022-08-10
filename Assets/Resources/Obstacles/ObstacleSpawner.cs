using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacleList = new List<GameObject>();
    public List<float> obstaclePositionYList = new List<float>();
    public List<GameObject> obstacleSpawnedList = new List<GameObject>();

    public GameObject player;

    public GameObject ceiling;
    public GameObject flooring;

    public float distanceBetweenObstacle;

    public int maxObstacleNum;

    public float destroyDistanceOffset;

    private List<int> randomNumSeens = new List<int>();

    public AnimationCurve randomCurve;

    void Start()
    {
        Object[] objects = Resources.LoadAll("Obstacles", typeof(GameObject));

        foreach(Object o in objects) 
        {
            GameObject gameObject = (GameObject) o;
            obstacleList.Add(gameObject);
        }

        obstaclePositionYList.Add(ceiling.transform.position.y - obstacleList[0].GetComponent<Renderer>().bounds.size.y / 2);
        obstaclePositionYList.Add(flooring.transform.position.y + obstacleList[1].GetComponent<Renderer>().bounds.size.y / 2);
        obstaclePositionYList.Add(ceiling.transform.position.y - obstacleList[2].GetComponent<Renderer>().bounds.size.y / 2);
    }
    void FixedUpdate()
    {
        SpawnObstacle();
        DestroyObstacle();
    }

    void SpawnObstacle()
    {
        if(obstacleSpawnedList.Count < maxObstacleNum)
        {
            float x = obstacleSpawnedList.Count == 0 ? player.transform.position.x : obstacleSpawnedList[obstacleSpawnedList.Count-1].transform.position.x;
            x += distanceBetweenObstacle;

            // 要產生亂數的範圍要在Inspector的AnimatorCurve調整
            int select = GetRandomNumberWithMaxRepaet(2);
            Vector3 v = new Vector3(x, obstaclePositionYList[select], 0);
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
            r = GetRandomNumberWithWeight();

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

    int GetRandomNumberWithWeight()
    {
        return (int)randomCurve.Evaluate(Random.Range(0, 11));
    }
}
