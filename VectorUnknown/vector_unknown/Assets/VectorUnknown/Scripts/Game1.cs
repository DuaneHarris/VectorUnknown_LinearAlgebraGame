using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game1 : MonoBehaviour {

	public int GridSpacing = 10;					//Grid Spacing on the Game Board
	public float Height = 2.5f;						//Y Value of Player and Goal
	public GameObject Player;
	public GameObject Goal;
	public Button[] VectorButtons = new Button[4];
	public Text[] ButtonText = new Text[4];
	public Text[] RouteText = new Text[2];
	public Text SolutionText;
	public System.Random rnd = new System.Random ();

	private Vector3[] Route = new Vector3[2];       //The Path that the Player Must Take to Reach the Goal
	private Vector2[] BaseVectors = new Vector2[9]; //The Basic Vectors that the game will use to create puzzles
	private Vector2[] Choices = new Vector2[4];			//Container for Vectors in the form used for UI Text
	private Vector2 Solution;
	private int[] Num = new int[2];					//Container for the random index numbers of the BaseVectors array
	private int[] Quad = new int[2];				//Container for the random quadrant number (changes direction of vectors)
	private int[] Mul = new int[2];					//Container for the random constant that extends the length of the vectors
	private int[] ButtonMap;
	private int stage;

	// Use this for initialization
	void Start () {

		// Initializes BaseVectors array.
		// All vectors should point into the 1st quadrant
		// i.e. all vector elements should be positive.
		BaseVectors [0] = new Vector3 (1, 1);
		BaseVectors [1] = new Vector3 (0, 1);
		BaseVectors [2] = new Vector3 (1, 0);
		BaseVectors [3] = new Vector3 (1, 2);
		BaseVectors [4] = new Vector3 (2, 1);
		BaseVectors [5] = new Vector3 (1, 3);
		BaseVectors [6] = new Vector3 (3, 1);
		BaseVectors [7] = new Vector3 (2, 3);
		BaseVectors [8] = new Vector3 (3, 2);

		NextPuzzle (); //Create 1st Puzzle

	}
		
	// Update is called once per frame
	void Update () {
		if (stage == 2)
			stage = 0;
	}

	public void NextPuzzle () {

		Player.transform.position = new Vector3 (0, Height, 0); //Initialize Player Position
		ButtonMap = new int[] { 0, 1, 2, 3 };
		stage = 0;

		// Randomly chooses two different vectors from the BaseVectors array
		// and stores them in the Choices array.
		int cL = BaseVectors.Length;
		Num [0] = rnd.Next (0, cL);     
		Num [1] = rnd.Next (0, cL - 1);
		if (Num [1] >= Num [0]) Num [1]++;
		Choices [0] = BaseVectors [Num [0]];
		Choices [1] = BaseVectors [Num [1]];

		// Randomly changes the directions of the vectors in the Choices array, so that they point to different quadrants.
		// The first vector can randomly point into any quadrant.
		// The second vector must point into a quadrant that borders the first vector's quadrant.
		// Ex: Quad[0] = 1 or 3  -->  Quad[1] = 2 or 4
		//     Quad[0] = 2 or 4  -->  Quad[1] = 1 or 3
		Quad[0] = rnd.Next (1, 5);
		Quad[1] = rnd.Next (1, 3);
		if (Quad [0] == 1 || Quad [0] == 3) Quad [1] *= 2;
		else Quad [1] = 2 * Quad [1] - 1;
		for (int i = 0; i < 2; i++) {
			if (Quad [i] == 2) Choices [i].x *= -1;
			if (Quad [i] == 3) Choices [i] *= -1;
			if (Quad [i] == 4) Choices [i].y *= -1;
		}
			
		// Needs Comments
		for (int i = 0; i < 2; i++) {			
			if (Num [i] <= 2) {
				Mul [i] = rnd.Next (2, 10);
				if (Mul [i] == 6) {
					int n = rnd.Next (2, 5);
					if (n == 4) n = 6;
					Choices [i + 2] = Choices [i] * n;
				}
				else if (Mul [i] == 4) Choices [i + 2] = Choices [i] * Mathf.Pow (2, rnd.Next (1, 3));
				else if (Mul [i] == 8) Choices [i + 2] = Choices [i] * Mathf.Pow (2, rnd.Next (1, 4));
				else if (Mul [i] == 9) Choices [i + 2] = Choices [i] * Mathf.Pow (3, rnd.Next (1, 3));
				else Choices [i + 2] = Choices [i] * Mul [i];
			} 
			else if (Num [i] <= 4) {
				Mul [i] = rnd.Next (2, 6);
				if (Mul [i] == 4) Choices [i + 2] = Choices [i] * Mathf.Pow (2, rnd.Next (1, 3));
				else Choices [i + 2] = Choices [i] * Mul [i];	
			} 
			else {
				Mul [i] = rnd.Next (1, 4);
				Choices[i+2] = Choices [i] * Mul [i];
			}
		}

		// Calculate Solution and Change Position of Goal
		Solution = Mul[0]*Choices[0] + Mul[1]*Choices[1];
		Goal.transform.position = new Vector3 (Solution.x, Height / GridSpacing, Solution.y) * GridSpacing;
	
		for (int i = 0; i < 20; i++) {
			int n1 = rnd.Next (0, 4);
			int n2 = rnd.Next (0, 3);
			if (n2 >= n1) n2++;
			int temp = ButtonMap [n1];
			ButtonMap [n1] = ButtonMap [n2];
			ButtonMap [n2] = temp;
		}
			
		// Changes the UI Text for Vectors.
		ButtonText[0].text = Choices [ButtonMap [0]].ToString ("F0");
		ButtonText[1].text = Choices [ButtonMap [1]].ToString ("F0");
		ButtonText[2].text = Choices [ButtonMap [2]].ToString ("F0");
		ButtonText[3].text = Choices [ButtonMap [3]].ToString ("F0");

		SolutionText.text = Solution.x.ToString ("F0") + "\n" + Solution.y.ToString ("F0");

	}

	public void GoButton () {
		Player.GetComponent<PlayerMovement> ().Move (Route);
	}

	public void VectorButton (int n) {
		Vector2 V = Choices [ButtonMap [n]];
		Route [stage] = new Vector3 (V.x, 0, V.y);
		RouteText [stage].text = V.x.ToString ("F0") + "\n" + V.y.ToString("F0");
		stage++;
	}

}
