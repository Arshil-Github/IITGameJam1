using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{
    [Header("Detection Stufff")]
    public string TargetTag; //Target Tag
    public float RangeRadius;// Range for detection
    public Vector2[] RangeVectors;
    public float AllowRange;// Allow range for mortars

    [Header("Attack Stuff")]
    public float AttackDelay;// Time in between attacks
    public int damage;//Damage to be dealt - transfer it over to bullet.damage

    [Header("Prefabs & Objects")]
    public GameObject pf_bullet;
    public GameObject TankerHead;
    public GameObject pf_InteractUI;

    [Header ("Don't Assign")]
    public bool dragged;
    public bool isAllowedToDrag = false;

    [Header ("Misc.")]
    public Animator anim;

    [Header("Upgrade Stuff")]
    public int cardNeeded = 5;
    public int cardHave = 0;
    public int coinNeeded = 5;
    public int Level = 0;
    public SpriteRenderer primarySprite;
    public SpriteRenderer secondarySprite;
    public List<DefenseUpgrades> upgradeList;

    [Header("Particles")]
    public GameObject pf_upgradeFX;

    [Header("Positioning Stuff")]
    public int widthOfRectangle;

    Transform selectedTarget;//Move towards this target
    public List<PointinTime> pointsinTime; //Define a TimeStation to jump to
    GameMaster gm;
    bool Detect = true;
    List<Collider2D> rangeCells;
    public void Start()
    {
        rangeCells = new List<Collider2D>();
        pointsinTime = new List<PointinTime>(); //Creates a new list
        gm = FindObjectOfType<GameMaster>();

        ChangeAccToUpgrades(upgradeList[0]);


        #region CellDetection

        StartCoroutine(DetectCellwithDelay());

        #endregion
    }
    IEnumerator DetectCellwithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        DetectCell();
    }
    public void DetectCell()
    {

        foreach (Vector2 v in RangeVectors)
        {
            Vector2 vInWorld = new Vector2(transform.position.x + v.x, transform.position.y + v.y);

            foreach (Collider2D c in Physics2D.OverlapCircleAll(vInWorld, 0.1f))
            {
                Debug.Log("Trooll Face");
                if (c.CompareTag("GridBlock"))
                {
                    rangeCells.Add(c.GetComponent<Collider2D>());
                }
            }
        }

    }
    #region EnemyAttackBehaivior---FixedUpdateMethod------OnDrawGizmos
    private void FixedUpdate()
    {
        if (Detect)//If detection is allowed to be turn off when already detected
        {
            #region ForCircularDetection
            /*foreach (Collider2D g in Physics2D.OverlapCircleAll(transform.position, RangeRadius))
            {
                if (g.gameObject.CompareTag(TargetTag) && Mathf.Abs((transform.position - g.transform.position).magnitude) > AllowRange)
                {
                    //Attack
                    selectedTarget = g.transform;
                    StartCoroutine(attackRepeat());


                    Debug.Log("Voila");
                    Detect = false;//turn detection off to maximise performance
                    //Stop further search until til motherfucker is defeated
                }
            }*/
            #endregion

            #region ForRectangularDetection

            /* foreach (Vector2 v in RangeVectors)
             {
                 Vector2 vInWorld = new Vector2(transform.position.x + v.x, transform.position.y + v.y);

                 if (vInWorld.y == transform.position.y)
                 {
                     Vector2 transformSideVertex = new Vector2(transform.position.x, transform.position.y - (widthOfRectangle / 2));

                     Vector2 vectorSideVertex = new Vector2(vInWorld.x, vInWorld.y + (widthOfRectangle / 2));

                     Collider2D[] collidersWithinRange = Physics2D.OverlapAreaAll(transformSideVertex, vectorSideVertex);
                     foreach (Collider2D g in Physics2D.OverlapCircleAll(transform.position, RangeRadius))
                     {
                         if (g.gameObject.CompareTag(TargetTag) && Mathf.Abs((transform.position - g.transform.position).magnitude) > AllowRange)
                         {
                             //Attack
                             selectedTarget = g.transform;
                             StartCoroutine(attackRepeat());


                             Debug.Log("Voila");
                             Detect = false;//turn detection off to maximise performance
                             //Stop further search until til motherfucker is defeated
                         }
                     }
                 }


             }*/
            #endregion

            #region CellDetection

            foreach (Collider2D cells in rangeCells)
            {
                foreach (Collider2D g in cells.GetComponent<PlacementCell>().colliderOnIt)
                {
                    if (g.gameObject.CompareTag(TargetTag) && Mathf.Abs((transform.position - g.transform.position).magnitude) > AllowRange)
                    {
                        //Attack
                        selectedTarget = g.transform;
                        StartCoroutine(attackRepeat());


                        Debug.Log("Voila");
                        Detect = false;//turn detection off to maximise performance
                                       //Stop further search until til motherfucker is defeated
                    }
                }
            }
            #endregion
        }
    }
    private void OnDrawGizmosSelected()
    {
        //------Only for Developer's satisfaction - Remove in final build---------
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, AllowRange);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, RangeRadius);

        UnityEditor.Handles.color = Color.magenta;
        
    }

    IEnumerator attackRepeat()
    {
        //Rotate the head towards enemy
        Quaternion rotation = Quaternion.LookRotation(selectedTarget.position - transform.position, transform.TransformDirection(Vector3.up));
        TankerHead.transform.rotation = new Quaternion(0, 0, -rotation.z, rotation.w);

        //Play Animation
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(AttackDelay);

        //Spawnm bullet which auto attack nearest ene4my
        GameObject bulletSpawned = Instantiate(pf_bullet, TankerHead.transform.position, Quaternion.identity);
        bulletSpawned.GetComponent<Bullet>().damage = damage;

        //Allow detection
        Detect = true;
    }
    #endregion

    #region MouseStuff
    private void OnMouseDrag()
    {
        if (isAllowedToDrag)//If move button is pressed
        {
            Vector2 current = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = current;
        }
    }
    private void OnMouseDown()
    {
        if (!isAllowedToDrag)//If is currently not dragging
        {
            //Instantiate that ui containing upgrade sell and move buttons
            GameObject a = Instantiate(pf_InteractUI, transform.position, Quaternion.identity);
            a.transform.SetParent(gameObject.transform);
        }
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Only for DEveloper Testing To be removed in final build
            AddCard();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Only for DEveloper Testing To be removed in final build
            Upgrade();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Only for DEveloper Testing To be removed in final build
            HighlightRange();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Only for DEveloper Testing To be removed in final build
            deHighlightRange();
        }
    }
    public void HighlightRange()
    {
        foreach(Collider2D c in rangeCells)
        {
            c.GetComponent<PlacementCell>().isHighlighted = true;
        }
    }
    public void deHighlightRange()
    {
        foreach (Collider2D c in rangeCells)
        {
            c.GetComponent<PlacementCell>().isHighlighted = false;
        }
    }
    public void AddCard()
    {
        //Add some kind of visual indication
        cardHave++;   
    }
    public void Upgrade() {
        if (cardHave >= cardNeeded && Level <= upgradeList.Count && gm.coins >= coinNeeded)//If have Enough coins
        {
            //Reduce the number of coins 
            cardHave = cardHave - cardNeeded;
            //Coin Payment
            gm.coins -= coinNeeded;
            //Increase level by 1
            Level += 1;

            ChangeAccToUpgrades(upgradeList[Level]);

            GameObject a = Instantiate(pf_upgradeFX, transform.position, Quaternion.identity);
            Destroy(a, 0.3f);
            //Some Visuals

            //Here do some BIG BRAIN Calculations to increase Card Needed and cose in coins to balance the game
        }
    }
    public void ChangeAccToUpgrades(DefenseUpgrades du)
    {
        gameObject.GetComponent<Heathbody>().health = du.health;
        damage = du.damage;
        /*RangeRadius = du.range;*/
        /*AttackDelay = du.attackDelay;

        primarySprite.sprite = du.primarySprite;
        secondarySprite.sprite = du.secondarySprite;*/
    }
}
