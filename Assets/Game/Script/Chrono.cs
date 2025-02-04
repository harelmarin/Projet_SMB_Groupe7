using UnityEngine;
using TMPro;

public class Chrono : MonoBehaviour
{
    public float chronometre = 0f; 
    public bool isRunning = true;
    public TextMeshProUGUI chronoText; 

    void FixedUpdate()
    {
        if (isRunning)
        {

            chronometre += Time.deltaTime;


            int seconds = Mathf.FloorToInt(chronometre); 
            int milliseconds = Mathf.FloorToInt((chronometre - seconds) * 100); 


            chronoText.text = string.Format("{0}.{1:00}", seconds, milliseconds);
        }
    }
}