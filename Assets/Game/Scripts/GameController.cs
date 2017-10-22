using UnityEngine;
using System.Collections;
using Entitas;

public class GameController : MonoBehaviour {

	Systems _systems;

	void Awake()
	{
		var contexts = Contexts.sharedInstance;

		_systems = new Feature("Systems")
			//.Add(new DebugMessageSystem(contexts))
//			.Add(new HelloWorldSystem(contexts));
			.Add(new AddChessHolderSystem(contexts))
			.Add(new EmitInputSystem(contexts))
			.Add(new EmitSelectHolderSystem(contexts))
			.Add(new RenderChessPieceSystem(contexts))
			.Add(new GameLogicSystem(contexts))
			.Add(new ChessComboSystem(contexts))
			;

		_systems.Initialize();
	}

//	void Start()
//	{
//		var e = Contexts.sharedInstance.game.CreateEntity();
//		e.isResetGame = true;
//	}

	void Update()
	{
		// call Execute() on all the IExecuteSystems and 
		// ReactiveSystems that were triggered last frame
		_systems.Execute();
		// call cleanup() on all the ICleanupSystems
		_systems.Cleanup();
	}

}
