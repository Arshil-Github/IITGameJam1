using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public int coins;
    public int diamonds;
    public int experience;
    public float TurnBackTime;

    public Text coin_Text;
    public Text diamond_Text;
    public GameObject TimeMap;
    bool isVisible = false;

    public void StopTime()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0.0000000001f;
        }else if (Time.timeScale == 0.0000000001f)
        {
            Time.timeScale = 1;
        }

    }
    public void ShowTimeMap()
    {
        TimeMap.GetComponent<Animator>().Play("OpenClose");

        if (isVisible)
        {
            //TimeMap.GetComponent<Animator>().speed = -1;
        }
        else
        {
            TimeMap.GetComponent<Animator>().speed = 1;
        }

        TimeMap.GetComponent<Animator>().SetBool("Return", isVisible);
        isVisible = !isVisible;

    }
    private void FixedUpdate()
    {
        coin_Text.text = coins.ToString();
        diamond_Text.text = diamonds.ToString();
    }
    
}
