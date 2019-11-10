using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CaseData
{
    public string character;
    public string room;
    public string weapon;
}


[System.Serializable]
public class CaseFile
{
    CaseData caseData;

    public string Character
    {
        get { return caseData.character; }
        set { caseData.character = value; }
    }

    public string Room
    {
        get { return caseData.room; }
        set { caseData.room = value; }
    }

    public string Weapon
    {
        get { return caseData.weapon; }
        set { caseData.weapon = value; }
    }

    #region Methods
    public bool compareAll(string character, string room, string weapon)
    {
        if (caseData.character.Equals(character) && caseData.room.Equals(room) && caseData.weapon.Equals(weapon))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool compareCharacter(string character)
    {
        if (caseData.character.Equals(character))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool compareRoom(string room)
    {
        if (caseData.room.Equals(room))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool compareWeapon(string weapon)
    {
        if (caseData.weapon.Equals(weapon))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion Methods
}