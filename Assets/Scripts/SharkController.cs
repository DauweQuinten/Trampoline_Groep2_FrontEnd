using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SharkController : MonoBehaviour
{
    private GameObject playerOjbect;
    private PlayerControls playerControlsScript;
    private bool playerhasCollided;
    private float sharkSpeed = 1.0f;

    public bool isMoving = false;
    

    // Start is called before the first frame update
    void Start()
    {
        playerOjbect = GameObject.Find("Boat");
        playerControlsScript = playerOjbect.GetComponent<PlayerControls>();
    }
    
    // Update is called once per frame
    void Update()
    {

        if (playerOjbect)
        {
            transform.position = new Vector3(playerOjbect.transform.position.x, this.transform.position.y, this.transform.position.z);
            playerhasCollided = playerControlsScript.hasCollided;

            if (playerhasCollided)
            {
                transform.Translate(Vector3.back * Time.deltaTime * sharkSpeed);  
                // StartCoroutine(ToggleMovementAfterSeconds(1));
            }              
        }else
        {
            Debug.Log("Das pech, player weg");
        }

        if (playerControlsScript.hittedWall)
        {
            StartCoroutine(ToggleMovementAfterSeconds(1));
            playerControlsScript.hittedWall = false;
        }

        

        if (isMoving)
        {
            transform.Translate(Vector3.back * (Time.deltaTime * sharkSpeed));       
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Blub");
    //        Destroy(collision.gameObject);
    //    }
    //}

    
    public IEnumerator ToggleMovementAfterSeconds(int seconds){
        isMoving = true;
        yield return new WaitForSeconds(seconds);
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Blub");
            Destroy(other.gameObject);
        }
    }
}

