using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;

public class AddChessHolderSystem : ReactiveSystem<GameEntity> {

	readonly Transform _viewContainer = new GameObject("ChessHolder Views").transform;
	readonly GameContext _context;

	public AddChessHolderSystem(Contexts contexts) : base(contexts.game)
	{
		_context = contexts.game;
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.ChessPieceHolder);
	}

	protected override bool Filter(GameEntity entity)
	{
		return !entity.isChessPieceHolder;
	}

	protected override void Execute(List<GameEntity> entities)
	{
		foreach (GameEntity e in entities)
		{
			GameObject go = new GameObject("Chess Holder View");
			go.transform.SetParent(_viewContainer, false);
			e.AddView(go);
			go.Link(e, _context);
		}
	}

}
