namespace Main.ViewModel
{
    public class LayoutViewModel : BaceViewModel
    {
        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, SearchBarViewModel searchBarViewModel,
            BaceViewModel contentViewModel)
        {
            ContentViewModel = contentViewModel;
            SearchBarViewModel = searchBarViewModel;
            NavigationBarViewModel = navigationBarViewModel;
        }

        public NavigationBarViewModel NavigationBarViewModel { get; }
        public BaceViewModel ContentViewModel { get; }
        public SearchBarViewModel SearchBarViewModel { get; }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            SearchBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}