using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;


namespace Guardian.GuardianCode.Cards.Common;

[Pool(typeof(GuardianCardPool))]
public class ChargeCore : GuardianCardModel, ITickCard
{
    public ChargeCore() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithKeyword(GuardianKeyword.Volatile);
        WithTip(CardKeyword.Exhaust);
        WithDamage(10, 5);
        WithCalculatedDamage("RandomDamage", 6, (_, _) => 0);
    }


    public async Task OnTick(PlayerChoiceContext ctx)
    {
        await CardPileCmd.Draw(ctx, 1, Owner);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay.Target, DynamicVars.Damage.BaseValue).Execute(ctx);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext ctx, CardModel card, bool causedByEthereal)
    {
        if (card != this) return;
        var enemy = CombatState!.HittableEnemies.TakeRandom(1, Owner.RunState.Rng.CombatTargets).FirstOrDefault();
        await CommonActions.CardAttack(this, enemy, DynamicVars["RandomDamageBase"].BaseValue).Execute(ctx);
    }
}
