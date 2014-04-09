using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static int currentDeviceOrientation = 4;
	public static int maxLevel = 10;
	public static int maxChapter = 4;

	public enum MenuType {
		None			= -1,
		MainMenu   	    = 0,
		Login	        = 1,
		LevelEditor		= 2,
		ChapterSelect	= 3,
		Chapter1		= 4,
		FacebookMenu	= 5
	}

	private static bool created = false;

	private static int chapterNumber;
	private static int levelNumber;
	private static bool isGameOver;
	private static bool gamePaused;
	private static bool isLevelDesignerGenerated;
	private static int currentLevelBeingEdited;
	private static MenuType nextMenu = 0;
	private static string webXMLText;
	private static bool wwwStatus;


	void Awake()
	{
		if (!created) {
			// this is the first instance - make it persist
			DontDestroyOnLoad (transform.gameObject);
			created = true;
		} else {
			//this must be a duplicate from a scene reload - DESTROY!
			Destroy(this.gameObject);
		}

	}

	void Update()
	{
		if (gamePaused) 
		{
			Time.timeScale = 0.0f;
		} 
		else 
		{
			Time.timeScale = 1.0f;
			#if !UNITY_EDITOR
			currentDeviceOrientation = getDeviceOrientation(Input.acceleration);
			#else
			currentDeviceOrientation = getEditorOrientation();
			#endif
		}

	}

	//re-initialize data here for Game: called from Menu
	public static void StartNewLevel()
	{
		isGameOver = false;
		ResumeGame();
		currentDeviceOrientation = 4;
		Application.LoadLevel("Gameplay");
	}

	 int getEditorOrientation()
	{
		int newRotation = currentDeviceOrientation;
		if(Input.GetKeyUp(KeyCode.Q))
		{
			if(currentDeviceOrientation == 1)
			{
				newRotation = 4;
			}
			else
			{
				newRotation = currentDeviceOrientation - 1;
			}
		}
		else if(Input.GetKeyUp(KeyCode.E))
		{
			if(currentDeviceOrientation == 4)
			{
				newRotation = 1;
			}
			else
			{
				newRotation = currentDeviceOrientation + 1;
			}

		}
		return newRotation;
	}


	public int getDeviceOrientation(Vector3 acceleration)
	{
		int orientation = 0;
		
		// TOP
		if(acceleration.z < 0.5 && acceleration.x > 0.5 && acceleration.y < 0.4)
			orientation = 1;
		// RIGHT
		else if(acceleration.z < 0.5 && acceleration.y > 0.5 && acceleration.x < 0.4)
			orientation = 2;
		// BOTTOM
		else if(acceleration.z < 0.5 && acceleration.x < -0.5 && acceleration.y < 0.4)
			orientation = 3;
		// LEFT
		else if(acceleration.z < 0.5 && acceleration.y < -0.5 && acceleration.x < 0.4)
			orientation = 4;
		
		return orientation;
	}

	public static bool IsGamePaused()
	{
		return gamePaused;
	}

	public static void PauseGame()
	{
		gamePaused = true;
	}

	public static void ResumeGame()
	{
		gamePaused = false;
	}

	//-----------------REFRACTOR GAMEOVER--------------------//

	public static void GameOver()
	{
		//isGameOver = true;
	}
	
	public static void GameOverCompleted()
	{
		isGameOver = true;
		GameObject CompletedMenu = GameObject.Find ("GameOverCompletedMenu");

		foreach(Transform child in CompletedMenu.transform)
		{
			child.gameObject.SetActive(true);
		}

		GameObject.Find ("txtGameOver").guiText.text = "wow! You made it!!!";
	}

	//Overloaded GameOverCompleted.
	public static void GameOverCompleted(string gameOverText)
	{
		isGameOver = true;
		GameObject CompletedMenu = GameObject.Find ("GameOverCompletedMenu");
		
		foreach(Transform child in CompletedMenu.transform)
		{
			child.gameObject.SetActive(true);
		}
		
		GameObject.Find ("txtGameOver").guiText.text = gameOverText;
	}

	//Overloaded GameOverCompleted.
	public static void GameOverCompleted(bool won, string gameOverText)
	{
		isGameOver = true;
		GameObject CompletedMenu = GameObject.Find ("GameOverCompletedMenu");
		
		foreach(Transform child in CompletedMenu.transform)
		{
			child.gameObject.SetActive(true);
		}
		
		GameObject.Find ("txtGameOver").guiText.text = gameOverText;
	}

	//-----------------REFRACTOR GAMEOVER--------------------//

	public static string GetLevelToLoad()
	{
		#if !UNITY_EDITOR
		if (isLevelDesignerGenerated) {
			return "Levels/level_ch" + chapterNumber + "_lvl" + levelNumber;
		} else {
			return "Levels/level_ch" + chapterNumber + "_lvl" + levelNumber + ".xml";
		}
		#elif UNITY_EDITOR
		return "Resources/Levels/xml/level_ch" + chapterNumber + "_lvl" + levelNumber+".xml";
		#endif
	}

	public static string GetLevelPathToLoadInEditor()
	{
		return "Levels/xml/level_ch0" + "_lvl" + currentLevelBeingEdited + ".xml";
	}

	public static string GenerateUserLevelPath(int lvlNumber)
	{
		#if !UNITY_EDITOR
		return "Levels/level_ch0" + "_lvl" + lvlNumber + ".xml";
		#endif
		#if UNITY_EDITOR
		return "Resources/Levels/xml/level_ch0" + "_lvl" + lvlNumber + ".xml";
		#endif
	}

	public static void SetChapterNumber(int chNum)
	{
		chapterNumber = chNum;
	}

	public static void SetLevelNumber(int lvlNum)
	{
		levelNumber = lvlNum;
	}

	public static void SetLevelToLoadStatus(bool isLevelGeneratedByDesigner)
	{
		isLevelDesignerGenerated = isLevelGeneratedByDesigner;
	}

	public static bool IsLevelDesignerCreated()
	{
		return isLevelDesignerGenerated;
	}

	public static void SetCurrentLevelBeingEdited(int levelName)
	{
		currentLevelBeingEdited = levelName;
	}

	public static void SetNextMenu(MenuType nM)
	{
		nextMenu = nM;
	}

	public static MenuType GetNextMenu()
	{
		return nextMenu;
	}

	public static int GetCurrentLevelBeingEdited()
	{
		return currentLevelBeingEdited;
	}

	public static int GetChapterNumber()
	{
		return chapterNumber;
	}

	public static int GetLevelNumber()
	{
		return levelNumber;
	}

	public static bool IsGameOver()
	{
		return isGameOver;
	}

	/*
	 * Gets the Level Data as String from given url
	 * url : the full path of the xml file on the server
	 */
	private IEnumerator GetLevelDataFromURL(string url)
	{
		if (url.Length < 1) 
		{
			Debug.LogError("Invalid url");
			wwwStatus = false;
			return false;
		} 

		WWW www = new WWW(url);

		//Wait for downlaod and load
		yield return www;

		if (www.error == null) {
			//Sucessfully loaded the XML
			Debug.Log (www.text);
			wwwStatus = true;
			webXMLText = www.text;
		} else {
			wwwStatus = false;
		}
	}
	
	/*
	 * Saves the Level data to xml file in the local file system
	 * url : full path of the level file on server
	 */
	public bool SaveLevelFromWeb(string url)
	{
		webXMLText = "";
		StartCoroutine (GetLevelDataFromURL(url));

		//The level fro the web is no in webXMLText, write it to file

		return wwwStatus;
	}


}
