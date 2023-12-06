using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    int currentWaypoint = 0;
    [SerializeField] float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2.Distance((waypoints[currentWaypoint].transform.position), ((waypoints[(currentWaypoint + 1) % waypoints.Length].transform.position)));
        if(transform.position == waypoints[(currentWaypoint + 1) % waypoints.Length].transform.position )
        {
            Debug.Log("ZMIANA");
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
        else
        {
            Debug.Log("JAZDA");
            transform.position = Vector2.MoveTowards(transform.position, waypoints[(currentWaypoint + 1) % waypoints.Length].transform.position, speed * Time.deltaTime);
        }

    }
}
