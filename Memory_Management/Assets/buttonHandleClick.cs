using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Elenesski.Camera.Utilities;

public class buttonHandleClick : MonoBehaviour
{
    public Camera mainCamera;
    public AppManager app;
    public Button startButton;
    public Light light1;
    public Light light2;
    public Light light3;
    public Light light4;
    int counter;
    bool isStarted;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        isStarted = false;
        app = FindObjectOfType<AppManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted)
        {
            counter++;
            if(counter == 60)
            {
                light1.intensity = 1;
                light1.gameObject.SetActive(true);
            }
            if (counter == 70)
                light1.gameObject.SetActive(false);

            if (counter == 90)
            {
                light1.gameObject.SetActive(true);
                light1.intensity = 3;
            }

            if (counter == 95)
                light2.gameObject.SetActive(true);

            if (counter == 120)
            {
                light2.gameObject.SetActive(false);

                light3.gameObject.SetActive(true);

            }

            if (counter == 135)
            {
                light4.gameObject.SetActive(true);
                light4.intensity = 1;
            }

            if (counter == 150)
                light4.gameObject.SetActive(false);

            if (counter == 160)
                light2.gameObject.SetActive(true);

            if (counter == 180)
            {
                light4.gameObject.SetActive(true);
                light4.intensity = 2;
            }

            if (counter == 190)
            {
                light4.gameObject.SetActive(false);
                light3.gameObject.SetActive(false);

            }

            if (counter == 210)
            {
                light4.gameObject.SetActive(true);
                light4.intensity = 3;
            }

            if (counter == 245)
            {
                light3.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
            }
            mainCamera.GetComponent<GenericMoveCamera>().Operational = true;
            app.isGamePaused = false;

        }
    }

    public void startGame()
    {
        isStarted = true;
        startButton.gameObject.SetActive(false);
        
    }
}
