using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Cards.Uncommon;

[Pool(typeof(GuardianCardPool))]
public class TimeBomb : GuardianCardModel
{
    public TimeBomb() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(15, 4);
        WithTip(GuardianTip.Stasis);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx); 
        var card = CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.AllCards, 1, Owner.RunState.Rng.CombatCardGeneration).First();
        var result = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        await GuardianCmd.PutIntoStasis(result.cardAdded, ctx, this);
    }
}