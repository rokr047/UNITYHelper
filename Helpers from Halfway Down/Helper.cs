using UnityEngine;
using System.Collections;

public class Helper
{
	private static Helper instance;
	private float originalWidth;
	private float originalHeight;

	protected Helper()
	{
		originalWidth = 640;
		originalHeight = 480;
	}

	public static Helper getInstance()
	{
		if (instance == null) 
		{
			instance = new Helper();
		}

		return instance;
	}

	public Vector3 getScaleVector()
	{
		float rx = Screen.width / originalWidth;
		float ry = Screen.height / originalHeight;
		return new Vector3 (rx,ry,1);;
	}

	public Matrix4x4 getScaledGUIMatrix()
	{
		float rx = Screen.width / originalWidth;
		float ry = Screen.height / originalHeight;
		return Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (rx,ry,1));
	}

	public float getOriginalHeight()
	{
		return originalHeight;
	}

	public float getOriginalWidth()
	{
		return originalWidth;
	}
}
