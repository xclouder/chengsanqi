using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.Unity;

public class RenderChessPieceSystem : ReactiveSystem<GameEntity> {

	public RenderChessPieceSystem(Contexts contexts) : base(contexts.game)
	{

	}

	#region implemented abstract members of ReactiveSystem

	protected override ICollector<GameEntity> GetTrigger (IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.ChessPiece);
	}

	protected override bool Filter (GameEntity entity)
	{
		return entity.hasChessPiece && entity.hasView;
	}

	protected override void Execute (System.Collections.Generic.List<GameEntity> entities)
	{
		foreach (var e in entities)
		{
			var go = e.view.gameObject;


		}
	}

	private void RenderChessPiece(ChessPieceComponent c, GameObject o)
	{

	}

	#endregion




}
