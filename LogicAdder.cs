using System.IO;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace RandomizeGladeAccess
{
    public class LogicAdder
    {
        public static void Hook()
        {
            // I have 0 point of reference for these numbers.
            RCData.RuntimeLogicOverride.Subscribe(100f, DefineTermsAndItems);
            RCData.RuntimeLogicOverride.Subscribe(101f, ApplyLogic);
        }
        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandomizeGladeAccess.GS.Enabled) return;
            Term gladeAccessTerm = lmb.GetOrAddTerm("Glade_Access");
            lmb.AddItem(new BoolItem(Consts.GladeAccessItemName, gladeAccessTerm));
        }
        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandomizeGladeAccess.GS.Enabled) return;
            using Stream s = typeof(LogicAdder).Assembly.GetManifestResourceStream("RandomizeGladeAccess.Resources.logic.json");
            lmb.DoLogicEdit(new RawLogicDef("Opened_Glade_Door", "Glade_Access"));
        }
    }
}