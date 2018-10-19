using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{

    // Gap van de ve Layer Mask. Ground duoc Spawn ra su dung LayerMask mac dinh va Player chi di duoc tren LayerMask mac dinh nen khong
    //the nang GreenBox len theo kieu thong thuong ma phai dung tham so

    // Use this for initialization
    Item item;
    public GameObject buildingModel;            //Tam thoi bo item vao cho co, Dung buildingModel de thay the vi item khong co model building cho rieng minh;
    public Camera camera;
    RaycastHit hit;
    bool bBuildModeOn = false;
    public int layerMask = 1 << 1;


    //Debug
    public bool bDebug = true;
    GameObject player;
    GameObject dubBuildingBox = null;

    public void Update()
    {

        //Initialize Debug Stat
        if (bDebug)
        {
            // bBuildModeOn = true;
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                Debug.Log("Can't Get Player Object!");
        }       

        if (!bBuildModeOn)
            return;


        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (dubBuildingBox == null)
            {
                dubBuildingBox = Instantiate(buildingModel, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                dubBuildingBox.transform.position = hit.point;
                Vector3 temp = dubBuildingBox.transform.position;
                temp.y += dubBuildingBox.GetComponent<Collider>().bounds.size.y / 2 + 0.5f;
                dubBuildingBox.transform.position = temp;
            }


            if (bDebug)
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            }
        }
        else
        {
            if (dubBuildingBox != null)
            {
                Destroy(dubBuildingBox);
                dubBuildingBox = null;
            }


            if (bDebug)
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            }
        }       
    }


    public void BuildModeOn()
    {
        bBuildModeOn = true;
    }
    public void BuildModeOff()
    {
        bBuildModeOn = false;
    }
    public void GetItem(Item item)
    {
        this.item = item;
    }

    public void OnMouseDown()
    {      
        //Left Mouse Clicked
        if (bBuildModeOn)
        {
            Inventory inventory = GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>();
            Instantiate(item.itemModel, dubBuildingBox.transform.position, Quaternion.identity);

            Destroy(dubBuildingBox);
            dubBuildingBox = null;

            inventory.deleteItemFromInventory(item);

            BuildModeOff();
        }
    }

    public void OnMouseOver()
    {
        //Right Mouse Clicked
        if (Input.GetMouseButtonDown(1) && bBuildModeOn)
        {
            Destroy(dubBuildingBox);
            dubBuildingBox = null;

            BuildModeOff();
        }
    }
}