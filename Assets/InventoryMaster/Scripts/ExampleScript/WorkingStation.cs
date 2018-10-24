using UnityEngine;
using System.Collections;

public class WorkingStation : MonoBehaviour
{

    public KeyCode openInventory;
    public GameObject craftSystem;
    public int distanceToOpenWorkingStation = 3;
    bool showCraftSystem;
    public Inventory craftInventory;
    public CraftSystem cS;


    // Use this for initialization
    void Start()
    {
        //if (craftSystem != null)
        //{
        //    craftInventory = craftSystem.GetComponent<Inventory>();
        //    cS = craftSystem.GetComponent<CraftSystem>();
        //}
        craftSystem = GameObject.FindGameObjectWithTag("CraftSystem");
        if (craftSystem != null)
        {
            cS = craftSystem.GetComponent<CraftSystem>();
            craftInventory = craftSystem.GetComponent<Inventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (Input.GetKeyDown(openInventory) && distance <= distanceToOpenWorkingStation)
        {
            showCraftSystem = !showCraftSystem;
            if (showCraftSystem)
            {
                Debug.Log("Show Craft !");
                craftInventory.openInventory();
            }
            else
            {
                cS.backToInventory();
                craftInventory.closeInventory();
            }
        }
        if (showCraftSystem && distance > distanceToOpenWorkingStation)
        {
            cS.backToInventory();
            craftInventory.closeInventory();
        }


    }
}
