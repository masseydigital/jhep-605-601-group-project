using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseFile : MonoBehaviour
{
    private string mCharacter;
    private string mRoom;
    private string mWeapon;

    public bool setCharacter(string character)
    {
        mCharacter = character;
        return true;
    }

    public bool setRoom(string room)
    {
        mRoom = room;
        return true;
    }

    public bool setWeapon(string weapon)
    {
        mWeapon = weapon;
        return true;
    }

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
}