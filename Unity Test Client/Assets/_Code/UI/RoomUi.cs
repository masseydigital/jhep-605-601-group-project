using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUi : MonoBehaviour
{
    public RoomData roomData;
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
        // If occupants is empty then this room is empty
        if (roomData.occupants.Count == 0)
        {
            ClearMarkers();
        }

        //Go through the number of occupants
        for (int i = 0; i < markerImages.Count; i++)
        {
            if(i < roomData.occupants.Count)
                markerImages[i].sprite = markerOptions.images[roomData.occupants[i]];
            else
                markerImages[i].sprite = null;
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
        // If we are at max capacity
        if (roomData.occupants.Count >= roomData.maxOccupancy)
            return;

        roomData.occupants.Add(playerId);
        UpdateMarkers();
    }


    // Removes a player from the room
    public void RemoveMarker(int playerId)
    {
        roomData.occupants.Remove(playerId);
        UpdateMarkers();
    }
}
