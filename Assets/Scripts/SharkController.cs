using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SharkController : MonoBehaviour
{
    private GameObject playerOjbect;
    private PlayerControls playerControlsScript;
    private LevelController levelControllerScript;
    private float sharkSpeed = 1.0f;
    public bool gameOver = false;  
    public bool isMoving = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerOjbect = GameObject.Find("Boat");
        playerControlsScript = playerOjbect.GetComponent<PlayerControls>();
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }
    
    // Update is called once per frame
    void Update()
    {

        if (playerOjbect)
        {
            transform.position = new Vector3(playerOjbect.transform.position.x, this.transform.position.y, this.transform.position.z);        
        }
        else
        {
            // Debug.Log("Das pech, player weg");
        }

        if (playerControlsScript.hittedWall)
        {
            StartCoroutine(ToggleMovementAfterSeconds(1));
            playerControlsScript.hittedWall = false;
        }

        if ((isMoving || playerControlsScript.isBackwards) && !levelControllerScript.gameOver)
        {
            transform.Translate(Vector3.back * Time.deltaTime * sharkSpeed);  
        }
    }
  
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
            levelControllerScript.gameOver = true;            
        }
    }
}

