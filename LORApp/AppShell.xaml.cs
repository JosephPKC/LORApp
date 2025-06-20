using LORApp.Views.Cards;

namespace LORApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute("ChampionCardPage", typeof(ChampionCardPage));
            //Routing.RegisterRoute("SpellCardPage", typeof(SpellCardPage));
            Routing.RegisterRoute("UnitCardPage", typeof(UnitCardPage));
        }
    }
}
