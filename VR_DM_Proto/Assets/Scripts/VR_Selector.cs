using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Selector : MonoBehaviour
{
    public float castDistance;
    public RaycastHit hit;
    private SelectionVisual lastSucRayHit;
    private VR_CustCtrl myCtrl;
    private UserData selectedUserData;

    void Awake()
    {
        myCtrl = GetComponentInParent<VR_CustCtrl>();
    }


    // Use this for initialization
	void Start ()
	{
	    myCtrl.OnTriggerClick += EventTileClick;
	}
	
	// Update is called once per frame
	void Update ()
	{
		DoCast();
	}

    public void EventTileClick()
    {
        if(!selectedUserData) return;
       selectedUserData.Clicked();

    }


    public void DoCast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, castDistance);
        Debug.DrawLine(transform.position, transform.position+transform.forward*castDistance, Color.red, Time.deltaTime); //debug

        if(!hit.collider) return;
       
        TryToPopAnimation(hit.collider.GetComponentInParent<SelectionVisual>());

        if (hit.collider.isTrigger) return;

        CheckIfUserDataTile(hit.collider.GetComponent<UserData>());
        



    }

    public void CheckIfUserDataTile(UserData data)
    {
        if(!data) return;
        selectedUserData = data.GetComponent<UserData>();
        
    }

    public void TryToPopAnimation(SelectionVisual tempSelectionVisual)
    {
       
        if (!tempSelectionVisual)
        {
            if (lastSucRayHit != null) lastSucRayHit.PopDown();
            lastSucRayHit = null;
            return;
        }

        if (tempSelectionVisual != lastSucRayHit )
        {
            if (lastSucRayHit != null) lastSucRayHit.PopDown();
            lastSucRayHit = tempSelectionVisual;
              tempSelectionVisual.PopUp();
            
        }
        
       

    }
}
