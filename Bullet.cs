using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string TargetTag;
    public Rigidbody2D rb;
    public float speed;
    public int damage;
    public GameObject pf_effect;
    public void Update()
    {
     
        if(GameObject.FindGameObjectWithTag(TargetTag) != null)
        {
            Quaternion rotation = Quaternion.LookRotation(GetClosestEnemy(GameObject.FindGameObjectsWithTag(TargetTag)).position - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0, 0, -rotation.z, rotation.w);

            transform.position = Vector2.MoveTowards(transform.position, GetClosestEnemy(GameObject.FindGameObjectsWithTag(TargetTag)).position, speed * Time.deltaTime);
        }
        else
        {
            GameObject.FindGameObjectWithTag("EffectSpawner").GetComponent<EffectSpawner>().instantiateEffect(pf_effect, 1f, transform.position, 1);
            Destroy(gameObject, .01f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TargetTag))
        {
            collision.gameObject.GetComponent<Heathbody>().ReduceHealth(damage);

            GameObject.FindGameObjectWithTag("EffectSpawner").GetComponent<EffectSpawner>().instantiateEffect(pf_effect, 1f, transform.position, 1);
            Destroy(gameObject, .01f);
        }
    }
    Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }
}
