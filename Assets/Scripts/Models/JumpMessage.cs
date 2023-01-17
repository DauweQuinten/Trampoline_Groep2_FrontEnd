using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMessage : MonoBehaviour
{
    private string index;
    
    public string Index
    {
        get { return index; }
        set { index = value; }
    }

   
    private float force;
    
    public float Force
    {
        get { return force; }
        set { force = value; }
    }


    // public string Index { get; set; }

    // public float Force { get; set; }
}
