using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOnTowers : MonoBehaviour
{
    public Button upgrade_Button;
    public Button sell_Button;
    public Button move_Button;
    public Button close_Button;
    public Button place_Button;

    public GameObject panel;
    Defense parent;
    // Start is called before the first frame update
    void Start()
    {
        upgrade_Button = gameObject.transform.Find("Upgrade").GetComponent<Button>();
        sell_Button = gameObject.transform.Find("Sell").GetComponent<Button>();
        move_Button = gameObject.transform.Find("Move").GetComponent<Button>();
        close_Button = gameObject.transform.Find("Close").GetComponent<Button>();
        place_Button = gameObject.transform.Find("Place").GetComponent<Button>();
        panel = gameObject.transform.Find("UpgradeStuff").gameObject;


        upgrade_Button.onClick.AddListener(Upgrade);
        sell_Button.onClick.AddListener(Sell);
        move_Button.onClick.AddListener(Move);
        close_Button.onClick.AddListener(Close);
        place_Button.onClick.AddListener(Place);

        place_Button.gameObject.SetActive(false);

        parent = transform.GetComponentInParent<Defense>();

        parent.HighlightRange();
    }

    private void FixedUpdate()
    {
        panel.transform.Find("currentHealth").GetComponent<TextMeshProUGUI>().text = transform.GetComponentInParent<Heathbody>().health.ToString();
        panel.transform.Find("currentDamage").GetComponent<TextMeshProUGUI>().text = parent.damage.ToString();


        panel.transform.Find("leveledHealth").GetComponent<TextMeshProUGUI>().text = parent.upgradeList[parent.Level + 1].health.ToString();
        panel.transform.Find("leveledDamage").GetComponent<TextMeshProUGUI>().text = parent.upgradeList[parent.Level + 1].damage.ToString();
    }

    public void Upgrade()
    {
        transform.GetComponentInParent<Defense>().Upgrade();
    }
    public void Sell()
    {
        Debug.Log("Sell");
    }
    public void Move()
    {
        Debug.Log("Yeah Boi");
        //Set All other buttons inactive
        upgrade_Button.gameObject.SetActive(false);
        panel.SetActive(false);
        sell_Button.gameObject.SetActive(false);
        move_Button.gameObject.SetActive(false);
        close_Button.gameObject.SetActive(false);

        //Set Place button active
        place_Button.gameObject.SetActive(true);

        //Is now allowed to drag
        transform.GetComponentInParent<Defense>().isAllowedToDrag = true;

        //My future boi here add some kind of Graphical indication O
    }
    public void Place()
    {
        //Man Just disable all that Graphical indiacation 0 coolness
        Debug.Log("Placed");
        //No longer allowed to drag
        transform.GetComponentInParent<Defense>().isAllowedToDrag = false;

        //Kill Self
        Close();
    }
    public void Close()
    {
        Destroy(gameObject);
        parent.HighlightRange();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
