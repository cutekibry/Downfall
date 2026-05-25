using System.Reflection;
using BaseLib.Patches.Saves;
using Godot;
using Godot.Bridge;
using Gremlins.GremlinsCode.Core;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace Gremlins.GremlinsCode;

[ModInitializer(nameof(Initialize))]
public partial class GremlinsMainFile : Node
{
    public const string ModId = "Gremlins"; //At the moment, this is used only for the Logger and harmony names.

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        ExtendedSaveTypes.RegisterObjectSaveType<GremlinSaveData>(
            ExtendedSaveTypes.PropertyFunc<GremlinSaveData, ModelId>("ModelId"),
            ExtendedSaveTypes.PropertyFunc<GremlinSaveData, int>("Hp"),
            ExtendedSaveTypes.PropertyFunc<GremlinSaveData, int>("MaxHp")
        );
        ExtendedSaveTypes.RegisterListSaveType<GremlinSaveData>();
        Harmony harmony = new(ModId);
        var assembly = Assembly.GetExecutingAssembly();
        ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        harmony.PatchAll();
    }
}