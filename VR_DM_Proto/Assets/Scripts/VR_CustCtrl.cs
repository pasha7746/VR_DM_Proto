using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_CustCtrl : MonoBehaviour
{
    public SteamVR_Action_Single clickTrigger;
    public event Action OnTriggerClick;




    // Use this for initialization
    void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		OnClickMainTrigger();
	}

    public void OnClickMainTrigger()
    {
        if (SteamVR_Input.__actions_default_in_TriggerClick.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (OnTriggerClick != null) OnTriggerClick();
        }
    }
}
