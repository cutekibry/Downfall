using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Interfaces;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Cards.Rare;

[Pool(typeof(GuardianCardPool))]
public class StasisField : GuardianCardModel, ITickCard
{
    public StasisField() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithBlock(11, 2);
        WithVar("Increase", 2, 1);
    }

    public Task OnTick(PlayerChoiceContext ctx)
    {
        DynamicVars.Block.UpgradeValueBy(DynamicVars["Increase"].IntValue);
        return Task.CompletedTask;
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
    }
}