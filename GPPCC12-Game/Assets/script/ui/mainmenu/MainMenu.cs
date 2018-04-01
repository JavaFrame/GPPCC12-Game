using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
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
	private GameObject mainPanelGo, joinMatchGo, matchMakingGo, lobbyGo, messageGo;

	[Space]
	[Header("Matchmaking Panel")]

	[SerializeField]
	private Transform mmJoinMatchListSpawnEntry;

	[SerializeField]
	private GameObject mmJoinMatchListEntryPrefab;

	[SerializeField]
	private InputField mmCreateMatchName;

	[SerializeField]
	private Button mmCreateMatchBtn;

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
		if(Instance != null)
			Destroy(Instance.gameObject);
		Instance = this;
	}

	void Start()
	{
		lobby = PlayerClassInitLobbyManager.Instance;
		ShowMainPanel();
		lobbyPlayerName.onValueChanged.AddListener(name => { MockPlayer.LocalMockPlayer.PlayerName = name; });
		DontDestroyOnLoad(this.gameObject);

		lobby.StartMatchMaker();
	}

	void Update()
	{
		//lan
		lanCreateBtn.interactable = !lanCreateIpTf.text.Trim().Equals("") && !lanCreatePortTf.text.Trim().Equals(""); 
		lanJoinBtn.interactable = !lanJoinIpTf.text.Trim().Equals("") && !lanJoinPortTf.text.Trim().Equals(""); 

		//mm
		mmCreateMatchBtn.interactable = !mmCreateMatchName.text.Trim().Equals("");
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
		matchMakingGo.SetActive(true);
		RefreshMMList();
	}

	public void ShowLobbyPanel()
	{
		HideAll();
		lobbyGo.SetActive(true);
	}

	public void ShowMessagePanel(string text = "", Action backDelegater = null, bool overrideBackDelegator = true)
	{
		HideAll();
		messageGo.SetActive(true);
		SetMessageText(text);
		if (overrideBackDelegator)
		{
			messageBackBtn.gameObject.SetActive(backDelegater != null);
			messageBackBtn.onClick.RemoveAllListeners();
			messageBackBtn.onClick.AddListener(() =>
			{
				if (backDelegater != null)
					backDelegater.Invoke();
			});
		}
	}

	private void HideAll()
	{
		joinMatchGo.SetActive(false);
		mainPanelGo.SetActive(false);
		matchMakingGo.SetActive(false);
		lobbyGo.SetActive(false);
		messageGo.SetActive(false);
	}

	//matchmaking (mm) panel

	public void RefreshMMList()
	{
		lobby.matchMaker.ListMatches(0, 10, "", true, 0, 0, (success, info, matches) =>
		{
			if (success)
			{
				//Clearing the match list
				foreach(Transform child in mmJoinMatchListSpawnEntry)
					GameObject.Destroy(child.gameObject);

				foreach (MatchInfoSnapshot ms in matches)
				{
					GameObject entry = Instantiate(mmJoinMatchListEntryPrefab, mmJoinMatchListSpawnEntry);
					MatchMakeMatchUi entryController = entry.GetComponent<MatchMakeMatchUi>();
					entryController.MatchInfo = ms;
				}
			}
		});
	}

	public void CreateMMMatch()
	{
		ShowMessagePanel("Creating a match...", () =>
		{
			ShowMatchmakingPanel();
			lobby.StopHost();
		});
		string mmName = mmCreateMatchName.text;
		lobby.matchMaker.CreateMatch(mmName, 4, true, "", "", "", 0, 0, (success, info, matchInfo) =>
		{
			if (success)
			{
				MatchInfo hostInfo = matchInfo;
				lobby.StartHost(hostInfo);
			}
			else
			{
				Debug.LogError("Creating a match failed because: " + info);
				SetMessageText("Creating a match failed because: " + info);
			}
		});

	}

	//lan panel
	public void CreateLanMatch()
	{
		lobby.networkAddress = lanCreateIpTf.text;
		lobby.networkPort = Convert.ToInt32(lanCreatePortTf.text);
		lobby.StartHost();
		ShowMessagePanel("Creating Server on port " + lobby.networkPort +" ...", () =>
		{
			ShowLanPanel();
			lobby.StopHost();
		});
	}

	public void JoinLanMatch()
	{
		lobby.networkAddress = lanJoinIpTf.text;
		lobby.networkPort = Convert.ToInt32(lanJoinPortTf.text);
		lobby.StartClient();
		ShowMessagePanel("Connecting to " + lobby.networkAddress + ":" + lobby.networkPort, () =>
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
		if (lobbyPlayerDictionary.ContainsKey(p))
		{
			Destroy(lobbyPlayerDictionary[p]);
			lobbyPlayerDictionary.Remove(p);
		}

		lobbyConnectedPlayerText.text = lobbyPlayerDictionary.Count + " / " + lobby.maxPlayers + " Players";
	}

	public void ReadyToggleChanged(bool val)
	{
		MockPlayer.LocalMockPlayer.readyToBegin = val; 
		if(val)
			MockPlayer.LocalMockPlayer.SendReadyToBeginMessage();
		else
			MockPlayer.LocalMockPlayer.SendNotReadyToBeginMessage();
		lobbyPlayerName.interactable = !val;
		lobbyClassDropdown.interactable = !val;
	}

	//mesage panel
	public void SetMessageText(string text)
	{
		messageText.text = text;
	}
}
