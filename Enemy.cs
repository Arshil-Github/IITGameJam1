using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum Type { Air, Ground } //Define Type
    public Type attackType;

    public int damage;//Damage dealt on TArget
    public float speed;//Speed

    public enum Target {Defenses, MainTower, Walls}//Select target from dropdown in editor
    public Target TargetTag;

    public int AttackDelay;//TimeBetween Attack
    public float minimumDistance; //Change original position at this distance
    public Animator EnemyAnim;//For Animation

    public List<PointinTime> pointsinTime; //Define a TimeStation to jump to

    Transform selectedTarget;//Move towards this target
    Vector2 originalpos;//Store original position and change to it after attack
    bool ShouldMove = true;//For stoppping movement when nessasary
    bool Detect = true;//For Setection of collision in FixedUpdate

    public bool isHorizontal = true;
    private void Start()
    {
        pointsinTime = new List<PointinTime>(); //Creates a new list
        DetectNearestTagTarget(); //DEtext nearest enemy with given tag

        if(Random.Range(0, 3) == 0)
        {
            isHorizontal = !isHorizontal; 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DetectableBody d)) //If object is detectable body
        {
            selectedTarget = collision.transform;//For heading towards it
            ShouldMove = false;//Stop Moving
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DetectableBody d))//If object is detectable body
        {
            selectedTarget = collision.transform;//For heading towards it
            StartCoroutine(attackRepeat());//Attack
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        DetectNearestTagTarget();//On Exit Detect new selectedTarget
    }
    void DetectNearestTagTarget()
    {
        selectedTarget = GetClosestEnemy(GameObject.FindGameObjectsWithTag(TargetTag.ToString())); //Get nearest tag target
    }
    bool moveToOriginal = false;
    public void Action()
    {
        Detect = false;//Turn off detection in FixedUpdate

        moveToOriginal = true;//For moving back to original position
    }
    bool changeDir = false;
    public void FixedUpdate()
    {
        if(selectedTarget == null)//After selected target's destruction
        {
            DetectNearestTagTarget();//Detect new body
            Detect = true;//Turn on Detection
            ShouldMove = true;//Start Moving
        }
        if (ShouldMove)
        {
            if (!changeDir && selectedTarget.position.x == transform.position.x || selectedTarget.position.y == transform.position.y)
            {
                isHorizontal = !isHorizontal;
            }
            Vector3 moveDirection = new Vector3();
            if (isHorizontal)
            {
                moveDirection = new Vector3(selectedTarget.position.x, transform.position.y, transform.position.z);
            }else
            {
                moveDirection = new Vector3(transform.position.x, selectedTarget.position.y, transform.position.z);
            }
            transform.position = Vector2.MoveTowards(transform.position, moveDirection, speed * Time.deltaTime);
            Quaternion rotation = Quaternion.LookRotation(moveDirection - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }

        if (moveToOriginal) //Moveto OriginalPosition and after .5 sec Start Attacking, Moving
        {
            transform.position = Vector2.MoveTowards(transform.position, originalpos, speed * Time.deltaTime / 2);
            StartCoroutine(setmoveToOriginalToFalseWithDelay());//For Delay
        }

        foreach(Collider2D g in Physics2D.OverlapCircleAll(transform.position, minimumDistance))
        {
            if (g.gameObject.TryGetComponent(out DetectableBody d) && Detect) //If object is detectable body
            {
                originalpos = transform.position;//So that position don't change
                //DetectNearestTagTarget();
                Detect = false;
            }
        }
    }
    IEnumerator setmoveToOriginalToFalseWithDelay()//Moveto OriginalPosition and after .5 sec Start Attacking, Moving
    {
        yield return new WaitForSeconds(.5f);

        moveToOriginal = false;
        ShouldMove = true;//Start Move

        StopAllCoroutines();
    }
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, minimumDistance);
    }
    bool allowedtoAttack = true;
    IEnumerator attackRepeat()
    {
        EnemyAnim.SetBool("Attack", true);
        /*
                Vector2 distanceBtw = transform.position - selectedTarget.position;
                if (distanceBtw.magnitude <= minimumDistance)
                {
                    transform.position = originalpos;
                }*/
        if (allowedtoAttack)
        {
            selectedTarget.gameObject.GetComponent<Heathbody>().ReduceHealth(damage);
            if (selectedTarget.GetComponent<Heathbody>().health < damage + 1)
            {
                DetectNearestTagTarget();
            }
        }
        allowedtoAttack = false;
        yield return new WaitForSeconds(AttackDelay);
        EnemyAnim.SetBool("Attack", false);
        allowedtoAttack = true;


        Action();

        StopAllCoroutines();

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
