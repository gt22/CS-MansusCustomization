using BepInEx;
using GreatWork;
using MansusCustomisation.Patches;

namespace MansusCustomisation
{
    [BepInPlugin("com.frgm.mansuscustomisation", "Mansus customisation", "1.0.0.0")]
    [BepInDependency("greatwork")]
    public class MansusCustomisation : BaseUnityPlugin
    {
        private void Awake()
        {
            GreatWorkAPI.RegisterCurrentAssembly();
            MansusPatch.PatchAll();
        }
    }
}