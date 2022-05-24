using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Modules;
using UnityEngine;

namespace RandomizeGladeAccess
{
    public class RemoveVanillaGladeAccess : Module
    {
        public override void Initialize()
        {
            Events.AddFsmEdit(SceneNames.RestingGrounds_07, new FsmID("Dream Moth", "Conversation Control"), EditSeerFSM);
            Events.AddLanguageEdit(new("Dream Witch", "WITCH_REWARD_2B"), SetSeerDialogue);
        }


        public override void Unload()
        {
            Events.RemoveFsmEdit(SceneNames.RestingGrounds_07, new FsmID("Dream Moth", "Conversation Control"), EditSeerFSM);
            Events.RemoveLanguageEdit(new("Dream Witch", "WITCH_REWARD_2B"), SetSeerDialogue);
        }

        private static void EditSeerFSM(PlayMakerFSM fsm)
        {
            var getDoorOpen = fsm.GetState("Get Door Open");
            getDoorOpen.RemoveActionsOfType<SetPlayerDataBool>();
            
            var open = fsm.GetState("Open");
            open.GetFirstActionOfType<AudioPlayerOneShotSingle>().audioClip = 
                GameObject.Find("Knight").LocateMyFSM("Dream Nail").GetState("Set Fail")
                    .GetFirstActionOfType<AudioPlayerOneShotSingle>().audioClip;
            open.RemoveActionsOfType<PlayParticleEmitter>();
        }
        
        private static void SetSeerDialogue(ref string value)
        {
            value = PlayerData.instance.gladeDoorOpened ? Consts.SeerGladeAlreadyOpen : Consts.SeerGladeDoorFail;
        }
    }
}