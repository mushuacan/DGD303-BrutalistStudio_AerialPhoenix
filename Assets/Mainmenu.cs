// Created by NightTime Developments!

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;


public class Mainmenu : MonoBehaviour

{

    public GameObject mainmenue;

    public GameObject Player;

    public GameObject MenuCam;

    // Start is called before the first frame update

    public void Play()

    {

        Player.SetActive(false);

        mainmenue.SetActive(true);

    }


    // Update is called once per frame

    /* void Update()

     {



     }*/


    public void PlayButton()

    {

        mainmenue.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        Player.SetActive(true);

        MenuCam.SetActive(false);

    }

}


