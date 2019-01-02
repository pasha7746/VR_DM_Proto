using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SelectionVisual : MonoBehaviour
{
    public Transform pointBRef;
    public GameObject movementChildGroup;
    private Vector3 pointA, pointB;
    public Ease movementEase;
    private Tween popTween;
    public float popDuration;

	// Use this for initialization
	void Start ()
	{
	    pointA = transform.position;
	    pointB = pointBRef.transform.position;


	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PopUp();
        }
    }

    public void PopUp()
    {
       popTween= movementChildGroup.transform.DOMove(pointB, popDuration).SetEase(movementEase);
       //popTween.onComplete += PopDown;
    }

    public void PopDown()
    {
        if (popTween.active)
        {
            popTween.Kill(false);
        }

        popTween = movementChildGroup.transform.DOMove(pointA, popDuration).SetEase(movementEase);
    }
}
