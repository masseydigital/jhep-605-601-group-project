using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseFile : MonoBehaviour
{
    private string mCharacter;
    private string mRoom;
    private string mWeapon;

    public string Character
    {
        get { return mCharacter; }
        set { mCharacter = value; }
    }

    public string Room
    {
        get { return mRoom; }
        set { mRoom = value; }
    }

    public string Weapon
    {
        get { return mWeapon; }
        set { mWeapon = value; }
    }

    #region Methods
    public bool compareAll(string character, string room, string weapon)
    {
        if (mCharacter.Equals(character) && mRoom.Equals(room) && mWeapon.Equals(weapon))
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
        if (mCharacter.Equals(character))
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
        if (mRoom.Equals(room))
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
        if (mWeapon.Equals(weapon))
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