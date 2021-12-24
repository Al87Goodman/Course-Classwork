using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SpiritData
{
    // Used for Identifying specific Crystals and Spirit Objects
    public string spiritName;
    public int objectID;
    public string objectName;
    public bool isActive;

    // Spirit Game Object Attributes

    public int HP; 
    public int maxHP;
    public int MP; 
    public int maxMP;
    public int attack;
    public int magic;
    public int defense;
    public int magDef;
    public int speed;

    public int level;
    public int totalExp;
    public int nextLevel;

    //fighter's stat increase bases
    /*
    private int incHP;
    private int incMP;
    private int incAttack;
    private int incMagic;
    private int incDefense;
    private int incMagDef;
    private int incSpeed;
    */



    public SpiritData()
    {
        isActive = false;
    }

}
