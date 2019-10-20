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

[DynamoDBTable("clueless-game-data")]
public class GameData
{
    [DynamoDBHashKey] public string id { get; set; }
    [DynamoDBProperty] public string theme { get; set; }
    [DynamoDBProperty("characterNames")] public List<string> characterNames { get; set; }
    [DynamoDBProperty("roomNames")] public List<string> roomNames { get; set; }
    [DynamoDBProperty("weaponNames")] public List<string> weaponNames { get; set; }
}
