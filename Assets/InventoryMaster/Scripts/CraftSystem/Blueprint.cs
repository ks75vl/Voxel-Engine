using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eCraftCondition
{
    Hand,
    FirePit
}

[System.Serializable]
public class Blueprint
{
    public List<int> ingredients = new List<int>();
    public List<int> amount = new List<int>();
    public List<eCraftCondition> condition = new List<eCraftCondition>();
    public Item finalItem;
    public int amountOfFinalItem;
    public float timeToCraft;

    public Blueprint(List<int> ingredients, int amountOfFinalItem, List<int> amount, Item item)
    {
        this.ingredients = ingredients;
        this.amount = amount;
        finalItem = item;
    }

    public Blueprint(List<int> ingredients, int amountOfFinalItem, List<int> amount, Item item, List<eCraftCondition> condition)
    {
        this.ingredients = ingredients;
        this.amount = amount;
        this.condition = condition;
        finalItem = item;
    }

    public Blueprint() { }

}