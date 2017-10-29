using UnityEngine;
using System.Collections;
using Entitas;

public class GameLogicSystem : ReactiveSystem<InputEntity> {

	private GameContext _gameContext;
	private const int MAX_DROP_ROUND = 5;
	private IGroup<GameEntity> m_holderGroup;

	public GameLogicSystem(Contexts contexts) : base(contexts.input)
	{
		_gameContext = contexts.game;

		contexts.input.GetGroup(InputMatcher.SelectChessHolder).OnEntityAdded += OnSelectChessPieceHolder;

		m_holderGroup = _gameContext.GetGroup(GameMatcher.ChessPieceHolder);

		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityAdded += OnAddChessPiece;
		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityRemoved += OnRemoveChessPiece;
		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityUpdated += OnUpdateChessPiece;

		_gameContext.GetGroup(GameMatcher.ResetGame).OnEntityAdded += OnResetGame;

		_gameContext.GetGroup(GameMatcher.ActionState).OnEntityUpdated += OnUpdateActionState;

		InitGame();
	}

	private void InitGame()
	{
		_gameContext.SetGameState(GameState.DropChess);
		_gameContext.SetDropChessState(1);

		_gameContext.SetActionState(ActionState.Start);
		_gameContext.SetTurnState(Turn.White);
		_gameContext.SetPreviousActionChessPiece(null);
	}

	private void OnUpdateActionState(IGroup<GameEntity> grp, GameEntity e, int index, IComponent comp, IComponent newComp)
	{
		Debug.Log("ActionState chagne to:" + e.actionState.actionState);
		if (e.actionState.actionState == ActionState.End)
		{
			if (_gameContext.turnState.turn == Turn.Black)
			{
				var r = _gameContext.dropChessState.round;
				if (_gameContext.gameState.gameState == GameState.DropChess && r >= MAX_DROP_ROUND)
				{
					Debug.LogWarning ("Enter Walk Phase");
					_gameContext.ReplaceGameState(GameState.WalkChess);
				}
				else
				{
					_gameContext.ReplaceDropChessState(r + 1);
				}
			}


			if (IsGameFinished())
			{
				Debug.LogError("Game Over, Winner:" + _gameContext.turnState.turn);
			}
			else
			{
				//update turn
				RevertTurn();

				_gameContext.ReplaceActionState(ActionState.Start);
			}


		}
		else if (e.actionState.actionState == ActionState.KillChess)
		{
			//tip select his chess piece to kill
			Debug.Log("please select his chess piece to kill");
		}
		else if (e.actionState.actionState == ActionState.WaitCheckCombo)
		{
			
		}
	}

	private void OnResetGame(IGroup<GameEntity> grp, GameEntity e, int index, IComponent comp)
	{
		Debug.Log("ResetGame");

		_gameContext.ReplaceGameState(GameState.DropChess);
		_gameContext.ReplaceDropChessState(0);

		_gameContext.ReplaceActionState(ActionState.Start);
		_gameContext.ReplaceTurnState(Turn.White);
//		_gameContext

	}

