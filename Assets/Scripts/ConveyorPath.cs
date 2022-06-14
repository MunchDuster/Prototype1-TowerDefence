using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPath
{
	private class Section
	{
		public Vector3 start;
		public Vector3 end;
		
		public float length {get {return _length; }}
		private float _length;
		
		public float totalLength;
		
		public Section(Vector3 start, Vector3 end)
		{
			this.start = start;
			this.end = end;
			_length = Vector3.Distance(start, end);
		}
		public void UpdateLength()
		{
			_length = Vector3.Distance(start, end);
		}	
		public Vector3 GetPoint(float dist)
		{
			float lerp = 1 - (totalLength - dist) / length;
			Debug.Log(lerp);
			return Vector3.Lerp(start, end, lerp);
		}
	}
	
	private List<Section> sections;
	
	public float length {get {return _length; }}
	private float _length;
	
	public ConveyorPath(List<Transform> points)
	{
		sections = new List<Section>();
		for(int i = 0; i < points.Count - 1; i++)
		{
			sections.Add(new Section(points[i].position, points[i + 1].position));
		}
		
		UpdateLength();
	}
	
	public void InsertPoint(int index, Transform point)
	{
		sections[index].end = point.position;
		sections[index + 1].start = point.position;
		
		sections[index].UpdateLength();
		sections[index + 1].UpdateLength();
		
		Section newSection = new Section(sections[index].end, point.position);
		sections.Insert(index, newSection);
		
		UpdateLength();
	}
	
	private void UpdateLength()
	{
		float total = 0;
		
		for(int i = 0; i < sections.Count; i++)
		{
			total += sections[i].length;
			sections[i].totalLength = total;
		}
		
		_length = total;
	}
	
	public Vector3 GetPointAtDistance(float distance, int startIndex = 0)
	{
		//Assuming distance <= length
		
		if(startIndex > 0) startIndex--;
		
		
		for(int i = startIndex; i < sections.Count; i++)
		{
			if(sections[i].totalLength >= distance)
			{
				//Lerp in section
				return sections[i].GetPoint(distance);
			}
		}
			
		throw new System.Exception("Conveyor Path GetPoint, distance past end of path");
		return Vector3.zero;
	}
}