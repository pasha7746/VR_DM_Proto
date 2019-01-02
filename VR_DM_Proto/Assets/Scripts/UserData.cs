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
    private Image myImage;
    public AudioClip audioClip;
    private VideoPlayer videoPlayer;
    public string nameText;
    private AudioSource audioPlayer;
    private Text myText;
    public string imagePath;
    public string audioPath;
    public string videoPath;

    void Awake()
    {
        myImage = GetComponentInChildren<Image>();
        audioPlayer = GetComponent<AudioSource>();
        myText = GetComponentInChildren<Text>();
        videoPlayer = GetComponentInChildren<VideoPlayer>();
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

    public void LoadImage(string path) 
    {
        if (File.Exists(path))
        {
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
        if (File.Exists(path))
        {
            videoPlayer.url = "file:///" + path;
           
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
