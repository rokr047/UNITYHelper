using System.Xml;
using System.Xml.Serialization;

public class LevelObject
{
	[XmlAttribute("isEmpty")]
	public bool isEmpty;

	[XmlAttribute("splObjType")]
	public string splObjType = "none";

	public int posX;
	public int posY;
	public string objectType;

	[XmlArray("Events"),XmlArrayItem("Event")]
	//[XmlElement("Event")]
	public Events[] listOfEvents;

	[XmlIgnore]
	public int _counter = 0;

	[XmlIgnore]
	public int _MaxEventsCount = 5;

	public LevelObject()
	{
		//Default Constructor
	}

	public LevelObject(bool _isEmpty, int _posX, int _posY, string _objectType)
	{
		isEmpty = _isEmpty;
		posX = _posX;
		posY = _posY;
		objectType = _objectType;
		listOfEvents = new Events[_MaxEventsCount];
	}

	public LevelObject(bool _isEmpty, string _splObjType, int _posX, int _posY, string _objectType)
	{
		isEmpty = _isEmpty;
		posX = _posX;
		posY = _posY;
		objectType = _objectType;
		splObjType = _splObjType;
		listOfEvents = new Events[_MaxEventsCount];
	}

	public LevelObject(bool _isEmpty, int _posX, int _posY, string _objectType, int _NumOfEvents)
	{
		isEmpty = _isEmpty;
		posX = _posX;
		posY = _posY;
		objectType = _objectType;
		listOfEvents = new Events[_NumOfEvents];
	}

	public LevelObject(bool _isEmpty, string _splObjType, int _posX, int _posY, string _objectType, int _NumOfEvents)
	{
		isEmpty = _isEmpty;
		posX = _posX;
		posY = _posY;
		objectType = _objectType;
		splObjType = _splObjType;
		listOfEvents = new Events[_NumOfEvents];
	}

	public void AddEvent(string _objectName)
	{
		listOfEvents [_counter] = new Events(_objectName);
		_counter++;
	}
}

public class Events
{
	//This will be X and Y combo. e.g 35
	[XmlAttribute("objectName")]
	public string objectName;

	public Events()
	{
		//Default Constructor
	}

	public Events(string _objectName)
	{
		objectName = _objectName;
	}
}
