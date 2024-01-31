using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace AlwaysLeadSieges
{
    public class MySubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("AlwaysLeadSieges").PatchAll();
        }
    }
}
