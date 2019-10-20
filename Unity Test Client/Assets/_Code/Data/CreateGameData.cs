using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateGameData
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Game Data/New Character Set")]
    public static void CreateCharacterSet()
    {
        CharacterSet characters = ScriptableObject.CreateInstance<CharacterSet>();

        AssetDatabase.CreateAsset(characters, "Assets/_Data/NewCharacterSet.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = characters;
    }

    [MenuItem("Assets/Create/Game Data/New Room Set")]
    public static void CreateRoomSet()
    {
        RoomSet rooms = ScriptableObject.CreateInstance<RoomSet>();

        AssetDatabase.CreateAsset(rooms, "Assets/_Data/NewRoomSet.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = rooms;
    }

    [MenuItem("Assets/Create/Game Data/New Weapon Set")]
    public static void CreateWeaponSet()
    {
        WeaponSet weapons = ScriptableObject.CreateInstance<WeaponSet>();

        AssetDatabase.CreateAsset(weapons, "Assets/_Data/NewWeaponSet.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = weapons;
    }

    [MenuItem("Assets/Create/Game Data/New Game Data Test")]
    public static void CreateTestGameData()
    {
        GameDataTest gamedata = ScriptableObject.CreateInstance<GameDataTest>();

        AssetDatabase.CreateAsset(gamedata, "Assets/_Data/NewGameData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = gamedata;
    }

    [MenuItem("Assets/Create/Game Data/New Database")]
    public static void CreateDatabase()
    {
        DatabaseTest database = ScriptableObject.CreateInstance<DatabaseTest>();

        AssetDatabase.CreateAsset(database, "Assets/_Data/NewDatabase.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = database;
    }
#endif
}

public class CharacterSet : ScriptableObject
{
    public List<string> characters;
}

public class RoomSet : ScriptableObject
{
    public List<string> rooms;
}

public class WeaponSet : ScriptableObject
{
    public List<string> weapons;
}

public class GameDataTest : ScriptableObject
{
    public string id;
    public string theme;
    public CharacterSet characters;
    public RoomSet rooms;
    public WeaponSet weapons;
}

public class DatabaseTest : ScriptableObject
{
    public List<GameDataTest> database;
}

