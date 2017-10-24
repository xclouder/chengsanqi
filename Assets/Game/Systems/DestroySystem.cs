using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;

//public interface IDestroyed {}
public interface IDestroyEntity : IEntity, IDestroyed { }

// tell the compiler that our context-specific entities implement IDestroyed
public partial class GameEntity : IDestroyEntity { }
public partial class InputEntity : IDestroyEntity { }

public class DestroySystem : MultiReactiveSystem<IDestroyEntity, Contexts>
{
	// base class takes in all contexts, not just one as in normal ReactiveSystems
	public DestroySystem(Contexts contexts) : base(contexts)
	{
	}

	// return an ICollector[] with a collector from each context
	protected override ICollector[] GetTrigger(Contexts contexts)
	{
		return new ICollector[] {
			
			contexts.game.CreateCollector(GameMatcher.Destroyed),
			contexts.input.CreateCollector(InputMatcher.Destroyed),
		};
	}

	protected override bool Filter(IDestroyEntity entity)
	{
		return entity.isDestroyed;
	}

	protected override void Execute(List<IDestroyEntity> entities)
	{
		foreach (var e in entities)
		{
			Debug.Log("Destroyed Entity from " + e.contextInfo.name + " context");
			e.Destroy();
		}
	}

}
