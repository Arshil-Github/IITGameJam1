using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCell : MonoBehaviour
{

    public List<Collider2D> colliderOnIt;
    //public Collider2D[] colliderOnIt;
    public Color color_highlight;
    public Color color_normal;
    public bool isHighlighted = false;

    SpriteRenderer sp;
    Vector2 a;
    private void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        a = new Vector2(transform.position.x + (FindObjectOfType<PlacementBlockSpawner>().cellSize / 2), transform.position.y + (FindObjectOfType<PlacementBlockSpawner>().cellSize / 2));
    }
    public void FixedUpdate()
    {
        if (isHighlighted)
        {
            sp.color = color_highlight;
        }
        else
        {
            sp.color = color_normal;
        }
        //colliderOnIt = Physics2D.OverlapBoxAll(transform.position, a, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
            colliderOnIt.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliderOnIt.Remove(collision);
    }
}