	private void OnSelectChessPieceHolder(IGroup<InputEntity> grp, InputEntity e, int index, IComponent comp)
	{
		if (_gameContext.actionState.actionState != ActionState.Start)
		{
			return;
		}

		Debug.LogFormat("select chess piece holder ({0}, {1})", e.selectChessHolder.chessHolder.coordinate.round, e.selectChessHolder.chessHolder.coordinate.pos);

		var currGameState = _gameContext.gameState.gameState;
		var currTurn = _gameContext.turnState.turn;

		switch (currGameState)
		{
		case GameState.DropChess:
			{
				var holder = e.selectChessHolder.chessHolder;

				if (holder.hasLayChessPiece)
				{
					Debug.Log("has lay chess piece, ignore");
					return;
				}

				if (holder.isForbiddenLayout) {
					Debug.LogError ("this coor is forbidden to lay chess piece");
					return;
				}

				var chess = _gameContext.CreateEntity();
				chess.AddPosition(holder.position.position);
				chess.AddCoordinate(holder.coordinate.round, holder.coordinate.pos);

				bool isWhite = currTurn == Turn.White;
				chess.AddChessPiece(isWhite);

				//link holder and chess piece
				holder.AddLayChessPiece(chess.chessPiece, chess);

				Debug.Log("set previous chess:" + chess.coordinate);
				_gameContext.ReplacePreviousActionChessPiece(chess);
				_gameContext.ReplaceActionState(ActionState.WaitCheckCombo);

				break;
			}
		case GameState.WalkChess:
			{
				var chessHolder = e.selectChessHolder.chessHolder;

				if (!_gameContext.hasWalkSelectedChessPiece)
				{
					//select a chess piece for current turn
					if (chessHolder.hasLayChessPiece)
					{
						var isWhite = (currTurn == Turn.White);
						if (chessHolder.layChessPiece.chessPiece.isWhite == isWhite)
						{
							chessHolder.layChessPiece.chessPieceEntity.isChessPieceSelected = true;
							//select it
							_gameContext.SetWalkSelectedChessPiece(chessHolder.layChessPiece.chessPieceEntity);

							Debug.LogFormat("select chess at:({0},{1})", chessHolder.coordinate.round, chessHolder.coordinate.pos);
						}
						else
						{
							//not yours
						}
					}
				}
				else
				{
					//walk to
					if (chessHolder.hasLayChessPiece)
					{
						//TODO allow reselect
						return;
					}
					else
					{
						var selectedChess = _gameContext.walkSelectedChessPiece.chessPieceEntity;
						var checker = _gameContext.comboChecker.comboChecker;

						var fromCoor = new Int2(selectedChess.coordinate.round, selectedChess.coordinate.pos);
						var toCoor = new Int2(chessHolder.coordinate.round, chessHolder.coordinate.pos);
						if (checker.CanWalkTo(fromCoor, toCoor))
						{
							Debug.LogFormat("Walk to ({0}, {1})", toCoor.round, toCoor.pos);
							//remove from origin holder
							foreach (var holder in m_holderGroup.GetEntities())
							{
								if (holder.hasLayChessPiece && holder.layChessPiece.chessPieceEntity == selectedChess)
								{
									holder.RemoveLayChessPiece();

									break;
								}
							}

							_gameContext.ReplacePreviousActionChessPiece(selectedChess);

							//add to new holder
							selectedChess.ReplaceCoordinate(toCoor.round, toCoor.pos);
							selectedChess.ReplacePosition(chessHolder.position.position);
							chessHolder.AddLayChessPiece(selectedChess.chessPiece, selectedChess);

							//_gameContext.SetPreviousActionChessPiece(selectedChess);
							if (checker.CheckHasCombo(toCoor, currTurn == Turn.White))
							{
								_gameContext.ReplaceActionState(ActionState.KillChess);	
							}
							else
							{
								_gameContext.ReplaceActionState(ActionState.End);
							}

							_gameContext.RemoveWalkSelectedChessPiece();

						}
						else
						{
							Debug.LogError("cannot walk to that coordinate");
						}
					}
				}


				break;
			}
		default:
			{
				Debug.LogError("game finished");
				break;
			}
		}


	}

	private void OnAddChessPiece(IGroup<GameEntity> grp, GameEntity entity, int index, IComponent comp)
	{
		//add chess piece

	}

	private void OnRemoveChessPiece(IGroup<GameEntity> grp, GameEntity entity, int index, IComponent comp)
	{
		//removed a chese piece, update GameMode, Turn
//		_gameContext.ReplaceGameMode(GameMode.Normal);
//
//		RevertTurn();
	}

	private void OnUpdateChessPiece(IGroup<GameEntity> grp, GameEntity entity, int index, IComponent comp, IComponent newComp)
	{

	}

	protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
	{
		return context.CreateCollector(InputMatcher.SelectChessHolder);
	}

	protected override bool Filter (InputEntity entity)
	{
		return entity.hasSelectChessHolder;
	}

	protected override void Execute (System.Collections.Generic.List<InputEntity> entities)
	{
		
	}

	void RevertTurn()
	{
		var currTurn = _gameContext.turnState.turn;
		_gameContext.ReplaceTurnState(currTurn == Turn.Black ? Turn.White : Turn.Black);
	}

	bool IsGameFinished()
	{
		bool isWalkState = _gameContext.gameState.gameState == GameState.WalkChess;

		if (isWalkState)
		{
			var turn = _gameContext.turnState.turn;
			bool isWhiteTurn = (turn == Turn.White);

			var num = _gameContext.comboChecker.comboChecker.NumOfChess(isWhiteTurn ? false : true);

			if (num <= 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}


}
