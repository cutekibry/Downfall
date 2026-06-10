using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Interfaces;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Cards.Common;

[Pool(typeof(GuardianCardPool))]
public class PrimingShot : GuardianCardModel, IGemSocketCard
{
    public PrimingShot() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
        WithCalculatedVar("Brace", 6, GetStrengthAmount, 2);
        WithTip(typeof(StrengthPower));
        WithTip(GuardianTip.Brace);
    }

    private static decimal GetStrengthAmount(CardModel card, Creature? _)
    {
        return card.Owner.Creature.GetPowerAmount<StrengthPower>();
    }

    public int GemSlots => 1;

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        await GuardianCmd.Brace(ctx, Owner, this.GetCalculatedValue("Brace", cardPlay));
    }
}