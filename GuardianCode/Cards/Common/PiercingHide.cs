using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Downfall.DownfallCode.Powers;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.Extensions;
using Guardian.GuardianCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Guardian.GuardianCode.Cards.Common;

[Pool(typeof(GuardianCardPool))]
public class PiercingHide : GuardianCardModel, IGemSocketCard
{
    public PiercingHide() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        this.WithSelfDamage(3);
        WithPower<ThornsPower>(5, 2);
    }

    public int GemSlots => 1;

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await MyCommonActions.SelfDamage(ctx, this);
        await CommonActions.ApplySelf<ThornsPower>(ctx, this);
    }
}