using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class SettingsController : MonoBehaviour 
{

	private WWW www;

	// Use this for initialization
	public IEnumerator Start () {
		Debug.Log("in the begining");
		//Load XML data from a URL
		string url = "file://" + Application.dataPath + "/settings.ini";
		www = new WWW(url);
		yield return www;
		Debug.Log(www.text);

		//now that settings are loaded tell game logic to continue
		GetComponent<GameController>().InitGame();
	}

	internal float GetSetting(string nodeName)
	{
		Debug.Log("Getting Setting");

		float setting = 0.0f;

		if(www != null){
			XmlTextReader reader = new XmlTextReader(new StringReader(www.text));
			reader.WhitespaceHandling = WhitespaceHandling.None;

			while(reader.Read()){
				if(reader.Name == nodeName){
					Debug.Log(reader.Name + " value = " + reader.GetAttribute("value"));
					setting = float.Parse(reader.GetAttribute("value"));
					break;
				}
			}
			reader.Close();
		}else{
			Debug.Log("null ini file");
		}

		return setting;
	}
	// The server ip gets it's own settings call due to its need to return a string
	// If this becomes a pattern the GetSetting function will need to be reworked
	internal string GetServerIP()
	{
		Debug.Log("Getting Setting");

		string setting = null;
		if(www != null){
			XmlTextReader reader = new XmlTextReader(new StringReader(www.text));
			reader.WhitespaceHandling = WhitespaceHandling.None;

			while(reader.Read()){
				if(reader.Name == "serverIP"){
					Debug.Log(reader.Name + " value = " + reader.GetAttribute("value"));
					setting = reader.GetAttribute("value");
				}
			}
			reader.Close();
		}else{
			Debug.Log("null ini file");
		}
		
		return setting;
	}

}//class
