using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SharkController : MonoBehaviour
{
    private GameObject playerOjbect;
    private bool playerhasCollided;
    private readonly float sharkSpeed = 1.0f;
    private PlayerControls _playerControls;

    // Start is called before the first frame update
    void Start()
    {
        _playerControls = playerOjbect.GetComponent<PlayerControls>();
        playerOjbect = GameObject.Find("Boat");
    }
    
    // Update is called once per frame
    void Update()
    {

        if (playerOjbect)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(playerOjbect.transform.position.x, position.y, position.z);
            transform1.position = position;
            playerhasCollided = _playerControls.hasCollided;

            if (playerhasCollided)
            {
                transform.Translate(Vector3.back * (Time.deltaTime * sharkSpeed));            
            }              
        }else
        {
            // Debug.Log("Das pech, player weg");
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Blub");
            Destroy(other.gameObject);
        }
    }
}

