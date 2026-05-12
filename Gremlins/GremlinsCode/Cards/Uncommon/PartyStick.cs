using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Events;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class PartyStick : GremlinsCardModel, IAfterGremlinSwap
{
    public PartyStick() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
        WithKeyword(CardKeyword.Unplayable);
        WithEnergy(1);
    }

    public async Task AfterGremlinSwap(PlayerChoiceContext ctx, Player player, GremlinSwapType  gremlinSwapType)
    {
        if (Owner != player || Pile is not { Type: PileType.Hand } || gremlinSwapType != GremlinSwapType.Move) return;
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}