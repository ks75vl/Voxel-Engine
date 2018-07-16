using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DropItemsData
{
    public Item item;
    public float value;
}

[System.Serializable]
public class DropItems : MonoBehaviour {

    public List<DropItemsData> listItem = null;
    ItemDataBaseList ItemList;
    DropItemsData itemm;

    // Use this for initialization
    void Start () {
		ItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddItemByID(int ID)
    {
        if (listItem == null)
            listItem = new List<DropItemsData>();
        ItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");
        
        itemm.item = ItemList.getItemByID(ID);
        itemm.value = 1.0f;
        listItem.Add(itemm);
    }

    public void DropPerformance()
    {
        Debug.Log("startDrop " + listItem.Count);
        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].value < Random.Range(0.0f, 1.0f))
                continue;
            Debug.Log("Drop");
            GameObject ItemOnGround;
            ItemOnGround = (GameObject)Instantiate(listItem[i].item.itemModel);
            ItemOnGround.AddComponent<PickUpItem>();
            ItemOnGround.GetComponent<PickUpItem>().item = listItem[i].item;
            ItemOnGround.transform.localPosition = this.transform.localPosition;
        }
    }
}
