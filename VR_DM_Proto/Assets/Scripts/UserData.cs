using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR;
using System.Data;
using Mono.Data.SqliteClient;



public class UserData : MonoBehaviour
{
    public Texture2D picture;
    private Canvas myCanvas;
    public RawImage rmimg;
    public AudioClip audioClip;
    public VideoPlayer videoPlayer;
    public string nameText;
    private AudioSource audioPlayer;
    private Text myText;
    public string imagePath;
    public string audioPath;
    public string videoPath;
    public event Action<UserData> OnTriggerVideo;
	public string path;

	public IDbConnection dbconn;
	public IDbCommand dbcmd;
	public IDataReader reader;

	public GameObject img;


	void Awake()
    {
	    //rmimg = GetComponentInChildren<RawImage>();
        audioPlayer = GetComponent<AudioSource>();
        myText = GetComponentInChildren<Text>();
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        myCanvas = GetComponentInChildren<Canvas>();
		
    }

    void Start()
    {
	    if (Application.platform != RuntimePlatform.Android)
	    {
		    path = Application.dataPath + "/StreamingAssets/PeopleDB.db";
	    }
	    else
	    {
		    path = Application.persistentDataPath + "/PeopleDB.db";
	    }
	    OpenDB("PeopleDB.db");
		print(path);
		videoPlayer.loopPointReached += (a) => { a.gameObject.SetActive(false); ; };
        videoPlayer.gameObject.SetActive(false);
    }

    void OnEnable()
    {
       
    }

    public void Setup()
    {
	    if (Application.platform != RuntimePlatform.Android)
	    {
		    path = Application.dataPath + "/StreamingAssets/PeopleDB.db";
	    }
	    else
	    {
		    path = Application.persistentDataPath + "/PeopleDB.db";
	    }
	    OpenDB("PeopleDB.db");
        LoadImage();
        LoadText();
    }

    // Update is called once per frame
    void Update ()
	{
	    
	}

    public void StopVideo()
    {
       
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false);
        }
    }


    public void Clicked()
    {
        PlaySound(audioPath);
      
        LoadVideo(videoPath);
    }

	public void OpenDB(string p)
	{
		if (!File.Exists(path))
		{
			WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
			while (!loadDB.isDone) { }
			// then save to Application.persistentDataPath
			File.WriteAllBytes(path, loadDB.bytes);
		}
	}

	//public void LoadImage(string path) 
	//   {
	//       if (File.Exists(path))
	//       {
	//           myCanvas.overrideSorting = true;
	//           byte[] tempFileData;
	//           tempFileData = File.ReadAllBytes(path);
	//           picture= new Texture2D(2,2);
	//           picture.LoadImage(tempFileData);
	//           myImage.sprite = Sprite.Create(picture, Rect.MinMaxRect(0, 0, picture.width, picture.height), Vector2.zero);

	//       }
	//       else
	//       {
	//           print("Missing image file.");
	//       }

	//   }
	public string GetPersonName()
	{
		//if (Application.platform != RuntimePlatform.Android)
		//{
		//	path = Application.dataPath + "/StreamingAssets/PeopleDB.db";
		//}
		//else
		//{
		//	path = Application.persistentDataPath + "/PeopleDB.db";
		//}
		//OpenDB("PeopleDB.db");
		string conn = "";
		string res = "";
		conn = "URI=file:" + path;

		dbconn = (IDbConnection)new SqliteConnection(conn);
		dbconn.Open();

		dbcmd = dbconn.CreateCommand();

		string sqlQuery = "SELECT FirstName,Lastname FROM persons where PID = 2";
		dbcmd.CommandText = sqlQuery;
		reader = dbcmd.ExecuteReader();

		reader.Read();

		res = reader.GetString(1) + "," + reader.GetString(0);

		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;

		return res; // return matches
	}
	void LoadImage()
	{
		nameText = GetPersonName();
		myCanvas.overrideSorting = true;
		//if (Application.platform != RuntimePlatform.Android)
		//{
		//	path = Application.dataPath + "/StreamingAssets/PeopleDB.db";
		//}
		//else
		//{
		//	path = Application.persistentDataPath + "/PeopleDB.db";
		//}
		//OpenDB("PeopleDB.db");

		string conn = "";

		int pid = 1; //debug

		conn = "URI=file:" + path;

		dbconn = (IDbConnection)new SqliteConnection(conn);
		dbconn.Open();

		dbcmd = dbconn.CreateCommand();

		string query = "SELECT Photo FROM persons WHERE PID=1";

		dbcmd.CommandText = query;
		//reader = dbcmd.ExecuteReader();
		byte[] data = (byte[])dbcmd.ExecuteScalar();

		if (data != null)
		{
			//File.WriteAllBytes("woman2.jpg", data);
			Texture2D sampleTexture = new Texture2D(2, 2);
			bool isLoaded = sampleTexture.LoadImage(data);
			if (isLoaded)
			{
				img.GetComponent<RawImage>().texture = sampleTexture;
				rmimg.texture = sampleTexture;
			}
		}
		//reader.Close();
		//reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;
	}

	public void PlaySound(string path)
    {
       if(audioPlayer.isPlaying) return;
        if (File.Exists(path))
        {
            WWW tempAudioPath = new WWW("file:///"+ path);
            audioClip = tempAudioPath.GetAudioClip(true,true);
            audioPlayer.clip = audioClip;
            audioPlayer.Play();
        }
        else
        {
            print("Missing audio data.");
        }
        
    }

    public void LoadVideo(string path)
    {
       if(videoPlayer.isPlaying) return;
        if (File.Exists(path))
        {
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.enabled = true;
            videoPlayer.url = "file:///" + path;
            if (OnTriggerVideo != null) OnTriggerVideo(this);
            videoPlayer.Play();
        }
        else
        {
            print("Missing video data.");
        }
    }

    public void LoadText()
    {
        myText.text = nameText;
    }


}
