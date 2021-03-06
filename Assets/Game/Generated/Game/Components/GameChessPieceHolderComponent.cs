//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly ChessPieceHolderComponent chessPieceHolderComponent = new ChessPieceHolderComponent();

    public bool isChessPieceHolder {
        get { return HasComponent(GameComponentsLookup.ChessPieceHolder); }
        set {
            if (value != isChessPieceHolder) {
                if (value) {
                    AddComponent(GameComponentsLookup.ChessPieceHolder, chessPieceHolderComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.ChessPieceHolder);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherChessPieceHolder;

    public static Entitas.IMatcher<GameEntity> ChessPieceHolder {
        get {
            if (_matcherChessPieceHolder == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ChessPieceHolder);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherChessPieceHolder = matcher;
            }

            return _matcherChessPieceHolder;
        }
    }
}
