using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Interfaces;
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
        var cards = Owner.GetDraw().Take(DynamicVars.Cards.IntValue).ToList();
        foreach (var card in cards)
        {
            await GuardianCmd.PutIntoStasis(card, ctx, this);
        }
    }
}
