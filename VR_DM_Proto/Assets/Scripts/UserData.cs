using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    public Texture2D picture;
    private Image myImage;
    public AudioClip sound;
    public string nameText;
    private AudioSource source;
    private Text myText;

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
	       myImage.sprite= Sprite.Create(picture, Rect.MinMaxRect(0,0,picture.width,picture.height),Vector2.zero );
           PlaySound();
           LoadText();
	    }
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
