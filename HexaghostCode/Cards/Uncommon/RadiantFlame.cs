using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Downfall.DownfallCode.Powers;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class RadiantFlame : HexaghostCardModel
{
    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e => e.HasPower<SoulBurnPower>()) == true;
    public RadiantFlame() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
        WithTip(typeof(SoulBurnPower));
        WithPower<IntensityPower>(3, 1);
    }

    protected override Artist Artist => Artist.Get<GoofballMcgee>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        var power = cardPlay.Target!.GetPower<SoulBurnPower>();
        if (power != null)
        {
            await PowerCmd.Remove(power);
            await CommonActions.ApplySelf<IntensityPower>(ctx, this);
        }
    }
}