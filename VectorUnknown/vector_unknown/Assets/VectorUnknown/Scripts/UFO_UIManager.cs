using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UFO_UIManager : MonoBehaviour {

	private GameObject Player;
	private int GridSpacing = 1;
	private float Height = 2.5f;

	public LineRenderer[] Arrows = new LineRenderer[2];
	public Text[] ChoiceText = new Text[4];
	public Text[] SelectedVectorText = new Text[2];
	public Text[] SelectedConstantText = new Text[2];
	public Text DestinationText;
	public Text SolutionText;

	private Vector3[] Route = new Vector3[2];
	private Vector3[] ArrowPoints = new Vector3[2];
	private Vector2[] Choices = new Vector2[4];
	private int[] ButtonMap = new int[4];
	private Vector2[] SelectedVectors = new Vector2[2];
	private int[] SelectedConstants = new int[2];
	private Vector2 Destination;
	private Vector2 Solution;
	private int Index;

	// Use this for initialization
	void Start () {

		Player = GetComponent<UFO_PuzzleManager> ().Player;
		Height = GetComponent<UFO_PuzzleManager> ().Height;
		GridSpacing = GetComponent<UFO_PuzzleManager> ().GridSpacing;
		for (int i = 0; i < SelectedVectors.Length; i++) {
			SelectedVectors [i] = Vector2.zero;
			SelectedVectorText[i].text = SelectedVectors [i].x.ToString ("F0") + "\n"
				                  + SelectedVectors [i].y.ToString ("F0");
			SelectedConstants [i] = 1;
			SelectedConstantText [i].text = SelectedConstants [i].ToString ("F0");
		}
		Destination = Vector2.zero;
		DestinationText.text = Destination.x.ToString ("F0") + "\n"
							 + Destination.y.ToString ("F0");
		Vector3 temp = Height * Vector3.up;
		ArrowPoints = new Vector3[] { temp, temp };
		Arrows [0].SetPositions (ArrowPoints);
		Arrows [1].SetPositions (ArrowPoints);
		Index = 0;

	}

	public void GoButton () {

		for (int i = 0; i < 2; i++) {
			Route [i] = new Vector3 (SelectedVectors [i].x, 0, SelectedVectors [i].y);
			Route [i] *= SelectedConstants [i] * GridSpacing;
		}
		Player.GetComponent<PlayerMovement> ().Move (Route);

	}

	public void VectorButton (int n) {

		SelectedVectors [Index] = Choices [ButtonMap [n]];
		SelectedVectorText [Index].text = SelectedVectors [Index].x.ToString ("F0") + "\n"
			                                  + SelectedVectors [Index].y.ToString ("F0");
		SelectedConstants [Index] = 1;
		SelectedConstantText [Index].text = SelectedConstants [Index].ToString ("F0");		
		Destination = SelectedVectors [0] + SelectedVectors [1];
		DestinationText.text = Destination.x.ToString ("F0") + "\n"
			                 + Destination.y.ToString ("F0");
		SetArrows ();
		Index = 1 - Index;

	}

	public void ConstantButton (int n) {

		if (n == 0) {
			SelectedConstants [0]--;
			if (Mathf.Abs(SelectedConstants [0] * SelectedVectors [0].x) > 10 || 
				Mathf.Abs(SelectedConstants [0] * SelectedVectors [0].y) > 10) 
				SelectedConstants [0]++;
		}	
		if (n == 1) {
			SelectedConstants [0]++;
			if (Mathf.Abs(SelectedConstants [0] * SelectedVectors [0].x) > 10 || 
				Mathf.Abs(SelectedConstants [0] * SelectedVectors [0].y) > 10) 
				SelectedConstants [0]--;
		}
		if (n == 2) {
			SelectedConstants [1]--;
			if (Mathf.Abs(SelectedConstants [1] * SelectedVectors [1].x) > 10 || 
				Mathf.Abs(SelectedConstants [1] * SelectedVectors [1].y) > 10) 
				SelectedConstants [1]++;
		}
		if (n == 3) {
			SelectedConstants [1]++;
			if (Mathf.Abs(SelectedConstants [1] * SelectedVectors [1].x) > 10 || 
				Mathf.Abs(SelectedConstants [1] * SelectedVectors [1].y) > 10) 
				SelectedConstants [1]--;
		}
		SelectedConstantText [0].text = SelectedConstants [0].ToString ("F0");
		SelectedConstantText [1].text = SelectedConstants [1].ToString ("F0");
		Destination = SelectedConstants [0] * SelectedVectors [0] + SelectedConstants [1] * SelectedVectors [1];
		DestinationText.text = Destination.x.ToString ("F0") + "\n"
			                 + Destination.y.ToString ("F0");
		SetArrows ();

	}

	public void UpdateGame () {

		Choices = GetComponent<UFO_PuzzleManager> ().Choices;
		ButtonMap = GetComponent<UFO_PuzzleManager> ().ButtonMap;
		ChoiceText[0].text = Choices [ButtonMap [0]].x.ToString ("F0") + "\n" + Choices [ButtonMap[0]].y.ToString("F0");
		ChoiceText[1].text = Choices [ButtonMap [1]].x.ToString ("F0") + "\n" + Choices [ButtonMap[1]].y.ToString("F0");
		ChoiceText[2].text = Choices [ButtonMap [2]].x.ToString ("F0") + "\n" + Choices [ButtonMap[2]].y.ToString("F0");
		ChoiceText[3].text = Choices [ButtonMap [3]].x.ToString ("F0") + "\n" + Choices [ButtonMap[3]].y.ToString("F0");
		Solution = GetComponent<UFO_PuzzleManager> ().Solution;
		SolutionText.text = "Goal:\n" + Solution.x.ToString ("F0") +"\n" + Solution.y.ToString ("F0");

	}

	public void ResetUI () {
		
		Player = GetComponent<UFO_PuzzleManager> ().Player;
		for (int i = 0; i < SelectedVectors.Length; i++) {
			SelectedVectors [i] = Vector2.zero;
			SelectedVectorText[i].text = SelectedVectors [i].x.ToString ("F0") + "\n"
				                       + SelectedVectors [i].y.ToString ("F0");
			SelectedConstants [i] = 1;
			SelectedConstantText [i].text = SelectedConstants [i].ToString ("F0");
		}
		Destination = Vector2.zero;
		DestinationText.text = Destination.x.ToString ("F0") + "\n"
			                 + Destination.y.ToString ("F0");
		Vector3 temp = Height * Vector3.up;
		ArrowPoints = new Vector3[] { temp, temp };
		Arrows [0].SetPositions (ArrowPoints);
		Arrows [1].SetPositions (ArrowPoints);
		Index = 0;

	}

	private void SetArrows () {
		
		ArrowPoints [0] = new Vector3 (SelectedVectors [0].x, 0, SelectedVectors [0].y) * SelectedConstants [0] + Height * Vector3.up;
		ArrowPoints [1] = new Vector3 (SelectedVectors [1].x, 0, SelectedVectors [1].y) * SelectedConstants [1] + ArrowPoints [0];
		Vector3[] temp = new Vector3[]{ Height * Vector3.up, ArrowPoints [0] };
		Arrows [0].SetPositions (temp);
		temp [0] = ArrowPoints [0]; temp [1] = ArrowPoints [1];
		Arrows [1].SetPositions (temp);

	}

}
