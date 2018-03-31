using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class CreateMatchPanelUi : MonoBehaviour
{
	[SerializeField]
	private InputField matchNameTf;

	[SerializeField]
	private Text errorLbl;

	[SerializeField]
	private Button createMatchBtn;
	
	void Update ()
	{
		createMatchBtn.enabled = !matchNameTf.text.Equals("");
	}

	public void CreateMatchBtn()
	{

	}

	private void OnInternetMatchCreate(bool success, string extendedinfo, MatchInfo responsedata)
	{
		Debug.Log("Trying to make a server");
		if (success)
		{
			MatchInfo info = responsedata;
			NetworkServer.Listen(info, 9000);
		}
		else
		{
			errorLbl.text = "Couldn't connect!";
		}
	}
}
