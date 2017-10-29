using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.Unity;

public class RenderChessPieceSystem : ReactiveSystem<GameEntity> {

	readonly Transform _viewContainer = new GameObject("Chess Views").transform;

	readonly GameContext _context;

	public RenderChessPieceSystem(Contexts contexts) : base(contexts.game)
	{
		_context = contexts.game;

		_context.GetGroup(GameMatcher.Position).OnEntityUpdated += (group, entity, index, previousComponent, newComponent) => {

			if (entity.hasChessPiece)
			{
				UpdateChessPiece(entity);
			}

		};
	}

	#region implemented abstract members of ReactiveSystem

	protected override ICollector<GameEntity> GetTrigger (IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.ChessPiece);
	}

	protected override bool Filter (GameEntity entity)
	{
		return entity.hasChessPiece;
	}

	protected override void Execute (System.Collections.Generic.List<GameEntity> entities)
	{
		foreach (var e in entities)
		{
			RenderChessPiece(e);
		}
	}

	private void RenderChessPiece(GameEntity e)
	{
		GameObject go = new GameObject("Chess View");
		go.transform.position = e.position.position;
		go.transform.SetParent(_viewContainer, false);
		e.AddView(go);
		go.Link(e, _context);

		e.view.Init(e.chessPiece);
	}

	private void UpdateChessPiece(GameEntity e)
	{
		e.view.gameObject.transform.position = e.position.position;
	}

	#endregion




}
