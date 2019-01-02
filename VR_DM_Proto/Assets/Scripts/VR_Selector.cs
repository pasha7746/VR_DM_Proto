using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Selector : MonoBehaviour
{
    public float castDistance;
    public RaycastHit hit;
    private SelectionVisual lastSucRayHit;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		DoCast();
	}

    public void DoCast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, castDistance);
        Debug.DrawLine(transform.position, transform.position+transform.forward*castDistance, Color.red, Time.deltaTime); //debug

        if(!hit.collider) return;
        TryToPopAnimation(hit.collider.GetComponentInParent<SelectionVisual>());

        if (hit.collider.isTrigger) return;
        



    }

    public void TryToPopAnimation(SelectionVisual tempSelectionVisual)
    {
        if (!tempSelectionVisual)
        {
            if (lastSucRayHit != null) lastSucRayHit.PopDown();
            lastSucRayHit = null;
            return;
        }

        if (tempSelectionVisual != lastSucRayHit)
        {
            lastSucRayHit = tempSelectionVisual;
            tempSelectionVisual.PopUp();
        }
        
       

    }
}
