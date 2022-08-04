using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacleList = new List<GameObject>();
    public Queue<GameObject> obstacleSpawnedList = new Queue<GameObject>();

    public GameObject player;

    void Start()
    {
        Object[] objects = Resources.LoadAll("Obstacles", typeof(GameObject));

        foreach(Object o in objects) 
        {
            GameObject gameObject = (GameObject) o;
            obstacleList.Add(gameObject);
        }
    }
    void FixedUpdate()
    {
        if(obstacleSpawnedList.Count < 10){
            SpawnRandomObstacle();
        }
        else if (Vector3.Distance(obstacleSpawnedList.Peek().transform.position, player.transform.position) > 60)
        {
            DestroyObstacle();
        }
    }

    void SpawnRandomObstacle()
    {
        int randomNum = Random.Range(0, 3);
        Vector3 targetPos = new Vector3(player.transform.position.x + 20 * obstacleSpawnedList.Count, 20, 0);

        GameObject obstacle = Instantiate(obstacleList[randomNum], targetPos, Quaternion.identity);

        obstacleSpawnedList.Enqueue(obstacle);
    }

    void DestroyObstacle()
    {
        Destroy(obstacleSpawnedList.Dequeue());
    }
}
