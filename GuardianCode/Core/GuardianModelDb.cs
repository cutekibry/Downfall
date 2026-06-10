using System.Collections.Generic;
using System.Linq;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace Guardian.GuardianCode.Core;

public static class GuardianModelDb
{
    private static IEnumerable<GemModel>? _allGems;

    public static IEnumerable<GemModel> AllGems
    {
        get
        {
            if (_allGems != null) return _allGems;

            return _allGems = ModelDb.AllAbstractModelSubtypes
                .Where(t => t.IsSubclassOf(typeof(GemModel)))
                .Select(t => (GemModel)ModelDb.Get(t))
                .ToList();
        }
    }

    public static T GuardianMode<T>() where T : GuardianModeModel
    {
        return ModelDb.Get<T>();
    }

    public static T Gem<T>() where T : GemModel
    {
        return ModelDb.Get<T>();
    }

    public static CardReward? GenerateSingleGemReward(Player player, CardCreationOptions rerollOptions)
    {
        var rng = player.PlayerRng.Rewards;
        var gemsByRarity = AllGems
            .GroupBy(g => g.Rarity)
            .ToDictionary(g => g.Key, g => g.ToList());

        var roll = rng.NextInt(100);
        var rarity = roll < 55 ? CardRarity.Common
            : roll < 85 ? CardRarity.Uncommon
            : CardRarity.Rare;

        if (!gemsByRarity.TryGetValue(rarity, out var pool)) return null;

        var candidate = rng.NextItem(pool);
        if (candidate == null) return null;

        var card = candidate.ToCard.ToMutable();
        player.RunState.AddCard(card, player);
        return new CardReward([card], CardCreationSource.Other, player, rerollOptions);
    }
}