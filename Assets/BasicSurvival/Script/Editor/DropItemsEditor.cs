using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DropItems))]
public class DropItemsEditor : Editor {

    private int itemID;
    DropItems dropItems;

    Item a;

    void OnEnable()
    {
        dropItems = target as DropItems;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("Box");
        AddItemGUI();
        DropItemGUI();
        GUILayout.EndVertical();
    }

    public void AddItemGUI()
    {
        GUILayout.Label("Choose Item: " + dropItems.listItem.Count);
        EditorGUILayout.BeginHorizontal();
        ItemDataBaseList inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");                            //loading the itemdatabase
        string[] items = new string[inventoryItemList.itemList.Count];                                                      //create a string array in length of the itemcount
        for (int i = 1; i < items.Length; i++)                                                                              //go through the item array
        {
            items[i] = inventoryItemList.itemList[i].itemName;                                                              //and paste all names into the array
        }
        itemID = EditorGUILayout.Popup("", itemID, items, EditorStyles.popup);

        if (GUILayout.Button("Add Item"))                                                                                   //creating button with name "AddItem"
        {
            dropItems.AddItemByID(itemID);
        }
        EditorGUILayout.EndHorizontal();
    }

    public void DropItemGUI()
    {
        EditorGUILayout.BeginHorizontal();

        var serializedObject = new SerializedObject(target);
        var property = serializedObject.FindProperty("listItem");
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndHorizontal();
    }
}
