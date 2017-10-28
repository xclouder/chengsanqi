using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entitas;
 
public class ChessComboSystem : ReactiveSystem<GameEntity> {

	public class Int2
	{
		public Int2(int round, int pos)
		{
			this.round = round;
			this.pos = pos;
		}

		public int round;
		public int pos;

		public override int GetHashCode ()
		{
			return round.GetHashCode () + pos.GetHashCode();
		}

		public override bool Equals (object obj)
		{
			if (obj is Int2)
			{
				var o = obj as Int2;
				return o.round == round && o.pos == pos;
			}

			return false;
		}
	}

	private List<Int2[]> m_sideList = new List<Int2[]>(20);
	private void AddSide(Int2 one, Int2 two, Int2 three)
	{
		m_sideList.Add(new Int2[]{one, two, three});
	}

	private void AddSideRoundFixed(int round, int pos1, int pos2, int pos3)
	{
		AddSide(new Int2(round, pos1), new Int2(round, pos2), new Int2(round, pos3));
	}

	private void AddSidePositionFixed(int pos)
	{
		AddSide(new Int2(0, pos), new Int2(1, pos), new Int2(2, pos));
	}

	private void AddSides(int round)
	{
		AddSideRoundFixed(round, 0, 1, 2);
		AddSideRoundFixed(round, 2, 3, 4);
		AddSideRoundFixed(round, 4, 5, 6);
		AddSideRoundFixed(round, 6, 7, 0);
	}

	private void InitBoard()
	{
		AddSides(0);
		AddSides(1);
		AddSides(2);

		AddSidePositionFixed(1);
		AddSidePositionFixed(3);
		AddSidePositionFixed(5);
		AddSidePositionFixed(7);

		BuildIndex();
	}


	private Dictionary<Int2, List<Int2[]>> m_dict = new Dictionary<Int2, List<Int2[]>>(50);
	private void BuildIndex()
	{
		foreach (var sideArr in m_sideList)
		{
			for (int i = 0; i < sideArr.Length; i++)
			{
				var coor = sideArr[i];

				List<Int2[]> _list;
				if (!m_dict.TryGetValue(coor, out _list))
				{
					_list = new List<Int2[]>(5);

					m_dict.Add(coor, _list);
				}

				_list.Add(sideArr);
			}
		}
	}

	readonly GameContext m_gameContext;

	public ChessComboSystem(Contexts contexts) : base(contexts.game)
	{
		InitBoard();

		m_gameContext = contexts.game;

		var grp = m_gameContext.GetGroup (GameMatcher.ChessPiece);
		grp.OnEntityRemoved += (group, entity, index, component) => {

			var c = entity.coordinate;
			var coor = new Int2(c.round, c.pos);

			if (m_chessPieceDict.ContainsKey(coor))
			{
				m_chessPieceDict.Remove(coor);
			}
			else
			{
				Debug.LogError("no this coor");
			}

		};

		grp.OnEntityAdded += (group, e, index, component) => {

			Debug.Log(string.Format("add chess piece to dict, pos:({0},{1}), isWhite:{2}", e.coordinate.round, e.coordinate.pos, e.chessPiece.isWhite));

			var coor = new Int2(e.coordinate.round, e.coordinate.pos);
			m_chessPieceDict.Add(coor, e.chessPiece);

		};

		var actStateGrp = m_gameContext.GetGroup(GameMatcher.ActionState);
		actStateGrp.OnEntityUpdated += (group, entity, index, previousComponent, newComponent) => {

			if (m_gameContext.actionState.actionState != ActionState.WaitCheckCombo)
			{
				return;
			}

			if (!m_gameContext.hasPreviousActionChessPiece)
			{
				Debug.LogError("previousActionChessPiece is null");
				return;
			}

			var chessEntity = m_gameContext.previousActionChessPiece.chessPieceEntity;
			var chess = chessEntity.chessPiece;
			var coor = new Int2(chessEntity.coordinate.round, chessEntity.coordinate.pos);
			if (CheckHasCombo(coor, chess.isWhite))
			{
				Debug.Log("Combo! isWhite:" + chess.isWhite);

				m_gameContext.ReplaceActionState(ActionState.KillChess);

			}
			else
			{
				m_gameContext.ReplaceActionState(ActionState.End);
			}

		};


	}

	#region implemented abstract members of ReactiveSystem

	private Dictionary<Int2, ChessPieceComponent> m_chessPieceDict = new Dictionary<Int2, ChessPieceComponent>(50);

	protected override ICollector<GameEntity> GetTrigger (IContext<GameEntity> context)
	{
		//add or "update"?
		return context.CreateCollector(GameMatcher.ChessPiece);
	}

	protected override bool Filter (GameEntity entity)
	{
		return entity.hasChessPiece;
	}



	protected override void Execute (System.Collections.Generic.List<GameEntity> entities)
	{
//		foreach (var e in entities)
//		{
//			Debug.Log(string.Format("add chess piece to dict, pos:({0},{1}), isWhite:{2}", e.coordinate.round, e.coordinate.pos, e.chessPiece.isWhite));
//
//			var coor = new Int2(e.coordinate.round, e.coordinate.pos);
//			m_chessPieceDict.Add(coor, e.chessPiece);
//
//		}
	}

	bool CheckHasCombo(Int2 coor, bool isWhite)
	{
		bool needWhite = isWhite;
		List<Int2[]> _list;
		if (m_dict.TryGetValue(coor, out _list))
		{
			foreach (var arr in _list)
			{
				int satisfiedCount = 0;
				foreach (var c in arr)
				{
					ChessPieceComponent comp;
					if (m_chessPieceDict.TryGetValue(c, out comp))
					{
						if (comp.isWhite == needWhite)
						{
							satisfiedCount++;
						}
					}

				}

				if (satisfiedCount >= 3)
				{
					return true;
				}
			}

			return false;
		}
		else
		{
			Debug.LogError("no chess piece at this coorination");
			return false;
		}
	}

	#endregion




}
