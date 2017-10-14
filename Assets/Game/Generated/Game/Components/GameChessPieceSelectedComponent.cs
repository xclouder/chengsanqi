//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly ChessPieceSelectedComponent chessPieceSelectedComponent = new ChessPieceSelectedComponent();

    public bool isChessPieceSelected {
        get { return HasComponent(GameComponentsLookup.ChessPieceSelected); }
        set {
            if (value != isChessPieceSelected) {
                if (value) {
                    AddComponent(GameComponentsLookup.ChessPieceSelected, chessPieceSelectedComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.ChessPieceSelected);
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

    static Entitas.IMatcher<GameEntity> _matcherChessPieceSelected;

    public static Entitas.IMatcher<GameEntity> ChessPieceSelected {
        get {
            if (_matcherChessPieceSelected == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ChessPieceSelected);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherChessPieceSelected = matcher;
            }

            return _matcherChessPieceSelected;
        }
    }
}
