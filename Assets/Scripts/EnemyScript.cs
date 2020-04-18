using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyScript : MonoBehaviour
{
    public GameObject target;
    public GameObject[] points = new GameObject[3];
    public NavMeshAgent agent;
    public int destPos;
    public bool move;
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                target = GameObject.FindGameObjectWithTag("Player");
                move = true;
            }
        }
        catch
        {

        }
        if (move == true)
        {
            if ((target.transform.position - gameObject.transform.position).magnitude < 15f)
            {
                agent.SetDestination(target.transform.position);
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    GotoNextposition();
                }
            }
        }
    }
    void GotoNextposition()
    {
        if (points.Length == 0)
        {
            return;
        }
        agent.destination = points[destPos].transform.position;
        destPos = (destPos + 1) % points.Length;

    }
}
