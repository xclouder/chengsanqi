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

public class LayChessPieceComponent : IComponent
{
	public ChessPieceComponent chessPiece;
}

public class ChessPieceSelectedComponent : IComponent
{

}

public class ViewComponent : IComponent
{
	public GameObject gameObject;

	private SpriteRenderer m_icon;
	private SpriteRenderer m_selectedIcon;

	public void Init(ChessPieceComponent comp)
	{
		var parent = gameObject.transform;
		var iconObj = new GameObject("icon");
		m_icon = iconObj.AddComponent<SpriteRenderer>();

		iconObj.transform.SetParent(parent, false);
		iconObj.transform.localPosition = Vector3.zero;

		var selObj = new GameObject("selected");
		m_selectedIcon = selObj.AddComponent<SpriteRenderer>();
		selObj.transform.SetParent(parent, false);
		selObj.transform.localPosition = Vector3.zero;

		m_icon.sprite = (Sprite)Resources.Load("Images/chess", typeof(Sprite));
		if (comp.isWhite)
		{
			m_icon.color = Color.white;
		}
		else
		{
			m_icon.color = Color.black;
		}

		selObj.SetActive(false);
	}

	public void SetSelected(bool sel)
	{
		m_selectedIcon.gameObject.SetActive(sel);
	}
}

public enum GameState
{
	Prepare,		//未开始
	DropChess,		//落子
	WalkChess,		//走棋
	End				//结束
}

[Unique]
public class GameStateComponent : IComponent
{
	public GameState gameState;
}

public enum Turn
{
	White,
	Black
}


public enum GameMode
{
	Normal,
	KillChess,		//摘子
}

[Unique]
public class GameModeComponent : IComponent
{
	public GameMode gameMode;
}


[Unique]
public class TurnStateComponent : IComponent
{
	public Turn turn;
}

[Unique]
public class DropChessStateComponent : IComponent
{
	public int round;
}

public class GameEndStateComponent : IComponent
{
	public Turn winner;
}

public class ResetGameComponent : IComponent
{

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

