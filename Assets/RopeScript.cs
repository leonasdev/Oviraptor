using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector2 destiny;

    public float speed = 1;

    public float distance = 2;

    public GameObject nodePrefab;

    public GameObject player;

    public GameObject lastNode;

    bool done = false;

    public List<GameObject> Nodes = new List<GameObject>();

    int vertexCount = 2;

    public LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");

        lastNode = transform.gameObject;

        Nodes.Add(transform.gameObject);
    }

    void FixedUpdate()
    {
        // Hook位置從Player直線移動到終點
        transform.position = Vector2.MoveTowards(transform.position, destiny, speed * Time.deltaTime);

        // 建立player到hook之間的node
        if ((Vector2)transform.position != destiny)
        {
            if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }
        else if (done == false)
        {
            done = true;

            while (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }

            lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }

        RenderLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RenderLine()
    {
        lr.positionCount = vertexCount;

        int i = 0;
        for (i = 0; i < Nodes.Count; i++)
        {
            lr.SetPosition(i, Nodes[i].transform.position);
        }

        lr.SetPosition(i, player.transform.position);

    }

    void CreateNode()
    {
        Vector2 pos2Create = player.transform.position - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= distance;
        pos2Create += (Vector2)lastNode.transform.position;

        GameObject go = Instantiate(nodePrefab, pos2Create, Quaternion.identity);

        go.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

        lastNode = go;

        Nodes.Add(lastNode);

        vertexCount++;
    }
}
