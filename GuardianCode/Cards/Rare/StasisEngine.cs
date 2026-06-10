using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Downfall.DownfallCode.CustomEnums;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Cards.Rare;

[Pool(typeof(GuardianCardPool))]
public class StasisEngine : GuardianCardModel
{
    public StasisEngine() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(2, 1);
        WithVar("StasisCards", 3);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(GuardianTip.Stasis);
    }


    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(ctx, DynamicVars.Cards.BaseValue, Owner);

        var cards = (await DownfallCardCmd.SelectFromHand(ctx, DownfallCardSelectorPrefs.StasisSelectionPrompt, DynamicVars["StasisCards"].IntValue, this, optional: true)).ToList();
        foreach (var card in cards)
        {
            await GuardianCmd.PutIntoStasis(card, ctx, this);
        }
    }
}