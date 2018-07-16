using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPComponent : MonoBehaviour {

    public float maxHP = 100;
    public float currentHP = 100;

    public bool bImmortal = false;

    private float deltaOverTime = 0;
    private float totalDelta = 0;
    private float timeRemaining = 0;
    private float timeInterval = 1;

	// Use this for initialization
	void Start () {
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
            Debug.Log("currentHP > MaxHP !!!");
        }
	}
	
	// Update is called once per frame
	void Update () {
        DeltaOverTimeDoing();
	}

    public void DoDelta(float delta)
    {
        if (bImmortal)
            return;
        currentHP += delta;
        Debug.Log("Aleart Aleart " + currentHP.ToString());
        

        if (currentHP > maxHP)
            currentHP = maxHP;
        else if (currentHP < 0)
            currentHP = 0;
    }

    public void DoDeltaOverTime(float delta, float time)
    {
        if (bImmortal)
            return;

        totalDelta += delta;
        timeRemaining = time;

        deltaOverTime = totalDelta / timeRemaining;
    }

    private void DeltaOverTimeDoing()
    {
        if (timeRemaining > 0)
        {
            currentHP += deltaOverTime;
            totalDelta -= deltaOverTime;

            if (Mathf.Abs(totalDelta) < Mathf.Abs(deltaOverTime))
                totalDelta = 0;
        }
        else
        {
            timeRemaining -= timeInterval;
        }
    }


}
