using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketMessage : MonoBehaviour
{
    // public JumpMessage jump { get; set; }

	private JumpMessage jump;

	public JumpMessage Jump
	{
		get { return jump; }
		set { jump = value; }
	}


    public ButtonMessage button { get; set; }

    //private ButtonMessage button;

    //public ButtonMessage Button
    //{
    //    get { return button; }
    //    set { button = value; }
    //}
}