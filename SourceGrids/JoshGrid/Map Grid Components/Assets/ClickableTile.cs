using UnityEngine;
using System.Collections;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap map;

	void OnMouseUp() {
		Debug.Log ("Well at least something works, right :)?");

		map.MoveSelectedUnitTo(tileX, tileY);
	}

}
