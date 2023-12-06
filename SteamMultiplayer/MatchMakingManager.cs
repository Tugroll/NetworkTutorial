using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingManager : MonoBehaviour
{
    private Callback<LobbyMatchList_t> lobbyMatchListCallback;

    void Start()
    {
        // Initialize Steamworks
      

        // Register callback for lobby match list
        lobbyMatchListCallback = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);

        // Start lobby search (you might add filters here)
        SteamMatchmaking.AddRequestLobbyListStringFilter("exampleFilter", "exampleValue", ELobbyComparison.k_ELobbyComparisonEqual);
        SteamMatchmaking.RequestLobbyList();
    }

    public void OnLobbyMatchList(LobbyMatchList_t callback)
    {
        // Get the number of lobbies found
        uint lobbyCount = callback.m_nLobbiesMatching;

        if (lobbyCount > 0)
        {
            // Choose a random lobby index
            int randomIndex = Random.Range(0, (int)lobbyCount);

            // Get the lobby information
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(randomIndex);

            // Join the randomly selected lobby
            SteamMatchmaking.JoinLobby(lobbyId);
        }
        else
        {
            Debug.Log("No lobbies found.");
        }
    }

    void OnDisable()
    {
        // Ensure Steamworks is properly shutdown
        SteamAPI.Shutdown();
    }
}
