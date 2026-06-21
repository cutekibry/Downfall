using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class MakeshiftArmor : GremlinsCardModel
{
    public MakeshiftArmor() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ArtifactPower>(0, 1);
        WithPower<MakeshiftArmorPower>(1);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<ArtifactPower>(ctx, this);
        await CommonActions.ApplySelf<MakeshiftArmorPower>(ctx, this);
    }
}