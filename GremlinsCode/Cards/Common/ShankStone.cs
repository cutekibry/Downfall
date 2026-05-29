using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Gremlins.GremlinsCode.Cards.Common;

[Pool(typeof(GremlinsCardPool))]
public class ShankStone : GremlinsCardModel
{
    public ShankStone() : base(-1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithCards(2, 1);
        WithKeyword(CardKeyword.Unplayable);
        this.WithTip<Shiv>();
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this) return;
        await DownfallCardCmd.GiveCards<Shiv>(Owner, PileType.Hand, DynamicVars.Cards.IntValue);
    }
}