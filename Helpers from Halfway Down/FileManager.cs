using UnityEngine;
using System.Collections;
using System.IO;

public static class FileManager {
	
	private static string _path = Application.persistentDataPath + "/";

	/*
	 * Check if the folder exists,
	 * optionally create the folder if it does not exits.
	 */
	public static bool doesFolderExits(string relativeFolderPath,bool createFolder)
	{
		bool returnValue = false;

		returnValue = Directory.Exists (_path + relativeFolderPath);

		if (!returnValue && createFolder) 
		{
			Directory.CreateDirectory (_path + relativeFolderPath);
		}

		returnValue = Directory.Exists (_path + relativeFolderPath);

		return returnValue;
	}

	public static string getPersistantLevelPath(string levelName)
	{
		//Debug.Log ("PERSISTANT PATH :: " + _path + levelName);
		return (_path + levelName);
	}

	public static bool doesFileExist(string relativeFilePath)
	{
		#if !UNITY_EDITOR
		return File.Exists (_path +relativeFilePath );
		#else
		return File.Exists( "Assets/" + relativeFilePath);
		#endif
	}

	public static int GetNumberOfUserLevels()
	{
		DirectoryInfo dirInfo = new DirectoryInfo (Application.persistentDataPath);
		FileInfo[] fInfo = dirInfo.GetFiles("*.xml");
		return fInfo.Length;
	}
}
