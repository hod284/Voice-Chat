using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Text ip;
    [SerializeField] private Text port;
    [SerializeField] private ConnectionManger ConnectionManger;
    private void Start()
    {
        button1?.onClick.AddListener(() => server());
        button2?.onClick.AddListener(() => client());
        button3?.onClick.AddListener(() => host());
        UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if(ip != null)
        ip.text = transport.ConnectionData.Address.ToString();
        if(port !=null)
        port.text = transport.ConnectionData.Port.ToString();
    }
    public void server()
    {
        ConnectionManger.StartSever();
        off();
    }
    public void client ()
    {
        ConnectionManger.StartClient();
        off();
    }
    public void host()
    {
        ConnectionManger.StartHOST();
        off();
    }
    public void off()
    {
        if (ConnectionManger.ConnnectingSucess_Public)
        {
            button1.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);
        }
    }
}
