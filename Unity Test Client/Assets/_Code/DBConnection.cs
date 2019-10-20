using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon;
using TMPro;

public class DBConnection : MonoBehaviour
{
    public int id=0;
    public string theme;
    public List<string> characterNames;
    public List<string> weaponNames;
    public List<string> roomNames;
    public TextMeshProUGUI displayText;
    public GameData gameData;

    private string region = RegionEndpoint.USEast2.SystemName;
    private static AmazonDynamoDBClient _ddbClient;
    private AWSCredentials _credentials;
    private DynamoDBContext _context;
    private Table _table;

    public DatabaseTest testDatabase;

    const string TABLENAME = @"clueless-game-data";

    // Initialize the Amazon Cognito credentials provider
    CognitoAWSCredentials credentials;

    // The region our DB is in
    private RegionEndpoint _DynamoRegion
    {
        get { return RegionEndpoint.GetBySystemName(region); }
    }

    // Before starts happen
    private void Awake()
    {
        gameData = new GameData();

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE
        // This is required to utilize SDK
        UnityInitializer.AttachToGameObject(this.gameObject);

        // change the http client...
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        // get your cognito credentials
        credentials = new CognitoAWSCredentials(
        "us-east-2:ccbdfc88-c19a-465b-aeac-c917fa1c30aa", // Identity pool ID
        RegionEndpoint.USEast2 // Region
        );

        // initialize the client
        _ddbClient = new AmazonDynamoDBClient(credentials, _DynamoRegion);

        // initialize the context
        _context = new DynamoDBContext(_ddbClient);

        // Describe the Table
        //DescribeTable();

        //Create a new game data entry
        //CreateData(4);

        //Retrieve the game data
        RetrieveData(id);
#endif
#if UNITY_EDITOR
        testDatabase = Resources.Load("_Data/TestDatabase") as DatabaseTest;

        // This only works in the editor
        gameData.id = testDatabase.database[id].id;
        gameData.theme = testDatabase.database[id].theme;
        gameData.characterNames = testDatabase.database[id].characters.characters;
        gameData.roomNames = testDatabase.database[id].rooms.rooms;
        gameData.weaponNames = testDatabase.database[id].weapons.weapons;

        id = int.Parse(testDatabase.database[id].id);
        theme = testDatabase.database[id].theme;
        characterNames = testDatabase.database[id].characters.characters;
        roomNames = testDatabase.database[id].rooms.rooms;
        weaponNames = testDatabase.database[id].weapons.weapons;
#endif
#if UNITY_WEBGL
        // TODO: Replace with actual server data
        gameData.id = "0";
        gameData.theme = "Default";
        gameData.characterNames = new List<string>()
        {
            "Col Mustard",
            "Ms Scarlet",
            "Mr Green",
            "Mrs Peacock",
            "Mrs White",
            "Prof Plum"
        };

        gameData.roomNames = new List<string>()
        {
            "Ball Room",
            "Billiard Room",
            "Conservatory",
            "Dining Room",
            "Hall",
            "Kitchen",
            "Library",
            "Lounge",
            "Study"
        };
        gameData.weaponNames = new List<string>()
        {
            "Axe",
            "Goblet",
            "Knife",
            "Lead Pipe",
            "Pistol",
            "Rope"
        };

        id = int.Parse(gameData.id);
        theme = gameData.theme;
        characterNames = gameData.characterNames;
        roomNames = gameData.roomNames;
        weaponNames = gameData.weaponNames;
#endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This pulls the table information from DynamoDB
    // This method should be used to verify DynamoDB connection
    public void DescribeTable()
    {
        if(displayText != null)
            displayText.text += ("\n*** Retrieving Table Information ***\n");

        var request = new DescribeTableRequest
        {
            TableName = TABLENAME
        };

        _ddbClient.DescribeTableAsync(request, (result) =>
        {
            if(result.Exception != null)
            {
                if (displayText != null)
                    displayText.text += result.Exception.Message;
                Debug.Log(result.Exception);
                return;
            }

            var response = result.Response;
            TableDescription description = response.Table;
            if (displayText != null)
                displayText.text += ("Name: " + description.TableName + "\n");
                displayText.text += ("# of items: " + description.ItemCount + "\n");
                displayText.text += ("Provision Throughput (reads/sec): " +
                    description.ProvisionedThroughput.ReadCapacityUnits + "\n");
                displayText.text += ("Provision Throughput (reads/sec): " +
                    description.ProvisionedThroughput.WriteCapacityUnits + "\n");
        }, null);

    }

    // This method creates a new game data
    // This one is the default creator
    private void CreateData(int id)
    {
        GameData newGameData = new GameData();

        newGameData.id = id.ToString();
        newGameData.theme = "Test";

        List<string> characters = new List<string>();
        List<string> rooms = new List<string>();
        List<string> weapons = new List<string>();

        for(int i=0; i<6; i++)
        {
            characters.Add($"Character {i}");
            weapons.Add($"Weapon {i}");
        }

        for(int i=0; i<9; i++)
        {
            rooms.Add($"Room {i}");
        }

        newGameData.characterNames = characters;
        newGameData.roomNames = rooms;
        newGameData.weaponNames = weapons;

        _context.SaveAsync(newGameData, (result) =>
        {
            if (result.Exception == null)
                if (displayText != null)
                    displayText.text += @"Game Data Saved";
            else
            {
                if (displayText != null)
                    displayText.text += ("LoadAsync error" + result.Exception.Message);
                Debug.LogException(result.Exception);
                return;
            }
        });
    }

    // This method retrieves data from the table
    // pass id, retrieve character names, room names, and weapon names
    public void RetrieveData(int id)
    {
#if UNITY_STANDALONE
        string displayMessage = "";

        displayMessage += $"\n*** Loading Game Data**\n";

        _context.LoadAsync<GameData>(
            id.ToString(),
            (result) =>
        {
        if (result.Exception != null)
        {
                if (displayText != null)
                    displayMessage += ("LoadAsync error" + result.Exception.Message);
            Debug.LogException(result.Exception);
            return;
        }

            gameData = result.Result as GameData;

            characterNames = gameData.characterNames;
            roomNames = gameData.roomNames;
            weaponNames = gameData.weaponNames;
            theme = gameData.theme;

            GameManager.gameData = gameData;

            displayMessage += "Successfully completed loading game data";

            if (displayText != null)
                displayText.text += displayMessage;
         
        }, null);
#endif
#if EDITOR
        // This only works in the editor
        gameData.id = testDatabase.database[id].id;
        gameData.theme = testDatabase.database[id].theme;
        gameData.characterNames = testDatabase.database[id].characters.characters;
        gameData.roomNames = testDatabase.database[id].rooms.rooms;
        gameData.weaponNames = testDatabase.database[id].weapons.weapons;
#endif
#if UNITY_WEBGL
        switch (id)
        {
            case (0):
                gameData.id = "0";
                gameData.theme = "Default";
                gameData.characterNames = new List<string>()
        {
            "Col Mustard",
            "Ms Scarlet",
            "Mr Green",
            "Mrs Peacock",
            "Mrs White",
            "Prof Plum"
        };

                gameData.roomNames = new List<string>()
        {
            "Ball Room",
            "Billiard Room",
            "Conservatory",
            "Dining Room",
            "Hall",
            "Kitchen",
            "Library",
            "Lounge",
            "Study"
        };
                gameData.weaponNames = new List<string>()
        {
            "Axe",
            "Goblet",
            "Knife",
            "Lead Pipe",
            "Pistol",
            "Rope"
        };
                break;

            case (1):
                gameData.id = "1";
                gameData.theme = "Marvel";
                gameData.characterNames = new List<string>()
        {
            "Black Widow",
            "Captain America",
            "Hawkeye",
            "Hulk",
            "Iron Man",
            "Thor"
        };

                gameData.roomNames = new List<string>()
        {
            "Multiverse",
            "Knowhere",
            "New York City",
            "Nine Realms",
            "Sanctuary",
            "Stark Tower",
            "Titan",
            "Vormir",
            "Wakanda"
        };
                gameData.weaponNames = new List<string>()
        {
            "Arc Blast",
            "Arrows",
            "Infinity Stones",
            "Mjolnir",
            "Shield",
            "Web Slingers"
        };
                break;

            case (2):
                gameData.id = "2";
                gameData.theme = "Star Wars";
                gameData.characterNames = new List<string>()
        {
            "Boba Fett",
            "Chewbacca",
            "Darth Vader",
            "Emperor Palpatine",
            "Han Solo",
            "Luke Skywalker"
        };

                gameData.roomNames = new List<string>()
        {
            "Alderaan",
            "Dagobah",
            "Death Star",
            "Endor",
            "Hoth",
            "Kashyyyk",
            "Mustafar",
            "Tatooine",
            "Yavin IV"
        };
                gameData.weaponNames = new List<string>()
        {
            "Blaster",
            "Bowcaster",
            "Electricity",
            "Flamethrower",
            "Lightsaber",
            "Vibroblade"
        };
                break;

            default:
                break;
        }

        
#endif
    }

    // This method updates data in a table
    private void UpdateData(int id)
    {
        //TODO: Add update functionality
    }
}
