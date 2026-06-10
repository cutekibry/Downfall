using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Downfall.DownfallCode.Commands;
using Downfall.DownfallCode.CustomEnums;
using Guardian.GuardianCode.Cards.Token;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Cards.Ancient;

[Pool(typeof(GuardianCardPool))]
public class StasisEgg : GuardianCardModel
{
    public StasisEgg() : base(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
    {
        WithUpgradingCardTip<GearUp>();
        WithTip(GuardianTip.Stasis);
    }

    protected override Artist Artist => Artist.Get<AlexMdle>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var card = Owner.RunState.CreateCard<GearUp>(Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);

        var candidates = Owner.GetDraw().Concat(Owner.GetHand()).Concat(Owner.GetDiscard()).ToList();
        var selected = (await DownfallCardCmd.SelectFromCards(ctx, candidates, DownfallCardSelectorPrefs.StasisSelectionPrompt, this)).FirstOrDefault();
        if (selected != null)
            await GuardianCmd.PutIntoStasis(selected, ctx, this);
    }
}