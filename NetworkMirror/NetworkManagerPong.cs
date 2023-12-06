using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class NetworkManagerPong : NetworkManager
{

    public Transform leftSpawn;
    public Transform rightSpawn;
    public ScoreSystem score;

    public List<GameObject> playerList = new List<GameObject>();
  
    GameObject Ball;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
       
        

        Transform start;
        if (numPlayers == 0)
        {
            string name = "player" + Random.Range(0, 500).ToString();

            start = leftSpawn;
            ScoreSystem.instance.localName = name;

        }
        else
        {
            string name = "player" + Random.Range(0, 500).ToString();
            start = rightSpawn;
            ScoreSystem.instance.clientName = name;

        }
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        playerList.Add(player);
        NetworkServer.AddPlayerForConnection(conn, player);
        
        
        if (numPlayers == 2)
        {
            Ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(Ball);
          
        }
    

    }
    

    public void GetScore()
    {
        playerList[0].transform.position = leftSpawn.position;
        playerList[1].transform.position = rightSpawn.position;
        Ball.transform.position = transform.position;
       
    }
       
    
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        //if (Ball != null)
        //    NetworkServer.Destroy(Ball);

        base.OnServerDisconnect(conn);
        playerList.Clear();
    }
}
