using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Cards.Uncommon;

[Pool(typeof(GuardianCardPool))]
public class OrbSupport : GuardianCardModel
{
    public OrbSupport() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(8, 4);
        WithKeyword(CardKeyword.Innate);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(GuardianTip.Stasis);
    }


    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        var card = Owner.GetDeck().TakeRandom(1, Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
        if (card != null)
        {
            await GuardianCmd.PutIntoStasis(card, ctx, this);
        }
    }
}