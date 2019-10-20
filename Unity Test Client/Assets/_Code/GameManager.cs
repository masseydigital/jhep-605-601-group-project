using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameData gameData;
    public static GameManager instance;

    public List<GameObject> characterObjs;
    public List<GameObject> roomObjs;
    public List<GameObject> weaponObjs;

    private DBConnection database;

    private void Awake()
    {
        instance = this;

        database = GameObject.Find("DynamoDB Connection").GetComponent<DBConnection>();

        if(database!= null)
            gameData = database.gameData;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGameData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Loads all of the game data on start
    public void LoadGameData()
    {
        LoadCharacters();
        LoadRooms();
        //LoadWeapons();
    }

    // Loads all the characters
    public void LoadCharacters()
    {
        for(int i=0; i<characterObjs.Count; i++)
        {
            characterObjs[i].GetComponent<TextMeshProUGUI>().text = gameData.characterNames[i];
        }
    }

    // Loads all of the room data on start
    public void LoadRooms()
    {
        for (int i = 0; i < roomObjs.Count; i++)
        {
            roomObjs[i].GetComponent<TextMeshProUGUI>().text = gameData.roomNames[i];
        }
    }

    // Loads all of the weapons on start
    public void LoadWeapons()
    {
        for (int i = 0; i < weaponObjs.Count; i++)
        {
            //weaponObjs[i].GetComponent<TextMeshProUGUI>().text = gameData.roomNames[i];
        }
    }
}