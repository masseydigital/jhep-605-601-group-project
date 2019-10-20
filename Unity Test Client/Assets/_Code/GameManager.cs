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

    private string goalCharacter;
    private string goalRoom;
    private string goalWeapon;

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

        SelectRandomWinConditions();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Select a random set of win conditions for testing
    public void SelectRandomWinConditions()
    {
        goalCharacter = gameData.characterNames[Random.Range(0, gameData.characterNames.Count)];
        goalRoom = gameData.roomNames[Random.Range(0, gameData.roomNames.Count)];
        goalWeapon = gameData.weaponNames[Random.Range(0, gameData.weaponNames.Count)];
    }

    //Check to see if we won
    public bool CheckWin(string chr, string room, string wpn)
    {
        if(goalCharacter == chr && goalRoom == room && goalWeapon == wpn)
        {
            return true;
        }
        else
        {
            return false;
        }
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