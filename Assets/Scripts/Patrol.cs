using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    //setting up drone and drone movement
    public Vector3 nextPoint;
    private GameObject checkPointsValue;
    public GameObject[] checkPoints;
    int goalMax;
    private UnityEngine.AI.NavMeshAgent drone;

    void Start()
    {
        //defining the checkpoint values
        goalMax = checkPoints.Length;
        checkPointsValue = checkPoints[Random.Range(0, goalMax)];
        nextPoint = checkPointsValue.transform.position;
       
        //finding the drone and beginning it's movement
        drone = GetComponent<UnityEngine.AI.NavMeshAgent>();
        drone.destination = nextPoint;
    }
    void Update()
    {
        //telling the drone where to go next
        if(drone.transform.position.x == nextPoint.x)
        {
            new WaitForSeconds(2);
            checkPointsValue = checkPoints[Random.Range(0, goalMax)];
            nextPoint = checkPointsValue.transform.position;
        }
        //updating the drones movement
        drone.destination = nextPoint;
    }
}