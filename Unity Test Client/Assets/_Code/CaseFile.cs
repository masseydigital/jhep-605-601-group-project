using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CaseFile : MonoBehaviour
{
    private static string mCharacter;
    private static string mRoom;
    private static string mWeapon;

    public static bool setCharacter(string character)
    {
        mCharacter = character;
        return true;
    }

    public static bool setRoom(string room)
    {
        mRoom = room;
        return true;
    }

    public static bool setWeapon(string weapon)
    {
        mWeapon = weapon;
        return true;
    }

    public static bool compareAll(string character, string room, string weapon)
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

    public static bool compareCharacter(string character)
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

    public static bool compareRoom(string room)
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

    public static bool compareWeapon(string weapon)
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