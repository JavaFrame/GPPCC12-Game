using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public static MainMenu Instance
	{
		get;
		private set;
	}

	private PlayerClassInitLobbyManager lobby;

	[SerializeField]
	private GameObject mainPanelGo, joinMatchGo, createMatchGo, lobbyGo, messageGo;

	[Space]
	[Header("LAN Panel")]

	[SerializeField]
	private InputField lanCreateIpTf;

	[SerializeField]
	private InputField lanCreatePortTf;

	[SerializeField]
	private Button lanCreateBtn;

	[Space]
	[SerializeField]
	private InputField lanJoinIpTf;

	[SerializeField]
	private InputField lanJoinPortTf;

	[SerializeField]
	private Button lanJoinBtn;

	[Space]
	[Header("Lobby Panel")]

	[SerializeField]
	private InputField lobbyPlayerName;

	[SerializeField]
	private Dropdown lobbyClassDropdown;

	[SerializeField]
	private Toggle lobbReadyToggle;

	[SerializeField]
	private Text lobbyConnectedPlayerText;

	[SerializeField]
	private Transform lobbyPlayerListContent;

	[SerializeField]
	private GameObject lobbyPlayerListEntryPrefab;

	private Dictionary<MockPlayer, GameObject> lobbyPlayerDictionary = new Dictionary<MockPlayer, GameObject>();

	//message 
	[Space]
	[Header("Message Panel")]
	[SerializeField]
	private Text messageText;

	[SerializeField]
	private Button messageBackBtn;

	public Dropdown LobbyClassDropdown
	{
		get { return lobbyClassDropdown; }
	}

	public InputField LobbyPlayerName
	{
		get { return lobbyPlayerName; }
	}

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		lobby = PlayerClassInitLobbyManager.Instance;
		ShowMainPanel();
		lobbyPlayerName.onValueChanged.AddListener(name => { MockPlayer.LocalMockPlayer.PlayerName = name; });
		DontDestroyOnLoad(this.gameObject);
	}

	void Update()
	{
		//lan
		lanCreateBtn.enabled = !lanCreateIpTf.text.Trim().Equals("") && !lanCreatePortTf.text.Trim().Equals(""); 
		lanJoinBtn.enabled = !lanJoinIpTf.text.Trim().Equals("") && !lanJoinPortTf.text.Trim().Equals(""); 
	}

	public void ShowLanPanel()
	{
		HideAll();
		joinMatchGo.SetActive(true);
	}

	public void ShowMainPanel()
	{
		HideAll();
		mainPanelGo.SetActive(true);
	}

	public void ShowMatchmakingPanel()
	{
		HideAll();
		createMatchGo.SetActive(true);

	}

	public void ShowLobbyPanel()
	{
		HideAll();
		lobbyGo.SetActive(true);
	}

	public void ShowMessagePanel(string text = "", Action backDelegater = null)
	{
		HideAll();
		messageGo.SetActive(true);
		SetMessageText(text);
		messageBackBtn.gameObject.SetActive(backDelegater != null);
		messageBackBtn.onClick.RemoveAllListeners();
		messageBackBtn.onClick.AddListener(() =>
		{
			if (backDelegater != null)
				backDelegater.Invoke();
		});
	}

	private void HideAll()
	{
		joinMatchGo.SetActive(false);
		mainPanelGo.SetActive(false);
		createMatchGo.SetActive(false);
		lobbyGo.SetActive(false);
		messageGo.SetActive(false);
	}

	//lan panel
	public void CreateLanMatch()
	{
		lobby.matchHost = lanCreateIpTf.text;
		lobby.matchPort = Convert.ToInt32(lanCreatePortTf.text);
		lobby.StartHost();
		ShowMessagePanel("Creating Server...", () =>
		{
			ShowLanPanel();
			lobby.StopHost();
		});
	}

	public void JoinLanMatch()
	{
		lobby.matchHost = lanJoinIpTf.text;
		lobby.matchPort = Convert.ToInt32(lanJoinPortTf.text);
		lobby.StartClient();
		ShowMessagePanel("Connecting to " + lobby.matchHost + ":" + lobby.matchPort, () =>
		{
			ShowLanPanel();
			lobby.StopClient();
		});

	}


	//lobby
	public void LeaveLobby()
	{
		ShowMainPanel();
		lobby.StopClient();
		lobby.StopHost();
	}


	public void AddPlayerToLobbyList(MockPlayer p)
	{ 
		var entry = Instantiate(lobbyPlayerListEntryPrefab, lobbyPlayerListContent);
		entry.GetComponent<LobbyPlayerListEntryUi>().Player = p;
		lobbyPlayerDictionary.Add(p, entry);
		lobbyConnectedPlayerText.text = lobbyPlayerDictionary.Count + " / " + lobby.maxPlayers + " Players";
	}

	public void RemovePlayerFromLobbyList(MockPlayer p)
	{
		Destroy(lobbyPlayerDictionary[p]);
		lobbyPlayerDictionary.Remove(p);
		lobbyConnectedPlayerText.text = lobbyPlayerDictionary.Count + " / " + lobby.maxPlayers + " Players";
	}

	public void ReadyToggleChanged(bool val)
	{
		//MockPlayer.LocalMockPlayer.readyToBegin = val; 
		MockPlayer.LocalMockPlayer.SendReadyToBeginMessage();
	}

	//mesage panel
	public void SetMessageText(string text)
	{
		messageText.text = text;
	}
}
