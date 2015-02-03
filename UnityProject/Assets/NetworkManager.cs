using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private const string typeName = "UniqueGameName";
	private const string gameName = "SpaceShooter";
	private HostData[] hostList;
	public GameObject playerPrefab;
	public GameObject asteroidsPrefab;
	private GameObject InstantiatedObject;
	private GameObject go;
	private LineRenderer lr;
	private bool serverStart = false;
	
	void Start(){
		lr = new LineRenderer ();
		GameObject newLine = new GameObject("Line");
		lr = newLine.AddComponent<LineRenderer>();
		lr.SetVertexCount (2);
		lr.SetWidth (0.1f,0.1f);
	}
	
	private void Update(){
		if(GameObject.Find("ship")!=null)
			lr.SetPosition(0, GameObject.Find("ship").transform.position);
		if(GameObject.Find("spaceShip(Clone)")!=null)
			lr.SetPosition(1, GameObject.Find("spaceShip(Clone)").transform.position);
	}
	
	private void SpawnPlayer()
	{
		var clone = Network.Instantiate(playerPrefab, new Vector3(10.84378f, -0.02763581f, 1.295858f), Quaternion.AngleAxis(90, Vector3.up), 0);
		clone.name = "ship";
	}
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}
	
	private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{
		Network.Instantiate(asteroidsPrefab, new Vector3(10.84378f, -0.02763581f, 1.295858f), Quaternion.AngleAxis(90, Vector3.up), 0);
		SpawnPlayer();
	}
	
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		Network.Disconnect();
		MasterServer.UnregisterHost();
		Application.LoadLevel("menu");
	}
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(25, 25, 120, 25), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(25, 60, 120, 25), "Refresh Hosts"))
				RefreshHostList();
			
			if (GUI.Button(new Rect(25, 95, 120, 25), "Menu"))
				Application.LoadLevel("menu");
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}
}
