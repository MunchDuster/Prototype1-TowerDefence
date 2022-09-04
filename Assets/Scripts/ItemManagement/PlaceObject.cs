using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceObject : MonoBehaviour
{
    public static Dictionary<Vector2Int, PlaceObject> worldObjects;

	public static Vector2Int GetWorldIndex(Vector3 position)
	{
		return new Vector2Int(
			Mathf.FloorToInt(position.x),
			Mathf.FloorToInt(position.y)
		);
	}

	public static PlaceObject ObjectAtPlace(Vector3 position)
	{
		Vector2Int index = GetWorldIndex(position);
		if(worldObjects.ContainsKey(index))
		{
			return worldObjects[index];
		}
		else
		{
			return null;
		}
	}

	protected abstract Vector2Int[] GetWorldIndices();

	protected void OnAwake()
	{
		Vector2Int[] indices = GetWorldIndices();

		for(int i = 0; i < indices.Length; i++)
		{
			worldObjects.Add(indices[i], this);
		}
	}
}
