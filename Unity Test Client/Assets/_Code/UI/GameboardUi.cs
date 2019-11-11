﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameboardUi : MonoBehaviour
{
    // Dropdowns for suggestions/accussations
    public TMP_Dropdown characterDropdown;
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown roomDropdown;

    public TextMeshProUGUI debugAccuseText; // this is the text at the bottom for debugging
    public TextMeshProUGUI resultText;      // more debug text for win/lose

    //These are the images on the left for the suggestions/accusations
    public Image playerAccuseImage;
    public Image roomAccuseImage;
    public Image weaponAccuseImage;

    // These are all of the image option objects that can be accessed
    public ImageOptions playerImages;
    public ImageOptions roomImages;
    public ImageOptions weaponImages;

    // The bars in the upper left
    public List<GameObject> playerBars;
    public List<GameObject> playerMarkers;
    public List<GameObject> playerCrowns;
    public List<GameObject> playerCardIcons;

    // Suggestion/Accusation Butttons
    public GameObject actionButtons;

    // Card/Suggestion Windows
    public GameObject cardWindow;
    public GameObject suggestionWindow;
    public List<GameObject> playerCards;        // Cards in the card window
    public List<GameObject> suggestionCards;    // Cards in the suggestion window

    public LocationData locationData;

    // This is all of the board rooms and hallways
    // This is where players can move their icon too
    // 0 - Study
    // 1 - Hallway
    // 2 - Library
    // 3 - Hallway
    // 4 - Conservatory
    // 5 - Hallway
    // 6 - Hallway
    // 7 - Hallway
    // 8 - Hall
    // 9 - Hallway 
    // 10 - Biliard Room
    // 11 - Hallway
    // 12 - Ball Rooom
    // 13 - Hallway
    // 14 - Hallway 
    // 15 - Hallway 
    // 16 - Lounge
    // 17 - Hallway
    // 18 - Dining Room
    // 19 - Hallway
    // 20 - Kitchen
    public List<Image> boardImages;

    public CaseData caseData;

    public NetworkPlayer networkPlayer;
    public GameManagerService gameManager;

    // Start is called before the first frame update
    void Start()
    {
        FillDropdowns();

        debugAccuseText.text = "";
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

    /// <summary>
    /// Moves the player from one location to another
    /// </summary>
    public void MovePlayerMarker(int player, int to, int from)
    {
        // check if position is taken
        if(locationData.grid[to] != 0)
        {
            // currently occupied
            return;
        }
        else
        {
            locationData.grid[from] = 0;
            locationData.grid[to] = player;
        }

        // update images
        boardImages[to].sprite = playerImages.images[player];
        boardImages[from].sprite = playerImages.nullImage;
    }

    /// <summary>
    /// Updates the room image based on the current dropdown select
    /// This is triggered when a player selects a new room
    /// </summary>
    public void UpdateRoomImage()
    {
        roomAccuseImage.sprite = roomImages.images[roomDropdown.value];
    }

    /// <summary>
    /// Updates the weapon image based on the current dropdown select
    /// This is triggered when a player selects a new weapon
    /// </summary>
    public void UpdateWeaponImage()
    {
        weaponAccuseImage.sprite = weaponImages.images[weaponDropdown.value];
    }

    /// <summary>
    /// Updates the player image based on the current dropdown select
    /// This is triggered when a player selects a new player
    /// </summary>
    public void UpdatePlayerImage()
    {
        playerAccuseImage.sprite = playerImages.images[characterDropdown.value];
    }

    /// <summary>
    /// Shows the given player bar
    /// </summary>
    /// <param name="id"></param>
    public void ShowPlayerBar(int id)
    {
        playerBars[id].SetActive(true);
    }

    /// <summary>
    /// Hides the given player bar
    /// </summary>
    /// <param name="id"></param>
    public void HidePlayerMarker(int id)
    {
        playerMarkers[id].SetActive(false);
    }

    /// <summary>
    /// Shows the player crown for a specific player
    /// Player crowns coorelate to the player whose turn it is
    /// </summary>
    /// <param name="id"></param>
    public void ShowPlayerCrown(int id)
    {
        playerCrowns[id].SetActive(true);
    }

    /// <summary>
    /// Hides the player crown for a specific player
    /// Player crowns coorelate to the player whose turn it is
    /// </summary>
    /// <param name="id"></param>
    public void HidePlayerCrown(int id)
    {
        playerCrowns[id].SetActive(true);
    }

    /// <summary>
    /// Shows the given player bar
    /// </summary>
    /// <param name="id"></param>
    public void ShowPlayerMarker(int id)
    {
        playerMarkers[id].SetActive(true);
    }

    /// <summary>
    /// Hides the given player bar
    /// </summary>
    /// <param name="id"></param>
    public void HidePlayerBar(int id)
    {
        playerBars[id].SetActive(false);
    }

    /// <summary>
    /// Updates the player name for the given character
    /// </summary>
    /// <param name="id"></param>
    public void UpdatePlayerName(int id, string pname)
    {
        playerBars[id].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = pname;
    }

    /// <summary>
    /// Opens the card window
    /// </summary>
    public void OpenCardWindow()
    {
        cardWindow.SetActive(true);
    }

    /// <summary>
    /// Closes the card window
    /// </summary>
    public void CloseCardWindow()
    {
        cardWindow.SetActive(false);
    }

    /// <summary>
    /// Closes the card window
    /// </summary>
    public void CloseSuggestionWindow()
    {
        suggestionWindow.SetActive(false);
    }

    /// <summary>
    /// Opens the card window
    /// </summary>
    public void OpenSuggestionWindow()
    {
        suggestionWindow.SetActive(true);
    }

    /// <summary>
    /// Sets the number of cards that the player has
    /// TODO: Link to Game Manager
    /// </summary>
    public void InitializeCards(int num)
    {
        // Activate the number of cards you have
        for(int i=0; i<num; i++)
        {
            playerCards[i].SetActive(true);
            //TODO: Add method to polymorph card
        }
    }

    public void HideActionButtons()
    {
        actionButtons.SetActive(false);
    }

    public void ShowActionButtons()
    {
        actionButtons.SetActive(true);
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

        /*
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
        */

        // Make the case
        caseData.character = characterDropdown.options[characterDropdown.value].text;
        caseData.room = roomDropdown.options[roomDropdown.value].text;
        caseData.weapon = weaponDropdown.options[weaponDropdown.value].text;

        // Send the accusation over the network
        networkPlayer.MakeAccusation(caseData);
    }

    // Make a suggestion
    public void Suggest()
    {
        Debug.Log($"Suggesting: {characterDropdown.options[characterDropdown.value].text} " +
           $"in {roomDropdown.options[roomDropdown.value].text} " +
           $"with a {weaponDropdown.options[weaponDropdown.value].text}");
        
        // Make the case
        caseData.character = characterDropdown.options[characterDropdown.value].text;
        caseData.room = roomDropdown.options[roomDropdown.value].text;
        caseData.weapon = weaponDropdown.options[weaponDropdown.value].text;

        // Send the accusation over the network
        networkPlayer.MakeSuggestion(caseData);
    }
}
