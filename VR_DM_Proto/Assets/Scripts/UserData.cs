using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class UserData : MonoBehaviour
{
    public Texture2D picture;
    private Image myImage;
    public AudioClip sound;
    public string nameText;
    private AudioSource source;
    private Text myText;
    public string imagePath;

    void Awake()
    {
        myImage = GetComponentInChildren<Image>();
        source = GetComponent<AudioSource>();
        myText = GetComponentInChildren<Text>();

    }

    // Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	       LoadImage();
           PlaySound();
           LoadText();
	    }
	}

    public void LoadImage()
    {
        byte[] tempFileData;
        if (File.Exists(imagePath))
        {
            tempFileData = File.ReadAllBytes(imagePath);
            picture= new Texture2D(2,2);
            picture.LoadImage(tempFileData);
        }

        myImage.sprite = Sprite.Create(picture, Rect.MinMaxRect(0, 0, picture.width, picture.height), Vector2.zero);
    }

    public void PlaySound()
    {
        source.clip = sound;
        source.Play();
        
    }

    public void LoadText()
    {
        myText.text = nameText;
    }


}
