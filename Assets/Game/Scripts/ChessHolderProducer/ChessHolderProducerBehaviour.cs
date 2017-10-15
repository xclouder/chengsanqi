using UnityEngine;
using System.Collections;

public class ChessHolderProducerBehaviour : MonoBehaviour {

	public int round;
	public int pos;

	// Use this for initialization
	void Start () {
		var e = Contexts.sharedInstance.game.CreateEntity();
		e.AddPosition(round, pos);
		e.isChessPieceHolder = true;
	}
}
