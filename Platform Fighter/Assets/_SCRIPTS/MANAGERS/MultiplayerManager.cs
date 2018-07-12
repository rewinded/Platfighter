﻿using PlatFighter.PLAYER;
using UnityEngine;
using UnityEngine.Networking;

namespace PlatFighter.MANAGERS
{
    public class MultiplayerManager : NetworkManager
    {
        public override void OnClientConnect(NetworkConnection conn)
        {
            ClientScene.AddPlayer(conn, (short) conn.connectionId);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            Debug.Log(playerControllerId);
            var player = Instantiate
            (
                playerPrefab, 
                GameManager.Instance.spawnPoints[playerControllerId].position, 
                GameManager.Instance.spawnPoints[playerControllerId].rotation
            );
            player.GetComponent<PlayerInfo>().color = Color.red;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}