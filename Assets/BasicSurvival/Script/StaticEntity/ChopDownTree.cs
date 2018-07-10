using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDownTree : MonoBehaviour {

    public Item item;
    private Inventory _inventory;
    private GameObject _player;

    public int maxhp;
    private int hp;

    ItemDataBaseList inventoryItemList;

    Animator animator;

    
    void Start()
    {
        hp = maxhp;

        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
            _inventory = _player.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();
        inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");
        animator = _player.GetComponent<Animator>();
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            float distance = Vector3.Distance(this.gameObject.transform.position, _player.transform.position);

            if (distance <= 3)
            {
                onChop();
            }
        }
    }

    public void getItemByID(int id)
    {
        inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");
        item = inventoryItemList.getItemByID(id);
    }

    public void setMaxHp(int maxhp)
    {
        this.maxhp = maxhp;
        this.hp = maxhp;
    }

    public void onChop()
    {
        hp -= 1;
        animator.SetTrigger("bSwingAxe");

        if (hp < 1)
        {
            dropItem();
            Destroy(this.gameObject);
        }
    }

    public void dropItem()
    {       
        GameObject dropItem;
        dropItem = (GameObject)Instantiate(item.itemModel);
        dropItem.AddComponent<PickUpItem>();
        dropItem.GetComponent<PickUpItem>().item = item;
        dropItem.transform.localPosition = this.transform.localPosition;
    }
}
