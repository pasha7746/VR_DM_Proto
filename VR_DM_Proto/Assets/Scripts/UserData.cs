using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR;

public class UserData : MonoBehaviour
{
    public Texture2D picture;
    private Canvas myCanvas;
    private Image myImage;
    public AudioClip audioClip;
    public VideoPlayer videoPlayer;
    public string nameText;
    private AudioSource audioPlayer;
    private Text myText;
    public string imagePath;
    public string audioPath;
    public string videoPath;
    public event Action<UserData> OnTriggerVideo;

    void Awake()
    {
        myImage = GetComponentInChildren<Image>();
        audioPlayer = GetComponent<AudioSource>();
        myText = GetComponentInChildren<Text>();
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        myCanvas = GetComponentInChildren<Canvas>();
    }

    void Start()
    {
        videoPlayer.loopPointReached += (a) => { a.gameObject.SetActive(false); ; };
        videoPlayer.gameObject.SetActive(false);
    }

    void OnEnable()
    {
       
    }

    public void Setup()
    {
        LoadImage(imagePath);
        LoadText();
    }

    // Update is called once per frame
    void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	       LoadImage(imagePath);
           PlaySound(audioPath);
           LoadText();
           LoadVideo(videoPath);
	    }
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

    public void LoadImage(string path) 
    {
        if (File.Exists(path))
        {
            myCanvas.overrideSorting = true;
            byte[] tempFileData;
            tempFileData = File.ReadAllBytes(path);
            picture= new Texture2D(2,2);
            picture.LoadImage(tempFileData);
            myImage.sprite = Sprite.Create(picture, Rect.MinMaxRect(0, 0, picture.width, picture.height), Vector2.zero);

        }
        else
        {
            print("Missing image file.");
        }

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
