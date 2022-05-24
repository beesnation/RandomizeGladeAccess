using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using static RandomizerMod.Localization;

namespace RandomizeGladeAccess
{
    public class MenuHolder
    {
        internal MenuPage RandomizeGladeAccess;
        internal MenuElementFactory<GlobalSettings> rpMEF;
        internal VerticalItemPanel rpVIP;

        internal SmallButton JumpToRPButton;

        private static MenuHolder _instance = null;
        internal static MenuHolder Instance => _instance ??= new MenuHolder();

        public static void OnExitMenu()
        {
            _instance = null;
        }

        public static void Hook()
        {
            RandomizerMenuAPI.AddMenuPage(Instance.ConstructMenu, Instance.HandleButton);
            MenuChangerMod.OnExitMainMenu += OnExitMenu;
        }

        private bool HandleButton(MenuPage landingPage, out SmallButton button)
        {
            JumpToRPButton = new(landingPage, Localize("RandomizeGladeAccess"));
            JumpToRPButton.AddHideAndShowEvent(landingPage, RandomizeGladeAccess);
            button = JumpToRPButton;
            return true;
        }

        private void ConstructMenu(MenuPage landingPage)
        {
            RandomizeGladeAccess = new MenuPage(Localize("RandomizeGladeAccess"), landingPage);
            rpMEF = new(RandomizeGladeAccess, global::RandomizeGladeAccess.RandomizeGladeAccess.GS);
            rpVIP = new(RandomizeGladeAccess, new(0, 300), 50f, true, rpMEF.Elements);
            Localize(rpMEF);
        }
    }
}