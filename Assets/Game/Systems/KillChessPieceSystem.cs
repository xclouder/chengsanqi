using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.Unity;

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
		return entity.hasSelectChessHolder && m_gameContext.actionState.actionState == ActionState.KillChess;
	}

	protected override void Execute (System.Collections.Generic.List<InputEntity> entities)
	{
		var currTurn = m_gameContext.turnState.turn;
		foreach (var e in entities)
		{
			var holder = e.selectChessHolder.chessHolder;
			if (holder.hasLayChessPiece)
			{
				if (holder.layChessPiece.chessPieceEntity == m_gameContext.previousActionChessPiece.chessPieceEntity)
				{
					continue;
				}

				bool isWhite = holder.layChessPiece.chessPiece.isWhite;
				bool needWhite = currTurn == Turn.White ? false : true;
				if (isWhite == needWhite)
				{
					var coor = holder.layChessPiece.chessPieceEntity.coordinate;

					if (m_gameContext.comboChecker.comboChecker.CheckHasCombo(new Int2(coor.round, coor.pos), isWhite))
					{
						Debug.LogError("cannot kill comboed chess piece");
						return;
					}

					Debug.Log(string.Format("trigger destroy chess piece at ({0},{1})", coor.round, coor.pos));
					var piece = holder.layChessPiece.chessPieceEntity;
					holder.RemoveLayChessPiece ();
					holder.isForbiddenLayout = true;

					var o = piece.view.gameObject;
					o.Unlink ();
					Object.Destroy (o);

					piece.Destroy();


					m_gameContext.ReplaceActionState(ActionState.End);

				}
			}
		}
	}

	#endregion




}
