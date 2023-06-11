using System;
using System.Collections.Generic;
using System.Windows;
using LibrarySystem.Domain.Models;
using LibrarySystem.EntityFramework;
using LibrarySystem.Service;
using LibrarySystem.WPF.Navigation;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;


namespace LibrarySystem.WPF
{
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;
        private LibraryDBContextFactory _dbContextFactory => new LibraryDBContextFactory();

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            //singletons that are added to the services, these persist though out the applications lifetime
            services.AddSingleton<AccountStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<SearchStore>();
            services.AddSingleton<LibraryDBContextFactory>();


            services.AddSingleton(s => CreateLoginNavigationService(s));

            //below sets up the view modules for all user-controls  
            services.AddTransient(CreateNavigationBarViewModel);
            services.AddTransient(s =>
                new SearchBarViewModel(s.GetRequiredService<SearchStore>(), CreateSearchNavigationService(s)));
            services.AddTransient(s => new LoginViewModel(s.GetRequiredService<AccountStore>(),
                CreateAccountNavigationService(s), CreateRegisterNavigationService(s)));
            services.AddTransient(s =>
                new RegisterViewModel(CreateLoginNavigationService(s), s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new AccountViewModel(s.GetRequiredService<AccountStore>(),
                s.GetRequiredService<LibraryDBContextFactory>()));
            services.AddTransient(s =>
                new SearchViewModel(s.GetRequiredService<SearchStore>(), s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new CheckInBookViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new CheckOutBookViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient(s =>
                new AllUsersViewModel(s.GetRequiredService<AccountStore>(),
                    CreateEditAccountDetailsNavigationService(s)));
            services.AddTransient(s => new AddUserViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new EditUserViewModel(s.GetRequiredService<AccountStore>()));


            services.AddSingleton<MainViewModel>();
            services.AddSingleton(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (LibraryDbContext context = _dbContextFactory.CreateDbContext())
                context.Database.Migrate();

            var initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            //checks if there are any fines in the xml files and added them or updates exsisting fines.
            var fineService = new FineService();
            fineService.CheckFines();

            if (MessageBox.Show("GENERATE INITIAL DATA?", "TEST", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var _db = _dbContextFactory.CreateDbContext())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        _db.Books.AddRange(new List<Book>
                        {
                            new Book
                            {
                                Title = "To Kill a Mockingbird",
                                Author = "Harper Lee",
                                Isbn = "9780060935467",
                                Publisher = "Harper Perennial Modern Classics",
                                PublishDate = new DateTime(2005, 7, 5),
                                Description =
                                    "A classic American novel set in the 1930s, exploring racial injustice through the eyes of Scout Finch.",
                                Genre = "Fiction",
                                BookCost = 10.23m
                            },
                            new Book
                            {
                                Title = "Pride and Prejudice",
                                Author = "Jane Austen",
                                Isbn = "9780141439518",
                                Publisher = "Penguin Classics",
                                PublishDate = new DateTime(2002, 9, 24),
                                Description =
                                    "A beloved romantic novel following the story of Elizabeth Bennet and Mr. Darcy in 19th century England.",
                                Genre = "Fiction/Romance",
                                BookCost = 9.52m
                            },
                            new Book
                            {
                                Title = "1984",
                                Author = "George Orwell",
                                Isbn = "9780451524935",
                                Publisher = "Signet Classics",
                                PublishDate = new DateTime(1961, 8, 1),
                                Description =
                                    "A dystopian novel depicting a totalitarian society where individualism and independent thinking are suppressed.",
                                Genre = "Fiction/Dystopian",
                                BookCost = 8.58m
                            },
                            new Book
                            {
                                Title = "The Great Gatsby",
                                Author = "F. Scott Fitzgerald",
                                Isbn = "9780743273565",
                                Publisher = "Scribner",
                                PublishDate = new DateTime(2004, 9, 30),
                                Description =
                                    "A classic American novel set in the Roaring Twenties, exploring themes of wealth, love, and the American Dream.",
                                Genre = "Fiction",
                                BookCost = 12.25m
                            },
                            new Book
                            {
                                Title = "Moby-Dick",
                                Author = "Herman Melville",
                                Isbn = "9780142437247",
                                Publisher = "Penguin Classics",
                                PublishDate = new DateTime(2003, 5, 27),
                                Description =
                                    "An epic adventure of Captain Ahab's pursuit of the white whale, Moby-Dick, exploring themes of obsession and fate.",
                                Genre = "Fiction/Adventure",
                                BookCost = 11.97m
                            },
                            new Book
                            {
                                Title = "Harry Potter and the Sorcerer's Stone",
                                Author = "J.K. Rowling",
                                Isbn = "9780590353427",
                                Publisher = "Scholastic",
                                PublishDate = new DateTime(1998, 9, 1),
                                Description =
                                    "The first book in the popular Harry Potter series, introducing the young wizard's journey at Hogwarts School of Witchcraft and Wizardry.",
                                Genre = "Fiction/Fantasy",
                                BookCost = 15.73m
                            },
                            new Book
                            {
                                Title = "The Catcher in the Rye",
                                Author = "J.D. Salinger",
                                Isbn = "9780316769488",
                                Publisher = "Back Bay Books",
                                PublishDate = new DateTime(2001, 1, 30),
                                Description =
                                    "A coming-of-age novel narrated by Holden Caulfield, a disillusioned teenager who rebels against the hypocrisy of society.",
                                Genre = "Fiction",
                                BookCost = 10.26m
                            },
                            new Book
                            {
                                Title = "To the Lighthouse",
                                Author = "Virginia Woolf",
                                Isbn = "9780156907392",
                                Publisher = "Harcourt Brace Jovanovich",
                                PublishDate = new DateTime(1989, 10, 16),
                                Description =
                                    "A modernist novel that explores the lives and thoughts of the Ramsay family and their guests over the course of a decade.",
                                Genre = "Fiction/Modernist",
                                BookCost = 9.67m
                            },
                            new Book
                            {
                                Title = "Animal Farm",
                                Author = "George Orwell",
                                Isbn = "9780451526342",
                                Publisher = "Signet Classics",
                                PublishDate = new DateTime(1996, 1, 1),
                                Description =
                                    "An allegorical novella depicting a group of farm animals who rebel against their human farmer, representing the events of the Russian Revolution and the subsequent Soviet Union.",
                                Genre = "Fiction/Political Satire",
                                BookCost = 8.25m
                            },
                            new Book
                            {
                                Title = "The Hobbit",
                                Author = "J.R.R. Tolkien",
                                Isbn = "9780547928227",
                                Publisher = "Houghton Mifflin Harcourt",
                                PublishDate = new DateTime(2012, 9, 18),
                                Description =
                                    "A fantasy novel following the adventures of Bilbo Baggins as he embarks on a quest to reclaim the Lonely Mountain from the dragon Smaug.",
                                Genre = "Fiction/Fantasy",
                                BookCost = 13.94m
                            },
                            new Book
                            {
                                Title = "Lord of the Flies",
                                Author = "William Golding",
                                Isbn = "9780399501487",
                                Publisher = "Penguin Books",
                                PublishDate = new DateTime(2003, 12, 16),
                                Description =
                                    "A disturbing novel that explores the descent into savagery of a group of British boys stranded on an uninhabited island.",
                                Genre = "Fiction",
                                BookCost = 10.33m
                            },
                            new Book
                            {
                                Title = "Jane Eyre",
                                Author = "Charlotte Brontë",
                                Isbn = "9780142437209",
                                Publisher = "Penguin Classics",
                                PublishDate = new DateTime(2003, 8, 5),
                                Description =
                                    "A classic novel following the life of Jane Eyre, an orphaned governess, and her passionate romance with Mr. Rochester.",
                                Genre = "Fiction/Romance",
                                BookCost = 11.85m
                            },
                            new Book
                            {
                                Title = "The Lord of the Rings",
                                Author = "J.R.R. Tolkien",
                                Isbn = "9780544003415",
                                Publisher = "Mariner Books",
                                PublishDate = new DateTime(2012, 8, 14),
                                Description =
                                    "An epic high fantasy trilogy set in the world of Middle-earth, chronicling the journey to destroy the One Ring and defeat the Dark Lord Sauron.",
                                Genre = "Fiction/Fantasy",
                                BookCost = 18.46m
                            },
                            new Book
                            {
                                Title = "Brave New World",
                                Author = "Aldous Huxley",
                                Isbn = "9780060850524",
                                Publisher = "Harper Perennial Modern Classics",
                                PublishDate = new DateTime(2006, 10, 17),
                                Description =
                                    "A dystopian novel set in a future society where humans are genetically engineered and socially conditioned to maintain stability.",
                                Genre = "Fiction/Dystopian",
                                BookCost = 9.33m
                            },
                            new Book
                            {
                                Title = "The Odyssey",
                                Author = "Homer",
                                Isbn = "9780140268867",
                                Publisher = "Penguin Classics",
                                PublishDate = new DateTime(1999, 9, 1),
                                Description =
                                    "An epic poem attributed to Homer, recounting the adventures of Odysseus as he tries to return home after the Trojan War.",
                                Genre = "Poetry/Epic",
                                BookCost = 12.99m
                            },
                            new Book
                            {
                                Title = "The Alchemist",
                                Author = "Paulo Coelho",
                                Isbn = "9780062315007",
                                Publisher = "HarperOne",
                                PublishDate = new DateTime(2014, 4, 15),
                                Description =
                                    "A mystical novel following the journey of a young Andalusian shepherd named Santiago as he seeks his personal legend.",
                                Genre = "Fiction",
                                BookCost = 10.25m
                            },
                            new Book
                            {
                                Title = "The Kite Runner",
                                Author = "Khaled Hosseini",
                                Isbn = "9781594631931",
                                Publisher = "Riverhead Books",
                                PublishDate = new DateTime(2013, 3, 5),
                                Description =
                                    "A powerful novel set in Afghanistan, exploring themes of friendship, betrayal, and redemption against the backdrop of a changing society.",
                                Genre = "Fiction",
                                BookCost = 11.19m
                            },
                            new Book
                            {
                                Title = "The Picture of Dorian Gray",
                                Author = "Oscar Wilde",
                                Isbn = "9780141439570",
                                Publisher = "Penguin Classics",
                                PublishDate = new DateTime(2003, 6, 3),
                                Description =
                                    "A philosophical novel depicting the moral decay of Dorian Gray as his portrait ages while he remains eternally young.",
                                Genre = "Fiction/Gothic",
                                BookCost = 9.01m
                            },
                            new Book
                            {
                                Title = "The Divine Comedy",
                                Author = "Dante Alighieri",
                                Isbn = "9780142437223",
                                Publisher = "Penguin Classics",
                                PublishDate = new DateTime(2003, 9, 2),
                                Description =
                                    "An epic poem by Dante, describing his journey through Hell, Purgatory, and Heaven, and exploring theological and philosophical concepts.",
                                Genre = "Poetry/Epic",
                                BookCost = 14.09m
                            },
                            new Book
                            {
                                Title = "Sapiens: A Brief History of Humankind",
                                Author = "Yuval Noah Harari",
                                Isbn = "9780062316097",
                                Publisher = "Harper",
                                PublishDate = new DateTime(2015, 2, 10),
                                Description =
                                    "A non-fiction book that surveys the history of Homo sapiens, examining key milestones and developments that shaped human societies.",
                                Genre = "Non-fiction/History",
                                BookCost = 15.65m
                            },
                            new Book
                            {
                                Title = "The Immortal Life of Henrietta Lacks",
                                Author = "Rebecca Skloot",
                                Isbn = "9781400052189",
                                Publisher = "Broadway Books",
                                PublishDate = new DateTime(2011, 3, 8),
                                Description =
                                    "A non-fiction book that tells the story of Henrietta Lacks and her unwitting contribution to medical science.",
                                Genre = "Non-fiction/Biography",
                                BookCost = 10.68m
                            },
                            new Book
                            {
                                Title = "A Game of Thrones",
                                Author = "George R.R. Martin",
                                Isbn = "9780553593716",
                                Publisher = "Bantam",
                                PublishDate = new DateTime(2011, 3, 22),
                                Description =
                                    "The first book in the fantasy series A Song of Ice and Fire, depicting the power struggles among noble families in the Seven Kingdoms.",
                                Genre = "Fiction/Fantasy",
                                BookCost = 17.38m
                            },
                            new Book
                            {
                                Title = "The Hunger Games",
                                Author = "Suzanne Collins",
                                Isbn = "9780439023528",
                                Publisher = "Scholastic Press",
                                PublishDate = new DateTime(2010, 7, 3),
                                Description =
                                    "The first book in the dystopian trilogy, following Katniss Everdeen as she participates in the brutal Hunger Games.",
                                Genre = "Fiction/Dystopian",
                                BookCost = 12.34m
                            },
                            new Book
                            {
                                Title = "The Da Vinci Code",
                                Author = "Dan Brown",
                                Isbn = "9780307474278",
                                Publisher = "Anchor",
                                PublishDate = new DateTime(2009, 3, 31),
                                Description =
                                    "A thriller novel featuring symbologist Robert Langdon, who gets entangled in a complex web of codes and secrets.",
                                Genre = "Fiction/Thriller",
                                BookCost = 11.63m
                            },
                            new Book
                            {
                                Title = "The Chronicles of Narnia",
                                Author = "C.S. Lewis",
                                Isbn = "9780066238500",
                                Publisher = "HarperCollins",
                                PublishDate = new DateTime(2005, 8, 22),
                                Description =
                                    "A series of fantasy novels set in the magical world of Narnia, where children embark on extraordinary adventures.",
                                Genre = "Fiction/Fantasy",
                                BookCost = 16.87m
                            },
                            new Book
                            {
                                Title = "Fahrenheit 451",
                                Author = "Ray Bradbury",
                                Isbn = "9781451673319",
                                Publisher = "Simon & Schuster",
                                PublishDate = new DateTime(2012, 1, 10),
                                Description =
                                    "A dystopian novel depicting a future society where books are banned and burned, and critical thinking is suppressed.",
                                Genre = "Fiction/Dystopian",
                                BookCost = 9.39m
                            },
                            new Book
                            {
                                Title = "The Diary of a Young Girl",
                                Author = "Anne Frank",
                                Isbn = "9780553296983",
                                Publisher = "Bantam",
                                PublishDate = new DateTime(1993, 6, 1),
                                Description =
                                    "The diary of Anne Frank, a Jewish girl hiding from the Nazis during World War II, offering a poignant account of her life and experiences.",
                                Genre = "Non-fiction/Autobiography",
                                BookCost = 8.66m
                            },
                            new Book
                            {
                                Title = "The Hitchhiker's Guide to the Galaxy",
                                Author = "Douglas Adams",
                                Isbn = "9780345391803",
                                Publisher = "Del Rey",
                                PublishDate = new DateTime(1997, 9, 27),
                                Description =
                                    "A comedic science fiction series following the adventures of Arthur Dent after the destruction of Earth.",
                                Genre = "Fiction/Science Fiction",
                                BookCost = 10.55m
                            },
                            new Book
                            {
                                Title = "The Shining",
                                Author = "Stephen King",
                                Isbn = "9780345806789",
                                Publisher = "Anchor",
                                PublishDate = new DateTime(2012, 6, 26),
                                Description =
                                    "A psychological horror novel set in an isolated hotel, exploring the descent into madness of the caretaker and the supernatural forces at play.",
                                Genre = "Fiction/Horror",
                                BookCost = 13.55m
                            },
                            new Book
                            {
                                Title = "The Road",
                                Author = "Cormac McCarthy",
                                Isbn = "9780307387899",
                                Publisher = "Vintage",
                                PublishDate = new DateTime(2006, 3, 28),
                                Description =
                                    "A post-apocalyptic novel following a father and son as they journey through a desolate landscape, facing threats and maintaining their humanity.",
                                Genre = "Fiction/Dystopian",
                                BookCost = 11.55m
                            }
                        });
                    }
                    
                    _db.Users.AddRange(new List<User>
                    {
                        new User
                        {
                            LibraryCardNumber = Guid.NewGuid(),
                            Email = "Johnathon.Millard@gmail.com",
                            Name = "Johnathon Millard",
                            AccountType = AccountType.Librarian,
                            PhoneNumber = "07712547027"
                        },
                        new User
                        {
                            LibraryCardNumber = Guid.NewGuid(),
                            Email = "EmilyHancox@gmail.com",
                            Name = "Emily Hancox",
                            AccountType = AccountType.Member,
                            PhoneNumber = "13265478"
                        },
                        new User
                        {
                            LibraryCardNumber = Guid.NewGuid(),
                            Email = "JoeBob@gmail.com",
                            Name = "Joe Bob",
                            AccountType = AccountType.Member,
                            PhoneNumber = "987654321"
                        },
                        new User
                        {
                            LibraryCardNumber = Guid.NewGuid(),
                            Email = "JML",
                            Name = "Johnathon Librarian",
                            AccountType = AccountType.Librarian,
                            PhoneNumber = "987654321"
                        },
                        new User
                        {
                            LibraryCardNumber = Guid.NewGuid(),
                            Email = "JMM",
                            Name = "Johnathon Member",
                            AccountType = AccountType.Member,
                            PhoneNumber = "987654321"
                        }
                    });

                    _db.SaveChanges();
                }
            }

            if (MessageBox.Show("GENERATE INITIAL LOGS?", "TEST", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                var logService = new LogService();
                logService.InitialLogs();
            }

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            //open the application for the user. 
            MainWindow.Show();
            base.OnStartup(e);
        }

        //within this region are the navigation service builders that are not within the layout 

        #region Navigation

        private INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LoginViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LoginViewModel>());
        }

        private INavigationService CreateRegisterNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<RegisterViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<RegisterViewModel>());
        }

        #endregion

        //within this region are the navigation service builders that are within the layout 

        #region LayoutNavigation

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                serviceProvider.GetRequiredService<AccountStore>(),
                CreateLoginNavigationService(serviceProvider),
                CreateAccountNavigationService(serviceProvider),
                CreateCheckedOutBooksNavigationService(serviceProvider),
                CreateCheckedInBooksNavigationService(serviceProvider),
                CreateEditAccountDetailsNavigationService(serviceProvider),
                CreateViewAllUsersNavigationService(serviceProvider));
        }

        private INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<AccountViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<AccountViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateSearchNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<SearchViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SearchViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateCheckedOutBooksNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<CheckOutBookViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<CheckOutBookViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateCheckedInBooksNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<CheckInBookViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<CheckInBookViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateEditAccountDetailsNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<EditUserViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<EditUserViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateViewAllUsersNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<AllUsersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<AllUsersViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        #endregion
    }
}