using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.Playables;


public class PlayerController : NetworkBehaviour
{
    Camera cam;

    NetworkManagerPong netPong;
    public Rigidbody2D rb;
    private SpriteRenderer playerMaterialClone;
    public string _clientName;
    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;
    public float moveX;
    public float moveZ;
    ScoreSystem score;

     
   

    void OnColorChanged(Color _Old, Color _New)
    {

        playerMaterialClone = GetComponent<SpriteRenderer>();
        playerMaterialClone.color = _New;
        GetComponent<SpriteRenderer>().color = playerMaterialClone.color;
    }

    private void Start()
    {
        netPong = GameObject.Find("NetworkManager").GetComponent<NetworkManagerPong>();
     
    }

    public override void OnStartLocalPlayer()
    {
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //string name = "player" + Random.Range(0, 500).ToString();
        CmdSetupPlayer(color,name);

        if (!isClientOnly)
        {
            cam = GameObject.Find("Camera").GetComponent<Camera>();
            cam.enabled = true;
            //ScoreSystem.instance.localName = name;
           

        }
        else
        {
            cam = GameObject.Find("SecondCamera").GetComponent<Camera>();
            cam.enabled = true;
            //_clientName = name;
           
        }
        //ScoreSystem.instance.clientName = _clientName;

    }

    [Command]
    public void CmdSetupPlayer( Color _col,string name)
    {
   
        playerColor = _col;
       

    }

    public void CmdMovementInput(float moveX, float moveY)
    {
        Vector2 direct = new Vector2(moveX,moveY);
        rb.velocity = (direct.x * cam.transform.right + direct.y * cam.transform.up) * 20;
    }

    private void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        

        if (!isLocalPlayer)
        {
            return;
        }
        CmdMovementInput(moveX, moveZ);
    }
}
