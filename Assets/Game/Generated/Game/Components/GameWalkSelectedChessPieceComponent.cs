//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity walkSelectedChessPieceEntity { get { return GetGroup(GameMatcher.WalkSelectedChessPiece).GetSingleEntity(); } }
    public WalkSelectedChessPieceComponent walkSelectedChessPiece { get { return walkSelectedChessPieceEntity.walkSelectedChessPiece; } }
    public bool hasWalkSelectedChessPiece { get { return walkSelectedChessPieceEntity != null; } }

    public GameEntity SetWalkSelectedChessPiece(GameEntity newChessPieceEntity) {
        if (hasWalkSelectedChessPiece) {
            throw new Entitas.EntitasException("Could not set WalkSelectedChessPiece!\n" + this + " already has an entity with WalkSelectedChessPieceComponent!",
                "You should check if the context already has a walkSelectedChessPieceEntity before setting it or use context.ReplaceWalkSelectedChessPiece().");
        }
        var entity = CreateEntity();
        entity.AddWalkSelectedChessPiece(newChessPieceEntity);
        return entity;
    }

    public void ReplaceWalkSelectedChessPiece(GameEntity newChessPieceEntity) {
        var entity = walkSelectedChessPieceEntity;
        if (entity == null) {
            entity = SetWalkSelectedChessPiece(newChessPieceEntity);
        } else {
            entity.ReplaceWalkSelectedChessPiece(newChessPieceEntity);
        }
    }

    public void RemoveWalkSelectedChessPiece() {
        walkSelectedChessPieceEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public WalkSelectedChessPieceComponent walkSelectedChessPiece { get { return (WalkSelectedChessPieceComponent)GetComponent(GameComponentsLookup.WalkSelectedChessPiece); } }
    public bool hasWalkSelectedChessPiece { get { return HasComponent(GameComponentsLookup.WalkSelectedChessPiece); } }

    public void AddWalkSelectedChessPiece(GameEntity newChessPieceEntity) {
        var index = GameComponentsLookup.WalkSelectedChessPiece;
        var component = CreateComponent<WalkSelectedChessPieceComponent>(index);
        component.chessPieceEntity = newChessPieceEntity;
        AddComponent(index, component);
    }

    public void ReplaceWalkSelectedChessPiece(GameEntity newChessPieceEntity) {
        var index = GameComponentsLookup.WalkSelectedChessPiece;
        var component = CreateComponent<WalkSelectedChessPieceComponent>(index);
        component.chessPieceEntity = newChessPieceEntity;
        ReplaceComponent(index, component);
    }

    public void RemoveWalkSelectedChessPiece() {
        RemoveComponent(GameComponentsLookup.WalkSelectedChessPiece);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherWalkSelectedChessPiece;

    public static Entitas.IMatcher<GameEntity> WalkSelectedChessPiece {
        get {
            if (_matcherWalkSelectedChessPiece == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.WalkSelectedChessPiece);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherWalkSelectedChessPiece = matcher;
            }

            return _matcherWalkSelectedChessPiece;
        }
    }
}
