using Baselib.Abstracts;
using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Basic;

[Pool(typeof(GremlinsCardPool))]
public class StrikeGremlins : GremlinsCardModel
{
    public StrikeGremlins() : base(1, CardType.Attack, CardRarity.Basic, CustomTargetType.AllFullLifeEnemies)
    {
        WithTags(CardTag.Strike);
        WithDamage(6, 3);
    }
    

    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        //await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(ctx);
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
    }
}