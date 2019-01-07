using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;

public class BoardBuilder : MonoBehaviour
{
    public int numberOfTiles;
    public GameObject tile;
    public Vector3[] pos;
    public event Action OnCompleteBuild;
    private GameObject[] tempTestStore;
    private UserData[] l_SpawnedTiles;
   // private List<VideoPlayer> l_VPlayer;

    void Awake()
    {
        BuildHoldTiles(numberOfTiles);

    }

    // Use this for initialization
    void Start ()
	{
	   

    }
	
	// Update is called once per frame
	void Update ()
	{
	 
    }
    public void RunVideoPlayCheck(UserData data)
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (data.videoPlayer != l_SpawnedTiles[i].videoPlayer)
            {
                l_SpawnedTiles[i].StopVideo();
            }

        }
        
        
    }

    public IEnumerator WaitTimer()
    {
        yield return new WaitForEndOfFrame();
        BuildRealTiles();
        GetComponentInParent<Canvas>().enabled = false;
        for (int i = 0; i < numberOfTiles; i++)
        {
            l_SpawnedTiles[i].Setup();
            l_SpawnedTiles[i].OnTriggerVideo += RunVideoPlayCheck;
            Destroy(tempTestStore[i]);
        }

        //for (int i = 0; i < numberOfTiles; i++)
        //{
        //    l_VPlayer.Add(l_SpawnedTiles[i].videoPlayer);
        //}


        yield return null;
    }
  

    public void BuildRealTiles()
    {
       
        for (int i = 0; i < tempTestStore.Length; i++)
        {
            pos[i] = tempTestStore[i].transform.position;
        }
        GameObject tempInstance;
        l_SpawnedTiles= new UserData[numberOfTiles];
        for (int i = 0; i < numberOfTiles; i++)
        {
            tempInstance = Instantiate(tile);
            tempInstance.transform.position = pos[i];
            tempInstance.transform.parent = transform;
            l_SpawnedTiles[i] = tempInstance.GetComponent<UserData>();
        }
    }

    public void BuildHoldTiles(int numberOfTiles)
    {
        tempTestStore= new GameObject[numberOfTiles];
        GameObject tempInstance;
         pos= new Vector3[numberOfTiles];
        for (int i = 0; i < numberOfTiles; i++)
        {

            tempInstance= new GameObject();
            tempInstance.AddComponent<Image>();
            tempInstance.transform.parent = transform;
            tempInstance.transform.localScale = Vector3.one;
            
            tempTestStore[i] = tempInstance;
            
        }

        StartCoroutine(WaitTimer());


    }
}
