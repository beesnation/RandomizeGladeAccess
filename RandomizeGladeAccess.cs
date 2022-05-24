using Modding;
using System.Collections.Generic;
using System.IO;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using RandomizerMod.Logging;
using RandomizerMod.RC;
using UnityEngine;

namespace RandomizeGladeAccess
{
    public class RandomizeGladeAccess : Mod, IGlobalSettings<GlobalSettings>
    {
        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();
        internal static RandomizeGladeAccess Instance;
        public static GlobalSettings GS = new();
        public void OnLoadGlobal(GlobalSettings gs) => GS = gs;
        public GlobalSettings OnSaveGlobal() => GS;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");
            
            Instance = this;
            
            SettingsLog.AfterLogSettings += AddGladeAccessSettings;
            
            var icExists = ModHooks.GetMod("ItemChangerMod") is Mod;
            if (!icExists) return;
            DefineGladeAccessItem();
            
            var randoExists = ModHooks.GetMod("Randomizer 4") is Mod;
            if (!randoExists) return;
            MenuHolder.Hook();
            RequestMaker.Hook();
            LogicAdder.Hook();
            
            // Apparently this is a good place to add modules?
            RandoController.OnExportCompleted += OnRandoControllerOnOnExportCompleted; 
        }

        
        // Thanks, Rider. This name is *perfect*.
        private static void OnRandoControllerOnOnExportCompleted(RandoController rc)
        {
            if (GS.Enabled) ItemChangerMod.Modules.GetOrAdd<RemoveVanillaGladeAccess>();
            else ItemChangerMod.Modules.Remove<RemoveVanillaGladeAccess>();
        }
        private static void DefineGladeAccessItem()
        {
            BoolItem gladeAccessItem = new BoolItem() { 
                fieldName = nameof(PlayerData.gladeDoorOpened),
                name = Consts.GladeAccessItemName,
                UIDef = new MsgUIDef()
                {
                    sprite = new EmbeddedSprite("glade_door"),
                    shopDesc = new BoxedString(Consts.GladeAccessShopDesc),
                    name = new BoxedString(Consts.GladeAccessFriendlyName)
                },
            };
            var tag = gladeAccessItem.AddTag<InteropTag>();
            tag.Message = "RandoSupplementalMetadata";
            tag.Properties["ModSource"] = nameof(RandomizeGladeAccess);
            tag.Properties["PoolGroup"] = "Keys";
            
            Finder.DefineCustomItem(gladeAccessItem);
        }

        private static void AddGladeAccessSettings(LogArguments args, TextWriter tw)
        {
            tw.WriteLine($"RandomizeGladeAccess.Enabled: {GS.Enabled}");
            tw.WriteLine($"RandomizeGladeAccess.AddDupe: {GS.AddDupe}");
        }
    }
}