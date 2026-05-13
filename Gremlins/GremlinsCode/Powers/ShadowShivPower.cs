using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Gremlins.GremlinsCode.Powers;

public class ShadowShivPower : GremlinsPowerModel
{

    public ShadowShivPower()
    {
        WithTip(CardKeyword.Exhaust);
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner || cardPlay.Card.Type != CardType.Attack || cardPlay.ResultPile == PileType.Exhaust) return;
        await DownfallCardCmd.GiveCards<Shiv>(cardPlay.Card.Owner, PileType.Hand, Amount);
    }
}