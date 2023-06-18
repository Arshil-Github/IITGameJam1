using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Heathbody : MonoBehaviour
{
    public int health;
    public GameObject HealthBar; //To Update
    public GameObject pf_dieEffect;
    public UnityEvent afterDeath;
    private void Start()
    {
        HealthBar.GetComponent<healthbar>().SetMaxHealth(health); // Set Health Bar to max
    }
    public void ReduceHealth(int damage)
    {
        //Can be called from any damaging unit. Pretty Handy isn't it?
        if (health > 0)
        {
            health -= damage;
            HealthBar.GetComponent<healthbar>().SetHealth(health);
        }
        else
        {
            //This stuff happens when Health goes below or equal zero
            //Destroy(gameObject);
            GameObject.FindGameObjectWithTag("EffectSpawner").GetComponent<EffectSpawner>().instantiateEffect(pf_dieEffect, 1f, transform.position, 1);
            gameObject.SetActive(false); //Setting Inactive for Reqind Stuff
            afterDeath.Invoke();
        }
    }
}
