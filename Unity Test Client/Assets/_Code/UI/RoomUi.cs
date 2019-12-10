using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClueLess;

public class RoomUi : MonoBehaviour
{
    public ClueLess.Room roomData;
    public List<Image> markerImages;
    public HoverControl hoverControl;
    public ImageOptions markerOptions;

    public void Start()
    {
        ClearMarkers();
    }

    // Initializes the board to have nothing on it
    public void InitiliazeBoard()
    {
        ClearMarkers();
    }

    // Blanket update of the markers
    public void UpdateMarkers()
    { 
        //Go through the number of occupants
        for (int i = 0; i < roomData.occupants.Length; i++)
        {
            if(roomData.occupants[i] != -1)
            {
                markerImages[i].sprite = markerOptions.images[roomData.occupants[i]];
            }
            else
            {
                markerImages[i].sprite = null;
            }    
        }
    }

    /// <summary>
    /// Clears all the markers
    /// </summary>
    public void ClearMarkers()
    {
        for (int i = 0; i < markerImages.Count; i++)
        {
            markerImages[i].sprite = null;
        }
    }

    // Adds a new player to the room
    public void AddMarker(int playerId)
    {
        for(int i=0; i<roomData.maxOccupancy; i++)
        {
            // If  we have an empty space
            if(roomData.occupants[i] == -1)
            {
                roomData.AddPlayer(playerId);
                UpdateMarkers();
                return;
            }
        }

        Debug.Log("Room is full!");
    }


    // Removes a player from the room
    public void RemoveMarker(int playerId)
    {
        roomData.RemovePlayer(playerId);
        UpdateMarkers();
    }
}
