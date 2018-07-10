using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChopDownTree))]
public class ChopDownTreeEditor : Editor
{
    private int itemID;
    ChopDownTree chopTree;

    void OnEnable()
    {
        chopTree = target as ChopDownTree;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("Box");
        AttributeGUI();
        GUILayout.EndVertical();
        GUILayout.BeginVertical("Box");
        ItemGUI();
        GUILayout.EndVertical();
    }

    //Hiển thị các chỉ số của ChopDownTree
    void AttributeGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("MAX HP: ");
        chopTree.setMaxHp(int.Parse(EditorGUILayout.TextField(chopTree.maxhp.ToString(), EditorStyles.miniTextField)));
            
        EditorGUILayout.EndHorizontal();
    }

    //Dùng để chọn Item sẽ drop
    void ItemGUI()
    {
        GUILayout.Label("Choose Item Drop:");

        EditorGUILayout.BeginHorizontal();
        ItemDataBaseList inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");                            //loading the itemdatabase
        string[] items = new string[inventoryItemList.itemList.Count];                                                      //create a string array in length of the itemcount
        for (int i = 1; i < items.Length; i++)                                                                              //go through the item array
        {
            items[i] = inventoryItemList.itemList[i].itemName;                                                              //and paste all names into the array
        }
        itemID = EditorGUILayout.Popup("", itemID, items, EditorStyles.popup);
        if (itemID != 0)
            chopTree.getItemByID(itemID);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Current Item Drop:");
        GUILayout.Label(chopTree.item.itemName);
        EditorGUILayout.EndHorizontal();
    }
}
