using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine.UI;
public class SteamLobby : MonoBehaviour
{
    //callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;
    protected Callback<LobbyMatchList_t> lobbyMatchListCallback;

    //variables
    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private NetworkManagerPong manager;

   
    //GameObject
    public GameObject HostButton;
    public TextMeshProUGUI LobbyNameText;
    
    private void Start()
    {
        //SteamAPI.Init();
        if (!SteamManager.Initialized) { return; }

        manager =  GetComponent<NetworkManagerPong>();

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        SteamMatchmaking.AddRequestLobbyListStringFilter("exampleFilter", "exampleValue", ELobbyComparison.k_ELobbyComparisonEqual);
        lobbyMatchListCallback = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);


        //JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
       

    }

    public void JoinLobby()
    {
        SteamMatchmaking.RequestLobbyList();
       
    }
    void OnLobbyMatchList(LobbyMatchList_t callback)
    {
        // Get the number of lobbies found
        uint lobbyCount = callback.m_nLobbiesMatching;
       
        if (lobbyCount > 0)
        {
            // Choose a random lobby index
            int randomIndex = Random.Range(0, (int)lobbyCount);
            
            // Get the lobby information
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(randomIndex);

            SteamMatchmaking.JoinLobby(lobbyId);
            // Join the randomly selected lobby

        }
        else
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, manager.maxConnections);

        }
    }
    
  

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK) 
        {
            HostButton.SetActive(true);
            return; 
        }

        Debug.Log("Lobby Created Succesfully");

        manager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
      
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request to join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //everyone
        //HostButton.SetActive(false);
        //CurrentLobbyID = callback.m_ulSteamIDLobby;
        //LobbyNameText.gameObject.SetActive(true);
        //LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");
       
      
        //client
        if (NetworkServer.active)
            return;

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),HostAddressKey);
        manager.networkAddress = hostAddress;

        manager.StartClient();

        HostButton.SetActive(false);
    }
}
