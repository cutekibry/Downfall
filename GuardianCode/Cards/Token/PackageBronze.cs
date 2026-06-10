using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Downfall.DownfallCode.CustomEnums;
using Guardian.GuardianCode.Cards.Abstract;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Guardian.GuardianCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class PackageBronze : GuardianCardModel, IPackageCard
{
    public PackageBronze() : base(0, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
    {
        WithDamage(12, 4);
        WithTip(GuardianTip.Stasis);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);

        if (GuardianCmd.CanPutIntoStasis(Owner))
        {
            var card = (await DownfallCardCmd.SelectFromHand(ctx, DownfallCardSelectorPrefs.StasisSelectionPrompt, this)).FirstOrDefault();
            if (card == null) return;
            await GuardianCmd.PutIntoStasis(card, ctx, this);
        }
    }
}