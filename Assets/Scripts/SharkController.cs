using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SharkController : MonoBehaviour
{
    private GameObject playerOjbect;
    private bool playerhasCollided;
    private float sharkSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerOjbect = GameObject.Find("Boat");
    }
    
    // Update is called once per frame
    void Update()
    {

        if (playerOjbect)
        {
            transform.position = new Vector3(playerOjbect.transform.position.x, this.transform.position.y, this.transform.position.z);
            playerhasCollided = playerOjbect.GetComponent<PlayerControls>().hasCollided;

            if (playerhasCollided)
            {
                transform.Translate(Vector3.back * Time.deltaTime * sharkSpeed);            
            }              
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Blub");
            Destroy(collision.gameObject);
        }
    }
}

