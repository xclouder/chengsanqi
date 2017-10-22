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
		var currTurn = m_gameContext.turnState.turn;
		foreach (var e in entities)
		{
			var holder = e.selectChessHolder.chessHolder;
			if (holder.hasLayChessPiece)
			{
				bool isWhite = holder.layChessPiece.chessPiece.isWhite;
				bool needWhite = currTurn == Turn.White ? false : true;
				if (isWhite == needWhite)
				{
					var coor = holder.layChessPiece.chessPieceEntity.coordinate;
					Debug.Log(string.Format("destroy chess piece at ({0},{1})", coor.round, coor.pos));
					holder.layChessPiece.chessPieceEntity.Destroy();

					holder.isForbiddenLayout = true;
				}
			}
		}
	}

	#endregion




}
