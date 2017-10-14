using UnityEngine;
using System.Collections;
using Entitas;


public class ChessPieceComponent : IComponent {

}


public class PositionComponent : IComponent
{
	public int round;
	public int pos;
}

public class ChessPieceHolderComponent : IComponent
{

}

public class ChessPieceSelectedComponent : IComponent
{

}

public class ViewComponent : IComponent
{
	public GameObject gameObject;
}

public class SpriteComponent : IComponent
{
	public string name;
}