using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.CodeGeneration.Attributes;

public class ChessPieceComponent : IComponent {
	public bool isWhite;
}

public class PositionComponent : IComponent
{
	public Vector3 position;
}

public class CoordinateComponent : IComponent
{
	public int round;
	public int pos;
}

public class ForbiddenLayoutComponent : IComponent
{
	
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


//input
[Input, Unique]
public class LeftMouseComponent : IComponent
{
}

[Input, Unique]
public class RightMouseComponent : IComponent
{
}

[Input]
public class MouseDownComponent : IComponent
{
	public Vector2 position;
}

[Input]
public class MousePositionComponent : IComponent
{
	public Vector2 position;
}

[Input]
public class MouseUpComponent : IComponent
{
	public Vector2 position;
}

[Input]
public class SelectChessHolderComponent : IComponent
{
	public GameEntity chessHolder;
}