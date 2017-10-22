using UnityEngine;
using System.Collections;
using Entitas;

public class KillChessPieceSystem : ReactiveSystem<InputEntity> {

	readonly GameContext m_gameContext;
	public KillChessPieceSystem(Contexts contexts) : base(contexts.input)
	{
		m_gameContext = contexts.game;
	}

	#region implemented abstract members of ReactiveSystem

	protected override ICollector<InputEntity> GetTrigger (IContext<InputEntity> context)
	{
		return context.CreateCollector(InputMatcher.SelectChessHolder);
	}

	protected override bool Filter (InputEntity entity)
	{
		return entity.hasSelectChessHolder && m_gameContext.gameMode.gameMode == GameMode.KillChess;
	}

	protected override void Execute (System.Collections.Generic.List<InputEntity> entities)
	{
		foreach (var e in entities)
		{
			
		}
	}

	#endregion




}
