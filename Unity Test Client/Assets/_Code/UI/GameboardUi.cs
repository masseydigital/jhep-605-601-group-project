using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameboardUi : MonoBehaviour
{
    public TMP_Dropdown characterDropdown;
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown roomDropdown;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI debugAccuseText;
    public TextMeshProUGUI resultText;

    // Start is called before the first frame update
    void Start()
    {
        FillDropdowns();

        debugAccuseText.text = "";

        SetPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Gives the player a random name
    public void SetPlayerName()
    {
        playerNameText.text = GameManager.gameData.characterNames[Random.Range(0, GameManager.gameData.characterNames.Count)];
    }

    // Fills the dropdowns
    public void FillDropdowns()
    {
        characterDropdown.ClearOptions();
        weaponDropdown.ClearOptions();
        roomDropdown.ClearOptions();

        characterDropdown.AddOptions(GameManager.gameData.characterNames);
        weaponDropdown.AddOptions(GameManager.gameData.weaponNames);
        roomDropdown.AddOptions(GameManager.gameData.roomNames);
    }

    //Performs accuse action
    public void Accuse()
    {
        Debug.Log($"Accusing: {characterDropdown.options[characterDropdown.value].text} " +
            $"in {roomDropdown.options[roomDropdown.value].text} " +
            $"with a {weaponDropdown.options[weaponDropdown.value].text}");

        debugAccuseText.text = $"Accusing: {characterDropdown.options[characterDropdown.value].text} " +
            $"in {roomDropdown.options[roomDropdown.value].text} " +
            $"with a {weaponDropdown.options[weaponDropdown.value].text}";

        bool win = GameManager.instance.CheckWin(
            characterDropdown.options[characterDropdown.value].text,
            roomDropdown.options[roomDropdown.value].text,
            weaponDropdown.options[weaponDropdown.value].text);

        if(win)
        {
            resultText.text = "WINNER!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "Try Again!";
            resultText.color = Color.red;
        }
    }
}
