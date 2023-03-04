using Main.Servies;
using Main.Stores;

namespace Main.Commands
{
    public class CheckOutBookCommand : CommandBace
    {
        private readonly AccountStore _accountStore;

        private readonly BookService _bs;

        public CheckOutBookCommand(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _bs = new BookService(_accountStore);
        }

        
        public override void Execute(object isbn)
        {
            _bs.CheckOutBook((string)isbn);
        }
    }
}