using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;

public class AddChessSystem : ReactiveSystem<InputEntity> {

	readonly Transform _viewContainer = new GameObject("Chess Views").transform;
	readonly GameContext _context;

	public AddChessSystem(Contexts contexts) : base(contexts.input)
	{
		_context = contexts.game;
	}

	protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
	{
		return context.CreateCollector(InputMatcher.SelectChessHolder);
	}

	protected override bool Filter(InputEntity entity)
	{
		//is time for create chess entity?
		return entity.hasSelectChessHolder;
	}

	protected override void Execute(List<InputEntity> entities)
	{
		foreach (InputEntity e in entities)
		{
			var holder = e.selectChessHolder.chessHolder;

			var chess = _context.CreateEntity();
			chess.AddPosition(holder.position.position);
			chess.AddCoordinate(holder.coordinate.round, holder.coordinate.pos);

			GameObject go = new GameObject("Chess View");
			go.transform.position = chess.position.position;
			go.transform.SetParent(_viewContainer, false);
			chess.AddView(go);
			go.Link(chess, _context);

			chess.AddChessPiece(true);
		}
	}

}
