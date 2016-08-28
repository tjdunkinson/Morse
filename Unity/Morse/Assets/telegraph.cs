using UnityEngine;
using System.Collections;

public class telegraph : MonoBehaviour {

	//TODO: Still require a way to recognise if somone messes up a morse sequence and reset.

	public enum  Morse
	{
		nul = -1,
		dit = 0,
		dah = 1
	};

	float teleInputTimer;
	public bool timeInput;

	public float ditMaxThreshold = 0.2f;
	public float dahMaxThreshold = 0.5f;

	public Morse[] sequence;

	public int currentSeqInput = 0;

	public int[] coordinates;

	public int currentCoordInput = 0;

	public Vector2 coord;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < sequence.Length; i++)
		{
			sequence [i] = Morse.nul;
		}
		//print ((int)sequence [0]);  //reference on how to access the value

		teleInputTimer = 0f; 
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetButtonDown ("Telegraph")) 
		{
			teleInputTimer = 0f;
			timeInput = true;
		}

		if (timeInput) 
		{
			teleInputTimer += Time.deltaTime;

			if (currentSeqInput < sequence.Length)
				sequence [currentSeqInput] = TelegraphInput ();
			
			else 
			{
				coordinates[currentCoordInput] = DetermineCoord (sequence);
				currentCoordInput++;

				currentSeqInput = 0;
				for (int i = 0; i < sequence.Length; i++)
				{
					sequence [i] = Morse.nul;
				}
			}
		}

		if (Input.GetButtonUp ("Telegraph") && timeInput == true) 
		{
			timeInput = false;

			if (sequence [currentSeqInput] != Morse.nul)
				currentSeqInput++;
		}

		if (currentCoordInput > coordinates.Length)
		{
			currentCoordInput = coordinates.Length;
		}

		if (Input.GetButtonDown ("Fire") && currentCoordInput == coordinates.Length) 
		{
			coord = Vector2.zero;
			coord.x = (coordinates [0]*100) + (coordinates [1]*10f) + coordinates [2];
			coord.y = (coordinates [3]*100) + (coordinates [4]*10f) + coordinates [5];
			coord /= 10.0f;
			print (coord);
		}

	}

	Morse TelegraphInput ()
	{

		Morse setMorse = Morse.nul;

		if (timeInput == true) 
		{
			if (teleInputTimer < ditMaxThreshold)
				
				setMorse = Morse.dit;

			else 
				if (teleInputTimer < dahMaxThreshold)

				setMorse = Morse.dah;
		}

		return setMorse;
	}

	int DetermineCoord (Morse[] seq)
	{
		
		int bufferValue = 0;
		int coord = 0;

		for (int i = 1; i < (seq.Length +1); i++)
		{
			//sequence [i] = Morse.nul;

			bufferValue += ((int)seq [i-1] * i);
		}

		//this is where is gets shittily hacky
		if (bufferValue == 14)
			coord = 1;
		else if (bufferValue == 12)
			coord = 2;
		else if (bufferValue == 9)
			coord = 3;
		else if (bufferValue == 5)
			coord = 4;
		else if (bufferValue == 0)
			coord = 5;
		else if (bufferValue == 1)
			coord = 6;
		else if (bufferValue == 3)
			coord = 7;
		else if (bufferValue == 6)
			coord = 8;
		else if (bufferValue == 10)
			coord = 9;
		else if (bufferValue == 15)
			coord = 0;
		else 
		{
			int temp = (int)seq [currentCoordInput];
			currentCoordInput--;
			return temp;
		}

		return coord;
	}
}
