using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ExampleNetworkDiscovery))]
[RequireComponent(typeof(NetworkManager))]

public class ConnectionManger : MonoBehaviour
{
    [SerializeField] private ExampleNetworkDiscovery m_Discovery;

    [SerializeField] private NetworkManager m_NetworkManager;

    [SerializeField] private Dictionary<IPAddress, DiscoveryResponseData> discoveredServers = new Dictionary<IPAddress, DiscoveryResponseData>();
    private Coroutine StartClientConnection;
    private bool ConnnectingSucess=false;
    public bool ConnnectingSucess_Public { get => ConnnectingSucess; }
    private string SceenName= "NFGO Game World";
    private LoadSceneMode M_LoadSceneMode = LoadSceneMode.Single;
    void Awake()
    {
        m_Discovery = GetComponent<ExampleNetworkDiscovery>();
        m_NetworkManager = GetComponent<NetworkManager>();
        m_NetworkManager.OnServerStarted += OnServerStarted;
    }
    private  void OnServerStarted()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(SceenName, M_LoadSceneMode);
    }
    public void SetNetworkloadSceen(string Name, LoadSceneMode loadSceneMode)
    {
        SceenName =name;
        M_LoadSceneMode = loadSceneMode;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (m_Discovery == null) // This will only happen once because m_Discovery is a serialize field
        {
            m_Discovery = GetComponent<ExampleNetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(m_Discovery.OnServerFound, OnServerFound);
            Undo.RecordObjects(new Object[] { this, m_Discovery }, "Set NetworkDiscovery");
        }
    }
#endif

    void OnServerFound(IPEndPoint sender, DiscoveryResponseData response)
    {
        discoveredServers[sender.Address] = response;
    }
    public void StartHOST()
    {
        m_Discovery.StartServer();
        SetConnettion();
        m_NetworkManager.StartHost();
    }
    public void StartSever()
    {
        m_Discovery.StartServer();
        SetConnettion();
        m_NetworkManager.StartServer();
    }
    public void StopNetcode()
    {
      NetworkManager.Singleton.Shutdown(true);
        m_Discovery.StopDiscovery();
        StartClientConnection =  null;
    }
    public void StartClient()
    {
        StartClientConnection = StartCoroutine( Connectiong());
        if (ConnnectingSucess)
            StartClientConnection = null;
    }
    private IEnumerator Connectiong()
    {
        int n = 0;
        m_Discovery.StartClient();
        m_Discovery.ClientBroadcast(new DiscoveryBroadcastData());
        float TIME = 0;
        while (TIME <10.0F)
        {
            TIME += Time.deltaTime * 1.0f;
            foreach (var discoveredServer in discoveredServers)
            {
                if (n == 0)
                {
                    UnityTransport transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;
                    transport.SetConnectionData(discoveredServer.Key.ToString(), discoveredServer.Value.Port);
                    TIME = 10.0F;
                    n = 1;
                }
                else
                    break;
            }
            yield return null;
        }
        if (n == 1)
        {
            m_NetworkManager.StartClient();
            ConnnectingSucess = true;
        }
        else
        {
            ConnnectingSucess = false;
            Debug.LogError("MAKING THE HOST");
        }
    }
    private void SetConnettion()
    {
        UnityTransport transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;
        IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
        for (int i = 0; i < host.AddressList.Length; i++)
        {
            if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
            {
                transport.SetConnectionData(host.AddressList[i].ToString(), 7777);
            }
        }
    }

}
