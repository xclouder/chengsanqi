using UnityEngine;
using System.Collections;
using Entitas;

public class GameLogicSystem : ReactiveSystem<InputEntity> {

	private GameContext _gameContext;

	public GameLogicSystem(Contexts contexts) : base(contexts.input)
	{
		_gameContext = contexts.game;

		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityAdded += OnAddChessPiece;
		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityRemoved += OnRemoveChessPiece;
		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityUpdated += OnUpdateChessPiece;
	}

	private void OnAddChessPiece(IGroup<GameEntity> grp, GameEntity entity, int index, IComponent comp)
	{
		//add chess piece

	}

	private void OnRemoveChessPiece(IGroup<GameEntity> grp, GameEntity entity, int index, IComponent comp)
	{

	}

	private void OnUpdateChessPiece(IGroup<GameEntity> grp, GameEntity entity, int index, IComponent comp)
	{

	}

	protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
	{
		return context.CreateCollector(InputMatcher.AnyOf(InputMatcher.SelectChessHolder));
	}

	protected override bool Filter (InputEntity entity)
	{
		return entity.hasSelectChessHolder;
	}

	protected override void Execute (System.Collections.Generic.List<InputEntity> entities)
	{
		
	}


}
