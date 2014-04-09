using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

[XmlRoot("LevelDescription")]
public class Levels
{
	[XmlArray("Level"),XmlArrayItem("LevelObject")]
	public LevelObject[] lObjects;

	public Levels()
	{
		//Default Constructor for serialization.
	}

	public Levels(int size)
	{
		lObjects = new LevelObject[size];
	}

	public void SaveLevel(string path)
	{
		#if !UNITY_EDITOR
		//Debug.Log(Application.PersistentDataPath);
		if (FileManager.doesFolderExits ("Levels", true)) 
		{
			var serializer = new XmlSerializer (typeof(Levels));
			string persistantPath = FileManager.getPersistantLevelPath (path);
			using (var stream = new FileStream(persistantPath,FileMode.Create)) 
			{
					serializer.Serialize (stream, this);
			}
		}
		#elif UNITY_EDITOR
		var serializer = new XmlSerializer (typeof(Levels));
		using (var stream = new FileStream(Application.dataPath + Path.DirectorySeparatorChar + path,FileMode.Create)) 
		{
			serializer.Serialize (stream, this);
		}
		#endif
	}

	/*
	 * Returns the text of the Level xml as string.
	 * Use this only for User created levels.
	 */
	public static string LoadLevelAsText(string relativePath)
	{
		string fullPath;

		if (!FileManager.doesFileExist (relativePath)) 
		{
			Debug.LogError("The file you are trying to open does not exists!!");
			return null;
		}

		#if !UNITY_EDITOR
		fullPath = FileManager.getPersistantLevelPath (relativePath);
		#elif UNITY_EDITOR
		fullPath = Application.dataPath + Path.DirectorySeparatorChar + relativePath;
		#endif

		XmlDocument xmlLevel = new XmlDocument ();
		xmlLevel.Load (fullPath);
		return xmlLevel.OuterXml;
	}

	/*
	 * Use this to load User Created Levels
	 */ 
	public static Levels LoadLevel(string path)
	{
		#if !UNITY_EDITOR
		if (!FileManager.doesFileExist (path)) 
		{
			Debug.LogError("The file you are trying to open does not exists!!");
			return null;
		}
		var serializer = new XmlSerializer (typeof(Levels));
		string persistantPath = FileManager.getPersistantLevelPath (path);
		using (var stream = new FileStream(persistantPath, FileMode.Open)) 
		{
			return serializer.Deserialize(stream) as Levels;
		}
		#elif UNITY_EDITOR
		var serializer = new XmlSerializer (typeof(Levels));
		using (var stream = new FileStream(Application.dataPath + Path.DirectorySeparatorChar + path, FileMode.Open)) 
		{
			return serializer.Deserialize(stream) as Levels;
		}
		#endif
	}

	/*
	 * Use this to load Designer Created Levels
	 */
	public static Levels LoadLevel(string fileName, bool isDesignerLevel)
	{
		Levels lvl = null;

		if (isDesignerLevel) {
			#if !UNITY_EDITOR
			TextAsset txtAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
			if(txtAsset == null)
			{
				Debug.LogError("The level you are accessing cannot be read!!");
				return null;
			}
			else
			{
				return DeserializeStringToObject(txtAsset);
			}
			#elif UNITY_EDITOR
			var serializer = new XmlSerializer (typeof(Levels));
			using (var stream = new FileStream(fileName, FileMode.Open)) {
						return serializer.Deserialize (stream) as Levels;
				}
			#endif
		} else {
			Debug.LogError("You are trying to use incorrect function. Use LoadLevel(string path) instead.");
			return lvl;
		}

		return lvl;
	}

	public static Levels DeserializeStringToObject(TextAsset xmlTextAsset)
	{
		MemoryStream memStream = new MemoryStream (xmlTextAsset.bytes);
		XmlTextReader xmlReader = new XmlTextReader (memStream);
		var serializer = new XmlSerializer (typeof(Levels));
		Levels lvl = serializer.Deserialize (xmlReader) as Levels;

		xmlReader.Close ();
		memStream.Close ();

		return lvl;
	}

	//Loads the xml directly from the given string. Useful in combination with www.text.
	public static Levels LoadFromText(string text) 
	{
		var serializer = new XmlSerializer(typeof(Levels));
		return serializer.Deserialize(new StringReader(text)) as Levels;
	}
}