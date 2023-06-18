using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMaster : MonoBehaviour
{
    public bool isRewinding = false; //Check whether Rewinding is needed or not
    public List<GameObject> GameComponents; //Stores all the child of Time Master that need rewinding

    List<GameObject> EnemyList; //Differentiation for enemy
    List<GameObject> DefenseList; //Differentiation for defenses

    public List<PointinTime> pointsinTime; //Only to count the number of pointinTimes
    public int TimeRewingSpeed = 1;

    private void Start()
    {
        EnemyList = new List<GameObject>();
        DefenseList = new List<GameObject>();
        // Basiccly this segregates gameComponent List into EnemyList and DefensesList
        foreach(GameObject e in GameComponents)
        {
            if (e.CompareTag("Enemy"))
                EnemyList.Add(e);
            if (e.CompareTag("Defenses"))
                DefenseList.Add(e);
        }
        pointsinTime = new List<PointinTime>(); //Creates a new list
    }
    private void Update()
    {
        //This is only for developer testing. Android mobiles dont have keyboard
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRewind();

        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            StopRewind();
        }
    }
    private void FixedUpdate()
    {
        //This Make sure that stats are recorded when not rewinding
        if (isRewinding)
            Rewind();
        else
            Record();
    }
    public void addToDefenses(GameObject o)
    {
        DefenseList.Add(o);
    }
    public void Rewind()
    {
        //This portion assigns values in PointinTime to respective variables
        if (pointsinTime.Count > 0)
        {
            //For Enemy
            foreach (GameObject g in EnemyList)
            {
                if (g.GetComponent<Enemy>().pointsinTime.Count > 0)
                {
                    g.GetComponent<Heathbody>().health = g.GetComponent<Enemy>().pointsinTime[0].health;
                    g.GetComponent<Heathbody>().HealthBar.GetComponent<healthbar>().SetHealth(g.GetComponent<Enemy>().pointsinTime[0].health);

                    g.transform.position = g.GetComponent<Enemy>().pointsinTime[0].position;
                    g.transform.rotation = g.GetComponent<Enemy>().pointsinTime[0].rotation;

                    g.SetActive(g.GetComponent<Enemy>().pointsinTime[0].isActive);

                    g.GetComponent<Enemy>().pointsinTime.RemoveAt(0);
                }
            }
            //For Defenses
            foreach (GameObject g in DefenseList)
            {
                if (g.GetComponent<Defense>().pointsinTime.Count > 0)
                {
                    g.GetComponent<Heathbody>().health = g.GetComponent<Defense>().pointsinTime[0].health;
                    g.GetComponent<Heathbody>().HealthBar.GetComponent<healthbar>().SetHealth(g.GetComponent<Defense>().pointsinTime[0].health);

                    g.transform.position = g.GetComponent<Defense>().pointsinTime[0].position;
                    g.transform.rotation = g.GetComponent<Defense>().pointsinTime[0].rotation;

                    g.SetActive(g.GetComponent<Defense>().pointsinTime[0].isActive);

                    g.GetComponent<Defense>().pointsinTime.RemoveAt(0);
                }
            }
            pointsinTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }
    public void StartRewind()
    {
        isRewinding = true;
    }
    public void StopRewind()
    {
        isRewinding = false;
    }
    public void Record()
    {
        foreach(GameObject g in EnemyList)
        {
            bool isActive = g.activeInHierarchy;
            if (isActive == true)
                g.GetComponent<Enemy>().pointsinTime.Insert(0, new PointinTime(g.transform.position, g.transform.rotation, isActive, g.GetComponent<Heathbody>().health));
        }
        foreach (GameObject g in DefenseList)
        {
            bool isActive = g.activeInHierarchy;
            if(g.GetComponent<Defense>().dragged)
            {
                isActive = false;
                g.GetComponent<Defense>().dragged = false;
            }
            else
            {
                isActive = true;
            }
            g.GetComponent<Defense>().pointsinTime.Insert(0, new PointinTime(g.transform.position, g.transform.rotation, isActive, g.GetComponent<Heathbody>().health));
        }

        pointsinTime.Insert(0, new PointinTime(transform.position, transform.rotation, true, 10));
    }
}
