using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Guardian.GuardianCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Guardian.GuardianCode.Cards.Common;

[Pool(typeof(GuardianCardPool))]
public class WalkerClaw : GuardianCardModel
{
    public WalkerClaw() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(8, 3);
        WithCalculatedVar("Hit", 0, GetHitLastTurn);
    }

    private static decimal GetHitLastTurn(CardModel card, Creature? _)
    {
        return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>()
            .Count(e => 
                e.RoundNumber == card.CombatState!.RoundNumber - 1
                && e.Dealer!.IsEnemy
                && e.Receiver == card.Owner.Creature
            );
    }

    protected override Artist Artist => Artist.Get<AlexMdle>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay, (int)this.GetCalculatedValue("Hit", cardPlay) + 1).Execute(ctx);
    }
}