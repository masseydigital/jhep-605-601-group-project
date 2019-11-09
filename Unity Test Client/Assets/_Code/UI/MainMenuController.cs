using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public DBConnection dataRet;
    public TMP_Dropdown theme_dropdown;
    public TMP_InputField playerName_input;
    public TMP_InputField serverName_input;
    public TMP_InputField serverPort_input;

    public PlayerData playerData;
    public RandomNameGenerator nameGenerator;
    public GameServer server;

    // Start is called before the first frame update
    void Start()
    {
        playerName_input.text = nameGenerator.RandomName();
        UpdateName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindGame()
    {
        SceneManager.LoadScene(1);
    }

    // Communicates the the game data to update the theme
    public void UpdateTheme()
    {
        dataRet.RetrieveData(theme_dropdown.value);
    }

    public void UpdateName()
    {
        server.playerName = playerName_input.text;
    }

    public void UpdateIpAddress()
    {
        server.networkAddress = serverName_input.text;            
    }

    public void UpdatePortNumber()
    {
        server.networkPort = int.Parse(serverPort_input.text);
    }
}
