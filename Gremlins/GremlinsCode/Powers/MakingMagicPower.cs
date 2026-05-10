using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Powers;

public class MakingMagicPower : GremlinsPowerModel
{

    public MakingMagicPower()
    {
        WithTip(typeof(Bang));
    }
    
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext ctx, ICombatState combatState)
    {
        if (player.Creature != Owner) return;
        await DownfallCardCmd.GiveCards<Bang>(player, PileType.Hand, Amount);
    }
}