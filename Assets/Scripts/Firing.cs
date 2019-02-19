using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Firing : MonoBehaviour {
    public float speed;
    public Rigidbody rb;
    AudioSource fire;
    private GameObject playerTarget;
    private Transform playerPosition;
    

    void Start()
    {
        this.GetComponent<AudioSource>().Play();
        playerTarget = GameObject.Find("Player");
        transform.LookAt(playerTarget.transform.position);
        rb.velocity = transform.forward * speed;
    }


    void FixedUpdate()
    {
        // this.transform.position = Vector3.MoveTowards(this.transform.position, playerTarget.transform.position, speed);
        //if(timeAlive > 5)
        Destroy(gameObject, 4f);



    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            GameObject.Find("Drone").GetComponent<PlayerDetection>().isShooting = false;
            GameObject.Find("Player").GetComponent<GameController>().Death();
            //Time.timeScale = 0.0f;


        }
        //Destroy(other.gameObject);
        
    }
}
