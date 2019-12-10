using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ClueLess;

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
    //public List<GameObject> playerMarkers; //Replaces by room ui
    public List<GameObject> playerCrowns;
    public List<GameObject> playerCardIcons;

    // Suggestion/Accusation Butttons
    public GameObject actionButtons;

    // Card/Suggestion Windows
    public GameObject cardWindow;
    public GameObject suggestionWindow;
    public GameObject helpWindow;
    public List<GameObject> playerCards;        // Cards in the card window
    public List<GameObject> suggestionCards;    // Cards in the suggestion window

    public GameObject rulesPanel;               // Game rules
    public List<GameObject> rulePages;
    public int currentRulePageIndex = 0;

    public Gameboard gameboard;
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
    public List<RoomUi> roomUis;

    public CaseData caseData;       // Holds an accusation or suggestion
    public CaseData proofCards;

    public NetPlayer networkPlayer;
    public ClueLess.GameManager gameManager;
    public ClueLess.Deck deck;

    // Start is called before the first frame update
    void Start()
    {
        if(deck == null)
        {
            deck = GameObject.Find("Deck").GetComponent<ClueLess.Deck>();
        }

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

    public void SetBroadcastUI(string message)
    {
        debugAccuseText.text = message;
    }

    /// <summary>
    /// Moves the player from one location to another
    /// </summary>
    public bool MovePlayerMarker(int player, int from, int to)
    {
        /*
        if(!)
        {
            return false;
        }
        */

        gameboard.Move(player, from, to);

        UpdateRoomImages();

        //roomUis[to].AddMarker(player);
        //roomUis[from].RemoveMarker(player);

        return true;
    }

    /// <summary>
    /// Updates all the room images
    /// </summary>
    public void UpdateRoomImages()
    {
        Debug.Log("Updating the room images...");

        foreach(RoomUi room in roomUis)
        {
            room.UpdateMarkers();
        }
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

    public void ShowHelpBar()
    {
        helpWindow.SetActive(true);
    }

    /// <summary>
    /// Hides the given player bar
    /// </summary>
    /// <param name="id"></param>
    //public void HidePlayerMarker(int id)
    //{
    //    playerMarkers[id].SetActive(false);
    //}

    /// <summary>
    /// Shows the given player bar
    /// </summary>
    /// <param name="id"></param>
    //public void ShowPlayerMarker(int id)
    //{
    //    playerMarkers[id].SetActive(true);
    //}

        // Updates the crowns
    public void UpdateCrowns(int turn)
    {
        for(int i=0; i<playerCrowns.Count; i++)
        {
            if (i == turn)
            {
                ShowPlayerCrown(turn);
            }
            else
            {
                HidePlayerCrown(i);
            }
        }
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
        playerCrowns[id].SetActive(false);
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
        InitializeCards(networkPlayer.hand.Count);
    }

    /// <summary>
    /// Closes the card window
    /// </summary>
    public void CloseCardWindow()
    {
        cardWindow.SetActive(false);
    }

    /// <summary>
    /// Opens the card window
    /// </summary>
    public void OpenSuggestionWindow(CaseData suggestion)
    {
        suggestionWindow.SetActive(true);
        InitializeProofCards(suggestion);
    }

    /// <summary>
    /// Closes the card window
    /// </summary>
    public void CloseSuggestionWindow()
    {
        suggestionWindow.SetActive(false);
        caseData = new CaseData();
    }
    /// <summary>
    /// Make a proof with the character suggestion card and close the card window
    /// </summary>
    public void ProveSuggestionCard1()
    {
        Debug.Log("GameboardUI:ProveSuggestionCard1: Proving card 1");
        suggestionWindow.SetActive(false);
        networkPlayer.Cmd_MakeProof(0);
    }
    /// <summary>
    /// Make a proof with the room suggestion card and close the card window
    /// </summary>
    public void ProveSuggestionCard2()
    {
        Debug.Log("GameboardUI:ProveSuggestionCard2: Proving card 2");
        suggestionWindow.SetActive(false);
        networkPlayer.Cmd_MakeProof(1);
    }
    /// <summary>
    /// Make a proof with the weapon suggestion card and close the card window
    /// </summary>
    public void ProveSuggestionCard3()
    {
        Debug.Log("GameboardUI:ProveSuggestionCard3: Proving card 3");
        suggestionWindow.SetActive(false);
        networkPlayer.Cmd_MakeProof(2);
    }

    /// <summary>
    /// Sets the number of cards that the player has
    /// TODO: Link to Game Manager
    /// </summary>
    public void InitializeCards(int numCards)
    {
        // Activate the number of cards you have
        for(int i=0; i<playerCards.Count; i++)
        {
            if (i < numCards)
            {
                playerCards[i].SetActive(true);
                SetPlayerCard(i, networkPlayer.hand[i].name);
            }
            else
            {
                playerCards[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Initializes the player's cards that are available for making a proof.
    /// </summary>
    public void InitializeProofCards(CaseData suggestion)
    {
        suggestionCards[0].SetActive(false);    // Suggestion card 0 represents the character
        suggestionCards[1].SetActive(false);    // Suggestion card 1 represents the room
        suggestionCards[2].SetActive(false);    // Suggestion card 2 represents the weapon

        // Activate the number of cards you have
        for(int i = 0; i < networkPlayer.hand.Count; i++)
        {
            if (networkPlayer.hand[i].name == (suggestion.character))
            {
                Debug.Log("GameboardUI.InitializeProofCards: player contains the character card in the suggestion");
                suggestionCards[0].SetActive(true);
                SetSuggestionCard(0, networkPlayer.hand[i].name);
                proofCards.character = networkPlayer.hand[i].name;
            }
            else if (networkPlayer.hand[i].name == (suggestion.room))
            {
                Debug.Log("GameboardUI.InitializeProofCards: player contains the room card in the suggestion");
                suggestionCards[1].SetActive(true);
                SetSuggestionCard(1, networkPlayer.hand[i].name);
                proofCards.room = networkPlayer.hand[i].name;
            }
            else if (networkPlayer.hand[i].name == (suggestion.weapon))
            {
                Debug.Log("GameboardUI.InitializeProofCards: player contains the weapon card in the suggestion");
                suggestionCards[2].SetActive(true);
                SetSuggestionCard(2, networkPlayer.hand[i].name);
                proofCards.weapon = networkPlayer.hand[i].name;
            }
        }
    }

    /// <summary>
    /// Sets the player card based on the name
    /// </summary>
    /// <param name="cardName"></param>
    public void SetPlayerCard(int cardIndex, string cardName)
    {
        int index = 0;
        for(int i=0; i<deck.allCards.Count; i++)
        {
            // We found our card
            if (cardName == deck.allCards[i].name)
            {
                index = i;
                break;
            }
        }

        // it's a player card
        if(index <= 5)
        {
            playerCards[cardIndex].GetComponent<Image>().sprite = playerImages.images[index];
        }
        else if(index <= 11) // it's a weapon
        {
            playerCards[cardIndex].GetComponent<Image>().sprite = weaponImages.images[index - 6];
        }
        else if(index <= 21) // it's a room
        {
            playerCards[cardIndex].GetComponent<Image>().sprite = roomImages.images[index - 12];
        }
    }

    /// <summary>
    /// Sets the suggestion card based on the name
    /// </summary>
    /// <param name="cardName"></param>
    public void SetSuggestionCard(int cardIndex, string cardName)
    {
        int index = -1;

        for(int i = 0; i < deck.allCards.Count; i++)
        {
            if (cardName == deck.allCards[i].name)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            return;
        }
        else if(index <= 5)
        {
            suggestionCards[cardIndex].GetComponent<Image>().sprite = playerImages.images[index];
        }
        else if(index <= 11) // it's a weapon
        {
            suggestionCards[cardIndex].GetComponent<Image>().sprite = weaponImages.images[index - 6];
        }
        else if(index <= 21) // it's a room
        {
            suggestionCards[cardIndex].GetComponent<Image>().sprite = roomImages.images[index - 12];
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

        // Make the case
        caseData.character = characterDropdown.options[characterDropdown.value].text;
        caseData.room = roomDropdown.options[roomDropdown.value].text;
        caseData.weapon = weaponDropdown.options[weaponDropdown.value].text;

        networkPlayer.Cmd_MakeAccusation(caseData);
    }

    // Make a suggestion
    public void Suggest()
    {
        Debug.Log($"Suggesting: {deck.allCards[characterDropdown.value].name} " +
           $"in {deck.allCards[weaponDropdown.value + 6].name} " +
           $"with a {deck.allCards[roomDropdown.value + 12].name}");
        
        // Make the case
        caseData.character = deck.allCards[characterDropdown.value].name;
        caseData.room = deck.allCards[weaponDropdown.value + 6].name;
        caseData.weapon = deck.allCards[roomDropdown.value + 12].name;

        // Send the accusation over the network
        networkPlayer.Cmd_MakeSuggestion(caseData);
    }

    /// <summary>
    /// The button callback for endturn
    /// </summary>
    public void EndTurn()
    {
        networkPlayer.Cmd_EndTurn();
    }

    /// <summary>
    /// Closes the rule panel
    /// </summary>
    public void CloseRulesPanel()
    {
        rulesPanel.SetActive(false);
    }

    /// <summary>
    /// Closes the rule panel
    /// </summary>
    public void OpenRulesPanel()
    {
        rulesPanel.SetActive(true);
    }


    public void NextPage()
    {
        if(currentRulePageIndex < rulePages.Count-1)
        {
            rulePages[currentRulePageIndex].SetActive(false);
            currentRulePageIndex++;
            rulePages[currentRulePageIndex].SetActive(true);
        }
    }

    public void PreviousPage()
    {
        if (currentRulePageIndex > 0)
        {
            rulePages[currentRulePageIndex].SetActive(false);
            currentRulePageIndex--;
            rulePages[currentRulePageIndex].SetActive(true);
        }
    }

    public void UpdateRoomUis(SyncListRoom rooms)
    {
        for(int i=0; i<rooms.Count; i++)
        {
            roomUis[i].roomData = rooms[i];
        }
    }
}
