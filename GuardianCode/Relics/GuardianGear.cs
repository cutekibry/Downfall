using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Guardian.GuardianCode.Cards.Token;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Events;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Relics;

[Pool(typeof(GuardianRelicPool))]
public class GuardianGear : GuardianRelicModel
{
    public GuardianGear() : base(RelicRarity.Starter)
    {
        WithVar("Brace", 39);
        WithTip(GuardianTip.Brace);
    }
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext ctx, ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber > 1) return;
        await GuardianCmd.Brace(ctx, player, DynamicVars["Brace"].BaseValue);
    }
}