using UnityEngine;
using System.Collections;
using Entitas;

public class EmitSelectHolderSystem : ReactiveSystem<InputEntity> {
	#region implemented abstract members of ReactiveSystem

	readonly IGroup<GameEntity> _holders;

	readonly InputContext _inputContext;
	public EmitSelectHolderSystem(Contexts contexts) : base(contexts.input)
	{
		_inputContext = contexts.input;
		_holders = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ChessPieceHolder));
	}

	protected override ICollector<InputEntity> GetTrigger (IContext<InputEntity> context)
	{
		return context.CreateCollector(InputMatcher.AllOf(InputMatcher.LeftMouse, InputMatcher.MouseDown));
	}

	protected override bool Filter (InputEntity entity)
	{
		return entity.hasMouseDown;
	}

	private float m_holderRadius = 1f;
	protected override void Execute (System.Collections.Generic.List<InputEntity> entities)
	{
		foreach (var e in entities)
		{
			GameEntity[] holders = _holders.GetEntities();
			foreach (var h in holders)
			{
				if (Vector2.Distance(h.position.position, e.mousePosition.position) <= m_holderRadius)
				{
					Debug.Log("emit a select chessholder");

					var emitEntity = _inputContext.CreateEntity();
					emitEntity.AddSelectChessHolder(h);
				}
			}
		}
	}

	#endregion




}
