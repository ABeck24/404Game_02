using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDetection: MonoBehaviour {
    public GameObject spotLight;
    public Transform player;
    public int deceleration;
    public GameObject bullet;
    public GameObject bulletClone;
    public float shootSpeed;
    public Transform shotSpawn;
    public bool isShooting;
    private Vector3 drone;
    private float shooting;
    private float chaseLength;
    public float shootTimer;
    private IEnumerator movementReset;
    public IEnumerator shockReset;
    float savedSpeed;
    float savedTimer;
    public float downTime;
    public bool hitShocked;
    bool disabled;
    public float shootLength;
    float storedLength;


    private void Start()
    {
        spotLight.GetComponent<Light>().color = Color.blue;
        savedSpeed = this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed;
        savedTimer = shootTimer;
        storedLength = shootLength;
       
    }
    private void Update()
    {

    }
    void OnTriggerEnter (Collider col)
    {
        //Checking if the player is detected by the drone
        if (col.gameObject.name == "Player" && disabled == false) 
        {
            shootLength = storedLength;
            this.GetComponent<AudioSource>().Play();
            lostPlayer(false);
            spotLight.GetComponent<Light>().color = Color.red;
            //Debug.Log("Player Spotted");
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "Player" && disabled == false)
        {
            Vector3 rotateDrone = Vector3.RotateTowards(transform.forward, drone, 1, 0.0f);
            //Debug.DrawRay(transform.position, rotateDrone, Color.red);

            transform.rotation = Quaternion.LookRotation(rotateDrone);
            //this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
            drone = player.position - transform.position;
            chaseLength = Vector3.Distance(this.transform.position, player.position);
            shootTimer -= Time.deltaTime * 5;
            if (shootTimer <= 0)
            {
                isShooting = true;
            }
            if (this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed > 0)
            {
                this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed -= Time.deltaTime * deceleration;
                //Debug.Log(timer);   
            }
            if (Time.time > shooting && isShooting == true)
            {
                //Debug.Log("Attacking");
                shooting = Time.time + shootSpeed;
                bulletClone = Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
                shootLength -= Time.deltaTime * 5;
                if(shootLength <= 0)
                {
                    
                    movementReset = lostPlayer(true);
                    StartCoroutine(movementReset);
                    
                }
            }

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            isShooting = false;
            shootTimer = savedTimer;
            movementReset = lostPlayer(true);
            spotLight.GetComponent<Light>().color = Color.blue;
            StartCoroutine(movementReset);
        }

    }

    private IEnumerator lostPlayer(bool lost)
    {
        if (lost == true) {
            isShooting = false;
            shootTimer = savedTimer;
            spotLight.GetComponent<Light>().color = Color.blue;
            yield return new WaitForSeconds(savedTimer);
            shootLength = storedLength;
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = savedSpeed;
        }
        if(lost == false)
        {
            if (this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed > 0)
            {
                yield return this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed -= Time.deltaTime;
            }
        }
        /* while (done == false)
        {
            if (shootTimer <= savedTimer)
            {
                shootTimer += Time.deltaTime * 5;
            }
            if (shootTimer >= savedTimer)
            {
                done = true;
                this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = savedSpeed;
            }
            yield return null;

        }*/
    }

    public IEnumerator DisableDrone()
    {
        this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        spotLight.SetActive(false);
        disabled = true;
        yield return new WaitForSeconds(downTime);
        spotLight.SetActive(true);
        disabled = false;
        this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = savedSpeed;
        /* (reset == false)
        {
            while (timer > 0)
            {
                this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
                transform.rotation = Quaternion.LookRotation(Vector3.down / 2);
                timer -= Time.deltaTime;

            }
                reset = true;
        }

        while (reset == true && timer <= downTime){
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = savedSpeed;
            transform.rotation = Quaternion.LookRotation(Vector3.up/2);
            timer += Time.deltaTime;
            hitShocked = false;
        }
        yield return null;
        */
    }
}
