using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Guardian.GuardianCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Cards.Uncommon;

[Pool(typeof(GuardianCardPool))]
public class RevengeProtocol : GuardianCardModel
{
    public RevengeProtocol() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<DexterityPower>(1, 2);
    }

    protected override Artist Artist => Artist.Get<AlexMdle>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<DexterityPower>(ctx, this, DynamicVars["DexterityPower"].BaseValue * CombatState!.Enemies.Count);
    }
}