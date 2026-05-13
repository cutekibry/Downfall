using System.Text.Json.Serialization;
using Downfall.DownfallCode.Saves;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace Gremlins.GremlinsCode.Core;

public class GremlinState
{
    private static readonly Logger Log = GremlinsMainFile.Logger;

    private readonly LinkedList<Creature> _rotation = [];
    private readonly List<Creature> _gremlins = [];
    
    public Creature? Next => _rotation.First?.Next?.Value;
    public IReadOnlyList<Creature> Gremlins => _gremlins;
    public Creature? Active => _rotation.First?.Value;
    public IEnumerable<Creature> Bench => _rotation.Skip(1);

    internal void Register(Creature gremlin)
    {
        ArgumentNullException.ThrowIfNull(gremlin);
        if (_gremlins.Contains(gremlin)) throw new InvalidOperationException($"{gremlin} already registered.");
        _gremlins.Add(gremlin);
        _rotation.AddLast(gremlin);
    }

    internal void SwapTo(Creature target)
    {
        if (!_rotation.Contains(target)) throw new InvalidOperationException($"{target} is not alive.");
        if (target == Active) return;
        var current = _rotation.First!.Value;
        _rotation.RemoveFirst();
        _rotation.AddLast(current);
        _rotation.Remove(target);
        _rotation.AddFirst(target);
    }

    internal void Kill(Creature gremlin)
    {
        if (!_gremlins.Contains(gremlin)) throw new ArgumentException($"Unknown gremlin {gremlin}.");
        _rotation.Remove(gremlin);
        _gremlins.Remove(gremlin);
    }

    internal void Reset()
    {
        _rotation.Clear();
        _gremlins.Clear();
    }
    
    public List<GremlinSaveData> ToSaveData(Player player) => _rotation
        .Where(g => g.Monster is GremlinsMonsterModel { ShouldSave: true })
        .Select(g => new GremlinSaveData
        {
            ModelId = g.Monster!.Id,
            Hp      = g == Active ? player.Creature.CurrentHp : g.CurrentHp,
            MaxHp   = g == Active ? player.Creature.MaxHp : g.MaxHp,
        }).ToList();
}