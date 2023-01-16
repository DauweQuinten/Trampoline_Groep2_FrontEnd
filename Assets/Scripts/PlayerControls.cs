using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    private LevelController levelControllerScript;

    private float speed;
    private float xBounds = 3.5f;

    //public float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        // start coroutines
        StartCoroutine(SlowDown());

        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }



    // Update is called once per frame
    void Update()
    {
        // Get input from players
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            speed -= 1f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            speed += 1f;
        }


        //move the player left or right
        //horizontalInput = Input.GetAxis("Horizontal");     
        transform.Translate(Vector3.right * Time.deltaTime * speed);    

              
        

                   
        // position constraints
        if(transform.position.x > xBounds)
        {
            transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
        }
        
        if (transform.position.x < -xBounds)
        {
            transform.position = new Vector3(-xBounds, transform.position.y, transform.position.z);
        }
    }

    // Coroutine to slow down the player
    IEnumerator SlowDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (speed != 0)
            {
                if (speed > 0)
                {
                    speed -= 1;
                }
                else if (speed < 0)
                {
                    speed += 1;
                }
            }
        }
    }

    
    // On collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            levelControllerScript.ScrollState = false;
        }    
    }

    // On collision exit
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            levelControllerScript.ScrollState = true;
        }
    }
}
