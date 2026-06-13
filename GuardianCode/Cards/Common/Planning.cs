using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;


namespace Guardian.GuardianCode.Cards.Common;

[Pool(typeof(GuardianCardPool))]
public class Planning : GuardianCardModel, IGemSocketCard
{
    public int GemSlots => IsUpgraded ? 2 : 1;

    public Planning() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithTip(GuardianTip.Stasis);
        WithCards(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            if (!Owner.GetDraw().Any())
                await CardPileCmd.Shuffle(ctx, Owner);
            var card = Owner.GetDraw().ToList().FirstOrDefault();
            if (card == null) return;
            await GuardianCmd.PutIntoStasis(card, ctx, this);
        }
    }
}
