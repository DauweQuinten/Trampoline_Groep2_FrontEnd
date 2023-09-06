using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SharkController : MonoBehaviour
{
    #region Variables

    // references to other scripts and objects
    private GameObject playerOjbect;
    private PlayerControls playerControlsScript;
    private LevelController levelControllerScript;

    // variables for shark movement
    private float sharkSpeed = 1.0f;

    // shark states
    [HideInInspector] public bool gameOver = false;  
    [HideInInspector] public bool isMoving = false;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Get reference to other scripts and objects
        playerOjbect = GameObject.Find("Boat");
        playerControlsScript = playerOjbect.GetComponent<PlayerControls>();
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
        // Follow the player if the player is moving 
        if (playerOjbect)
        {
            transform.position = new Vector3(playerOjbect.transform.position.x, this.transform.position.y, this.transform.position.z);        
        }
        else
        {
            // Debug.Log("Das pech, player weg");
        }

        // Move forward if the player hitted the wall
        if (playerControlsScript.hittedWall)
        {
            StartCoroutine(ToggleMovementAfterSeconds(1));
            playerControlsScript.hittedWall = false;
        }

        // Shark forward movement
        if ((isMoving || playerControlsScript.isBackwards) && !levelControllerScript.gameOver)
        {
            transform.Translate(Vector3.back * Time.deltaTime * sharkSpeed, Space.World);  
        }
    }

    // Toggle shark movement during a certain time
    public IEnumerator ToggleMovementAfterSeconds(int seconds){
        isMoving = true;
        yield return new WaitForSeconds(seconds);
        isMoving = false;
    }

    // Shark collision with player == game over
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Blub");
            Destroy(other.gameObject);
            levelControllerScript.gameOver = true;            
        }
    }
}

