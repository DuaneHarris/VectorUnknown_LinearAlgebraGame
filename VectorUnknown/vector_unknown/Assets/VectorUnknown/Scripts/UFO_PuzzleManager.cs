using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UFO_PuzzleManager : MonoBehaviour {

	public int GridSpacing = 1;					    //Grid Spacing on the Game Board
	public float Height = 2.5f;						//Y Value of Player and Goal
	public GameObject Player;
	public GameObject[] Goal = new GameObject[2];

	public Vector2[] Choices = new Vector2[4];			//Container for Vectors in the form used for UI Text
	public int[] ButtonMap;
	public Vector2 Solution;

	public System.Random rnd = new System.Random ();

	private Vector3[] GoalPosition = new Vector3[2];
	private Vector3[] Route = new Vector3[2];       //The Path that the Player Must Take to Reach the Goal
	private Vector2[] BaseVectors = new Vector2[9]; //The Basic Vectors that the game will use to create puzzles
	private int[] Num = new int[2];					//Container for the random index numbers of the BaseVectors array
	private int[] Quad = new int[2];				//Container for the random quadrant number (changes direction of vectors)
	private int[] Mul = new int[2];					//Container for the random constant that extends the length of the vectors
	private int GameMode;

	// Use this for initialization
	void Start () {

		// Initializes BaseVectors array.
		// All vectors should point into the 1st quadrant
		// i.e. all vector elements should be positive.
		BaseVectors [0] = new Vector3 (0, 1);
		BaseVectors [1] = new Vector3 (1, 0);
		BaseVectors [2] = new Vector3 (1, 1);
		BaseVectors [3] = new Vector3 (1, 2);
		BaseVectors [4] = new Vector3 (2, 1);
		BaseVectors [5] = new Vector3 (1, 3);
		BaseVectors [6] = new Vector3 (3, 1);
		BaseVectors [7] = new Vector3 (2, 3);
		BaseVectors [8] = new Vector3 (3, 2);

		GameMode = 0;
		NextPuzzle (); //Create 1st Puzzle
		ResetGame (); //Set Up Game Board

	}

	// Update is called once per frame
	void Update () {

	}

	public void NextPuzzle () {

		// Randomly chooses two vectors from the BaseVectors array and stores them in the Choices array.
		int l = BaseVectors.Length;            // Num[i]= Random index number of BaseVectors array
		Num [0] = rnd.Next (0, l);             // Num[i] is used to select random vector
		if (Num [0] <= 1) {					   // If Num[0] --> <0,1> or <1,0>, then Num[1] should not be a duplicate of Num[0]
			Num [1] = rnd.Next (0, l - 1);     // So Num[1] should be selected from among l-1 index numbers
			if (Num [1] >= Num [0]) Num [1]++; // Adjust Num[1] depending on the value of Num[0], so that Num[0]!=Num[1] and 0<=Num[1]<=l
		} 
		else Num [1] = rnd.Next (0, l);		 // If Num[0] !-> <0,1> or <1,0>, then Num[1] can be equal to Num[2]
		Choices [0] = BaseVectors [Num [0]]; // Choices[0]= First Random Vector (Will 	Eventually Become First Solution Vector)
		Choices [1] = BaseVectors [Num [1]]; // Choiecs[1]= Second Random Vector (Will Eventually Become Second Solution Vector)

		// Randomly changes the directions of the vectors in the Choices array, so that they point to different quadrants.
		Quad[0] = rnd.Next (1, 5); // Quad[0]= Random quadrant for the first solution vector, can be any quadrant 1-4
		Quad[1] = rnd.Next (1, 3); // Quad[1]= Random quadrant for the second solution vector, must be adjacent to Quad[0]
		if (Quad [0] == 1 || Quad [0] == 3) Quad [1] *= 2; // Quad[0] = 1 or 3  -->  Quad[1] = 2 or 4
		else Quad [1] = 2 * Quad [1] - 1;				   // Quad[0] = 2 or 4  -->  Quad[1] = 1 or 3
		for (int i = 0; i < 2; i++) {               // All vectors initially point to quadrant 1
			if (Quad [i] == 2) Choices [i].x *= -1; // Change the sign of the x component, vector now points to quadrant 2
			if (Quad [i] == 3) Choices [i] *= -1;   // Change the sign of the both components, vector now points to quadrant 3
			if (Quad [i] == 4) Choices [i].y *= -1; // Change the sign of the y component, vector now points to quadrant 4
		}

		// Adds two new vectors to the Choices array that are proportional to the first two vectors
		// i.e. Choices[0] and Choices[2] are linearly dependent 
		//      Choices[1] and Choices[3] are linearly dependent 
		for (int i = 0; i < 2; i++) {			
			if (Num [i] <= 2) {              // If the vector is <1,1>, <1,0>, or <0,1>, then the Mul[i] can be from 2-10, 
				Mul [i] = rnd.Next (2, 11);  // Mul[i] = constant that will determine goal placement -- Max: <10,10>, <10,0>, <0,10>
				if (Mul [i] == 6) {          // To keep the vectors "nice", only certain constants can multiply Choices
					int n = rnd.Next (2, 5); // If Mul[i]=6, Choices[i+2] can be multiplied by 2,3,6 (Since 6 is divisible by 2,3,6)
					if (n == 4) n = 6;       // 4 Maps to 6
					Choices [i + 2] = Choices [i] * n; // Add vector that is linearly dependent with Choices[i]
				} 
				else if (Mul [i] == 10) {    // If Mul[i]=10, Choices[i+2] can be multiplied by 2,5,10
					int n = rnd.Next (2, 5); // Since 10 is divisible by 2,5,10
					if (n == 3) n = 5;       // 3 maps to 5
					if (n == 4) n = 10;      // 4 maps to 10
					Choices [i + 2] = Choices [i] * n; // Add vector that is linearly dependent with Choices[i]
				} 
				else if (Mul [i] == 4) Choices [i + 2] = Choices [i] * Mathf.Pow (2, rnd.Next (1, 3)); //4 is divisible by 2 and 4
				else if (Mul [i] == 8) Choices [i + 2] = Choices [i] * Mathf.Pow (2, rnd.Next (1, 4)); //8 is divisible by 2, 4, and 8
				else if (Mul [i] == 9) Choices [i + 2] = Choices [i] * Mathf.Pow (3, rnd.Next (1, 3)); //9 is divisible by 3 and 9
				else Choices [i + 2] = Choices [i] * Mul [i]; // All other possible numbers are only divisible by themselves
			} 
			else if (Num [i] <= 4) {       // If the vector is <1,2> or <2,1>
				Mul [i] = rnd.Next (2, 6); // Then Mul[i] can be 2-5, i.e. <5,10> and <10,5> are max
				if (Mul [i] == 4) Choices [i + 2] = Choices [i] * Mathf.Pow (2, rnd.Next (1, 3)); //4 is divisible by 2 and 4
				else Choices [i + 2] = Choices [i] * Mul [i]; // Add vector that is linearly dependent with Choices[i]
			} 
			else {						   // If the vector is <1,3>, <3,1>, <2,3>, or <3,2>
				Mul [i] = rnd.Next (2, 4); // Then Mul[i] can be 2-3, i.e. <3,9>, <9,3>, <6,9>, and <9,6> are max
				Choices[i+2] = Choices [i] * Mul [i]; // Add vector that is linearly dependent with Choices[i]
			}
			int s = rnd.Next (0, 2);           //Randomly change the sign of the new vectors
			if (s == 0) Choices [i + 2] *= -1; //0 --> sign change, 1 --> no sign change
		}

		// Shuffle the vectors in Choices my making a random mapping
		ButtonMap = new int[] { 0, 1, 2, 3 };
		for (int i = 0; i < 20; i++) {
			int n1 = rnd.Next (0, 4);
			int n2 = rnd.Next (0, 3);
			if (n2 >= n1) n2++;
			int temp = ButtonMap [n1];
			ButtonMap [n1] = ButtonMap [n2];
			ButtonMap [n2] = temp;
		}

		// Calculate Solution and Goal Positions
		if (GameMode == 0) {
			Solution = Mul [0] * Choices [0] + Mul [1] * Choices [1];
			GoalPosition [0] = new Vector3 (Solution.x, Height / GridSpacing, Solution.y) * GridSpacing;
		} 
		else {
			Solution = Vector2.zero;
			GoalPosition [0] = new Vector3 (Mul [0] * Choices [0].x, Height / GridSpacing, Mul [0] * Choices [0].y) * GridSpacing;
			GoalPosition [1] = new Vector3 (Mul [1] * Choices [1].x, Height / GridSpacing, Mul [1] * Choices [1].y) * GridSpacing;
		}
		GetComponent<UFO_UIManager>().UpdateGame();

	}

	public void ResetGame () {

		Player.transform.position = new Vector3 (0, Height, 0); //Initialize Player Position
		if (GameMode == 0) {
			Goal [0].transform.position = GoalPosition [0];
			Goal [1].SetActive (false);
		}
		else {
			Goal [0].transform.position = GoalPosition [0];
			Goal [1].transform.position = GoalPosition [1];
			Goal [1].SetActive (true);
		}

	}

	public void ChangeGameMode (){
		
		GameMode = 1 - GameMode;

	}

}
