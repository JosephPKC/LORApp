using System.Diagnostics;

namespace LORApp.Views.Listing;

public partial class CardListingPage : ContentPage
{
	public CardListingPage(CardListingViewModel pViewModel)
	{
		InitializeComponent();

		BindingContext = pViewModel;
		Trace.WriteLine("BOUND VIEW MODEL");
		Trace.WriteLine($"{pViewModel.CardListing.Count}");
	}
}