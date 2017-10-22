using UnityEngine;
using System.Collections;
using Entitas;

public class GameLogicSystem : ReactiveSystem<InputEntity> {

	private GameContext _gameContext;
	private const int MAX_DROP_ROUND = 9;

	public GameLogicSystem(Contexts contexts) : base(contexts.input)
	{
		_gameContext = contexts.game;

		contexts.input.GetGroup(InputMatcher.SelectChessHolder).OnEntityAdded += OnSelectChessPieceHolder;

		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityAdded += OnAddChessPiece;
		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityRemoved += OnRemoveChessPiece;
		_gameContext.GetGroup(GameMatcher.ChessPiece).OnEntityUpdated += OnUpdateChessPiece;

		_gameContext.GetGroup(GameMatcher.ResetGame).OnEntityAdded += OnResetGame;

		_gameContext.GetGroup(GameMatcher.GameMode).OnEntityUpdated += OnUpdateGameMode;

		InitGame();
	}

	private void InitGame()
	{
		_gameContext.SetGameState(GameState.DropChess);
		_gameContext.SetDropChessState(0);

		_gameContext.SetGameMode(GameMode.Normal);
		_gameContext.SetTurnState(Turn.White);
	}

	private void OnUpdateGameMode(IGroup<GameEntity> grp, GameEntity e, int index, IComponent comp, IComponent newComp)
	{
		if (e.gameMode.gameMode == GameMode.Normal)
		{
			//update turn
			RevertTurn();
		}
		else
		{
			//tip select his chess piece to kill
			Debug.Log("please select his chess piece to kill");

		}
	}

	private void OnResetGame(IGroup<GameEntity> grp, GameEntity e, int index, IComponent comp)
	{
		Debug.Log("ResetGame");

		_gameContext.ReplaceGameState(GameState.DropChess);
		_gameContext.ReplaceDropChessState(0);

		_gameContext.ReplaceGameMode(GameMode.Normal);
		_gameContext.ReplaceTurnState(Turn.White);

	}

	private void OnSelectChessPieceHolder(IGroup<InputEntity> grp, InputEntity e, int index, IComponent comp)
	{
		if (_gameContext.gameMode.gameMode == GameMode.KillChess)
		{
			return;
		}

		Debug.Log("select chess piece holder");

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

				var chess = _gameContext.CreateEntity();
				chess.AddPosition(holder.position.position);
				chess.AddCoordinate(holder.coordinate.round, holder.coordinate.pos);

				bool isWhite = currTurn == Turn.White;
				chess.AddChessPiece(isWhite);

				if (!isWhite)
				{
					var currRound = _gameContext.dropChessState.round;
					if (currRound >= MAX_DROP_ROUND)
					{
						_gameContext.ReplaceGameState(GameState.WalkChess);
					}
					else
					{
						_gameContext.ReplaceDropChessState(_gameContext.dropChessState.round + 1);
					}
				}

				//link holder and chess piece
				holder.AddLayChessPiece(chess.chessPiece, chess);

				//revert turn
				RevertTurn();
				break;
			}
		case GameState.WalkChess:
			{
				//revert turn
				RevertTurn();
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


}
