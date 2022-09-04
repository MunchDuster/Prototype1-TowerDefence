using UnityEngine;

public class EnergySource : MonoBehaviour
{
	public float resistance;
	public bool callUpdate = false;

	public float input;
	public float output;

	public delegate void OnChanged(float output);
	public OnChanged onChanged;


	// Start is called before the first frame update
	private void Start()
	{
		if (callUpdate) RecalculateEnergy(input);
	}

	public void RecalculateEnergy(float input)
	{
		this.input = input;

		output = input - resistance;

		if (onChanged != null) onChanged(output);
	}
}