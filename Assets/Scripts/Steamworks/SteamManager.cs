using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.Text;

public class SteamManager : MonoBehaviour
{
    SteamId TargetSteamId = new SteamId();


    // Start is called before the first frame update
    void Start()
    {
        Steamworks.SteamClient.Init(1236270);
    }

    // Update is called once per frame
    void Update()
    {
        RecieveP2P();
        while (SteamNetworking.IsP2PPacketAvailable())
        {
            var packet = SteamNetworking.ReadP2PPacket();
            if (packet.HasValue)
            {
                HandleMessageFrom(packet.Value.SteamId, packet.Value.Data);
            }
        }
        Steamworks.SteamClient.RunCallbacks();
    }

    void HandleMessageFrom(SteamId steamid, byte[] data)
    {
        
    }

    void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    void RecieveP2P()
    {
        SteamNetworking.OnP2PSessionRequest = (steamid) =>
        {
            // If we want to let this steamid talk to us
            SteamNetworking.AcceptP2PSessionWithUser(steamid);
        };
    }

    void SendServerMessageToAll(string message)
    {
        byte[] mydata = Encoding.ASCII.GetBytes(message);

        var sent = SteamNetworking.SendP2PPacket(TargetSteamId, mydata);

        // if sent is true - the data was sent !
    }
}
