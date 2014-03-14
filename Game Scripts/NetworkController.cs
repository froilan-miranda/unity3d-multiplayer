using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

public class NetworkController : MonoBehaviour
{		
    private ClientHelper cm = null;
    internal bool serverConnected = false;

    // connect to the server
    private void Connect(string serverIp)
    {
        if (cm == null){
            cm = new ClientHelper();
            cm.Connect(serverIp, 4444);
            cm.DataRecieved += new ClientHelper.DataRecievedEventHandler(cm_DataRecieved);
        }
    }

    // we got a message
    private void cm_DataRecieved(String data, Protocol protocol)
    {
        if (protocol == Protocol.TCP){
            // we got a tcp message
             Debug.Log("we got a tcp message");
        }else if (protocol == Protocol.UDP){
            //we got a udp message
            Debug.Log("we got a udp message: ");
        }
    }

    internal String[] GetMessages()
    {
        if(cm != null){
            String[] messageQ = cm.GetMessages();
            return messageQ;
        } else {
            return null;
        }
    }

    // send a message to clients
    public void SendClient(string message)
    {
    cm.Send(message, PotentialDestination.client, Protocol.TCP);
    }

    // send a message to host
    public void SendHost(string message)
    {
    cm.Send(message, PotentialDestination.host, Protocol.TCP);
    }

    // send a message to leaderboard
    public void SendLeaderboard(string message)
    {
    cm.Send(message, PotentialDestination.leaderboard, Protocol.TCP);
    }

    // send a message to everyohne
    public void SendGlobal(string message)
    {
    cm.Send(message, PotentialDestination.global, Protocol.TCP);
    }

    // send a message to clients
    public void SendClientUDP(string message)
    {
    cm.Send(message, PotentialDestination.client, Protocol.UDP);
    }

    // send a message to host
    public void SendHostUDP(string message)
    {
    cm.Send(message, PotentialDestination.host, Protocol.UDP);
    }

    // send a message to leaderboard
    public void SendLeaderboardUDP(string message)
    {
    cm.Send(message, PotentialDestination.leaderboard, Protocol.UDP);
    }

    public void SendGlobalUDP(string message)
    {
        cm.Send(message, PotentialDestination.global, Protocol.UDP);
    }
    void Update ()
    {
    }
    //
    //SendHost ("hi host");
    //SendLeaderboard ("hi leaderboard");
    //SendGlobalUDP("hi everyone");

    void Start ()
    {
        //Connect ("10.0.0.79");
    }
    internal void ConnectGameServer()
    {
        //Connect("10.0.0.49");
        Connect(GetComponent<SettingsController>().GetServerIP());
        serverConnected = true;
        Debug.Log("serverConnected" + serverConnected);
    }

    void OnApplicationQuit ()
    {

    }
}
