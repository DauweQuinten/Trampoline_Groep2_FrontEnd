using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    private LevelController levelControllerScript;

    private float speed = 10f;
    public float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }



    // Update is called once per frame
    void Update()
    {
        //move the player left or right
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
    }


    // On collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            levelControllerScript.ScrollState = false;
        }
    }
}
