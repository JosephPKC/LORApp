using LORApp.Views.Cards;

namespace LORApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("UnitCardPage", typeof(UnitCardPage));
        }
    }
}
