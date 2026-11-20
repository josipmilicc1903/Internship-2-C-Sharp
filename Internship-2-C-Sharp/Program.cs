using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Internship_2_C_Sharp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int membersNextId = 4;
            int borrowListNextId = 7;




            Dictionary<int, (string firstName, string lastName, DateOnly birthDate,
                List<int> borrowList)> members = new Dictionary<int, (string firstName, string lastName, DateOnly birthDate,
                List<int> borrowList)>();

            Dictionary<int, (string bookTitle, string author, DateOnly borrowDate,
                DateOnly? returnDate, int overdueDays, double lateFee)> borrowList = new Dictionary<int, (string bookTitle, string author, DateOnly borrowDate,
                DateOnly? returnDate, int overdueDays, double lateFee)>();





            members.Add(1, ("Josip", "Milic", new DateOnly(2004, 3, 19), new List<int> { 1, 3 }));
            members.Add(2, ("Marko", "Matic", new DateOnly(2002, 10, 11), new List<int> { 2, 4 }));
            members.Add(3, ("Ivan", "Ivanovic", new DateOnly(2010, 1, 21), new List<int> { 5, 6 }));

            borrowList.Add(1, ("Crime and Punishment", "Fjodor Dostojevski", DateOnly.FromDateTime(DateTime.Today.AddDays(-21)), DateOnly.FromDateTime(DateTime.Today), 0, 0));
            borrowList.Add(2, ("The Metamorphosis", "Franz Kafka", DateOnly.FromDateTime(DateTime.Today.AddDays(-10)), null, 0, 0));
            borrowList.Add(3, ("1984", "George Orwell", DateOnly.FromDateTime(DateTime.Today.AddDays(-32)), null, 0, 0));
            borrowList.Add(4, ("The Stranger", "Albert Camus", DateOnly.FromDateTime(DateTime.Today.AddDays(-10)), DateOnly.FromDateTime(DateTime.Today), 0, 0));
            borrowList.Add(5, ("The Trial", "Franz Kafka", DateOnly.FromDateTime(DateTime.Today.AddDays(-100)), DateOnly.FromDateTime(DateTime.Today.AddDays(-3)), 76, 38));
            borrowList.Add(6, ("The Brothers Karamazov", "Fjodor Dostojevski", DateOnly.FromDateTime(DateTime.Today.AddDays(-22)), null, 0, 0));



            void UpdateBorrowData()
            {
                foreach (var (key, value) in borrowList)
                {
                    DateOnly calculationDate = value.returnDate != null
                        ? value.returnDate.Value
                        : DateOnly.FromDateTime(DateTime.Today);

                    int dayDifference = calculationDate.DayNumber - value.borrowDate.DayNumber;

                    int overdueDays = dayDifference > 21 ? dayDifference - 21 : 0;
                    double lateFee = overdueDays * 0.5;

                    var tuple = (value.bookTitle, value.author, value.borrowDate,
                                 value.returnDate, overdueDays, lateFee);

                    borrowList[key] = tuple;
                }
            }

            bool IsVerified(string action)
            {
                Console.Write($"\nJeste li sigurni da želite {action} korisnika (Unesite Da): ");
                string verification = Console.ReadLine();
                if (verification == "Da" || verification == "da")
                {
                    return true;
                }
                return false;
            }

            bool IsVerifiedBorrow(string action)
            {
                Console.Write($"\nJeste li sigurni da želite {action} posudbu (Unesite Da): ");
                string verification = Console.ReadLine();
                if (verification == "Da" || verification == "da")
                {
                    return true;
                }
                return false;
            }

            bool HasNotReturned(int idMember)
            {
                foreach (var key in members[idMember].borrowList)
                {
                    if (borrowList[key].returnDate == null)
                    {
                        return true;
                    }
                }
                return false;

            }

            int ReturnUserId(string inputFirstName, string inputLastName)
            {
                int selectedId = 0;

                foreach (var (key, value) in members)
                {
                    if (value.firstName == inputFirstName && value.lastName == inputLastName)
                    {
                        selectedId = key;
                    }
                }
                return selectedId;
            }

            void PrintMemberList(List<(int id, string firstName, string lastName, int borrowCount)> memberList)
            {
                Console.WriteLine("\n");
                foreach (var member in memberList)
                {
                    Console.WriteLine($"Id: {member.id}, Prezime: {member.lastName}, Ime: {member.firstName}, Broj posudbi: {member.borrowCount}");
                }
                Console.WriteLine("\n");

            }

            (string inputFirstName, string inputLastName, string inputBirthDate) InputMemberData()
            {
                Console.Write("Unesi ime: ");
                string firstNameInput = Console.ReadLine();

                Console.Write("\nUnesi prezime: ");
                string lastNameInput = Console.ReadLine();

                Console.Write("\nUnesi datum rođenja: ");
                string dateInput = Console.ReadLine();

                return (firstNameInput, lastNameInput, dateInput);
            }


            string PrintTemplate(string template)
            {
                Console.WriteLine(template);
                Console.Write("Odabir: ");
                string input = Console.ReadLine();
                Console.WriteLine("");

                return input;

            }

            void PrintBorrowList(List<(int id, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)> borrows)
            {
                foreach (var borrow in borrows)
                {
                    PrintBorrow(borrow.id, borrow.bookTitle, borrow.author, borrow.borrowDate, borrow.returnDate, borrow.overdueDays, borrow.lateFee);
                }
            }

            List<(int id, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)> CreateBorrowTuple() {
                List<(int id, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)> borrows =
                    new List<(int id, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)>();

                foreach (var (key, value) in borrowList)
                {
                    var tuple = (key, value.bookTitle, value.author, value.borrowDate, value.returnDate, value.overdueDays, value.lateFee);
                    borrows.Add(tuple);

                }
                return borrows;
            }


            List<(int id, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)> SortedTupleByDate()
            {
                var borrows = CreateBorrowTuple();

                Console.Write("\nUlazno (odaberi 1) ili Silazno (odaberi drugi znak) sortiranje: ");
                string inputSort = Console.ReadLine();
                if (inputSort == "1")
                {
                    borrows.Sort((a, b) => a.borrowDate.CompareTo(b.borrowDate));
                    return borrows;

                }
                borrows.Sort((a, b) => b.borrowDate.CompareTo(a.borrowDate));
                return borrows;
            }


            List<(int id, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)> SortedTupleByOverdue()
            {
                var borrows = CreateBorrowTuple();

                Console.Write("\nUlazno (odaberi 1) ili Silazno (odaberi drugi znak) sortiranje: ");
                string inputSort = Console.ReadLine();
                if (inputSort == "1")
                {
                    borrows.Sort((a, b) => a.lateFee.CompareTo(b.lateFee));
                    return borrows;

                }
                borrows.Sort((a, b) => b.lateFee.CompareTo(a.lateFee));
                return borrows;
            }


            List<String> ReturnBorrowMonths()
            {
                List<string> borrowMonths = new List<string>();

                foreach (var (key, value) in borrowList)
                {
                    int flag = 0;
                    foreach (var date in borrowMonths)
                    {
                        if (value.borrowDate.ToString("yyyy-MM") == date)
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        borrowMonths.Add(value.borrowDate.ToString("yyyy-MM"));
                    }
                }
                return borrowMonths;
            }

            void PrintBorrow(int id, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)
            {
                string strReturnDate = returnDate?.ToString() ?? "Nije vraceno";
                Console.WriteLine($"{id} |  \"{bookTitle}\" - {author} | Posuđeno : {borrowDate} | " +
                    $"Vračeno: {strReturnDate} | Kašnjenje : {overdueDays} | Zakasnina : {lateFee} E");

            }

            (bool isTrue, int selectedId) DoesUserExistById(string input)
            {
                int selectedId = 0;
                if (int.TryParse(input, out selectedId)) { }
                else
                {
                    Console.WriteLine("Kriv unos, trebaš unijeti broj");
                    Console.WriteLine("\n");
                    return (false, -1);
                }

                if (!members.ContainsKey(selectedId))
                {
                    Console.WriteLine("Krivi unos, nepostoji id koji si unio");
                    return (false, -1);
                }
                return (true, selectedId);
            }

            (bool isTrue, int selectedId) DoesBorrowExistById(string input)
            {
                int selectedId = 0;
                if (int.TryParse(input, out selectedId)) { }
                else
                {
                    Console.WriteLine("Kriv unos, trebaš unijeti broj");
                    Console.WriteLine("\n");
                    return (false, -1);
                }

                if (!borrowList.ContainsKey(selectedId))
                {
                    Console.WriteLine("Krivi unos, nepostoji id koji si unio");
                    return (false, -1);
                }
                return (true, selectedId);
            }

            void RemoveBorrowIdFromMember(int borrowId)
            {
                foreach (var (key, value) in members)
                {
                    if (value.borrowList.Contains(borrowId))
                    {
                        value.borrowList.Remove(borrowId);
                    }
                }
            }

            (bool isTrue,double lateFee) ReturnLateFee()
            {
                Console.Write("Unesi unos zakasnine za izbrisat posudbe ili Q za izac: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    Console.WriteLine("\nBrisanje posudbe prekinuto");
                    return (false,0);
                }

                double lateFee;
                if (double.TryParse(input, out lateFee))
                {
                    if (lateFee >= 0)
                    {
                        return (true, lateFee);
                    }
                    Console.WriteLine("\nKriv unos, zabranjeno je unosit negativne brojeve");
                    return (false, 0);
                }
                Console.WriteLine("\nKriv unos, potrebno je unijeti broj");
                return (false, 0);
            }

            (bool hasContinue, string input) ReturnInputId(string action)
            {
                Console.Write($"\nUnesi Id posudbe koju želiš {action} ili Q za izac: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    Console.WriteLine("\nBrisanje posudbe prekinuto");
                    return (false, "");
                }
                return (true, input);
            }

            bool IsValidDate(DateOnly date,DateOnly? borrowDate = null)
            {
                if (date > DateOnly.FromDateTime(DateTime.Today))
                {
                    Console.WriteLine("\nGreska, datum nemoze bit u buducnosti");
                    return false;


                }
                if (borrowDate != null)
                {
                    if (date < borrowDate)
                    {
                        if (date < borrowDate)
                        {
                            Console.WriteLine("\nGreska, datum vracanja nemore biti prije datuma posudbe");
                            return false;
                        }
                    }
                }
                return true;
            }

            void EditBorrow(int selectedId, string bookTitle, string author, DateOnly borrowDate, DateOnly? returnDate, int overdueDays, double lateFee)
            {
                var borrowTuple = (bookTitle, author, borrowDate, returnDate, overdueDays, lateFee);
                borrowList[selectedId] = borrowTuple;
            }



            (int bookSum,int unreturnedSum) CalculateUserTotalBorrows(int id)
            {
                int bookSum = members[id].borrowList.Count;
                int unreturnedSum = 0;
                foreach(var borrow in members[id].borrowList)
                {
                    if (borrowList[borrow].returnDate == null)
                    {
                        unreturnedSum++;
                    }
                }
                return (bookSum,unreturnedSum);
            }

            double CalculateUserTotalLateFee(int id)
            {
                double sum=0;
                foreach(var borrow in members[id].borrowList)
                {
                    sum = sum + borrowList[borrow].lateFee;
                }
                return sum;
            }

            double CalculateAverageBorrowLenght(int id)
            {
                if (members[id].borrowList.Count == 0)
                {
                    return 0;
                }
                double sumDayReturned = 0;
                double returnedBooks = 0;
                foreach (var borrow in members[id].borrowList)
                {
                    if (borrowList[borrow].returnDate != null)
                    {
                        sumDayReturned = sumDayReturned + (borrowList[borrow].returnDate.Value.DayNumber - borrowList[borrow].borrowDate.DayNumber);
                        returnedBooks++;
                    }
                }
                return returnedBooks != 0 ? sumDayReturned / returnedBooks : 0;
               
            }


            int CalculateBiggestOverdue(int id)
            {
                if (members[id].borrowList.Count == 0)
                {
                    return 0;
                }
                var biggest = members[id].borrowList.MaxBy(x => borrowList[x].overdueDays);
                return borrowList[biggest].overdueDays;
            }


            void PrintBorrowsByDateRange(int id)
            {
                Console.WriteLine("\nUnesi raspon datuma za povijest posudbi ili Q za izac: ");
                string inputDate1 = Console.ReadLine();
                if(inputDate1.ToLower() == "q")
                {
                    return;
                }
                string inputDate2 = Console.ReadLine();
                Console.WriteLine("\n");

                if (DateOnly.TryParse(inputDate1, out DateOnly date1))
                {
                    if (DateOnly.TryParse(inputDate2, out DateOnly date2))
                    {
                        if (date1 > date2)
                        {
                            Console.WriteLine("Neispravan raspon datuma");
                            return;
                        }

                        foreach (var borrow in members[id].borrowList)
                        {
                            if (borrowList[borrow].borrowDate.DayNumber >= date1.DayNumber &&
                                borrowList[borrow].borrowDate.DayNumber <= date2.DayNumber)
                            {
                                Console.WriteLine($"Posuđena knjiga Id-a {borrow} datum posudbe: {borrowList[borrow].borrowDate}");
                            }

                            if (borrowList[borrow].returnDate != null)
                            {
                                if (borrowList[borrow].returnDate.Value.DayNumber >= date1.DayNumber &&
                                    borrowList[borrow].returnDate.Value.DayNumber <= date2.DayNumber)
                                {
                                    Console.WriteLine($"Vračena knjiga Id-a {borrow} datum povrata: {borrowList[borrow].returnDate}");
                                }
                            }
                        }
                        return;
                    }
                }

                Console.WriteLine("Kriv format datuma");
            }

































            void AddNewMember()
            {
                var creationData = InputMemberData();


                if (creationData.inputFirstName.Length == 0 || creationData.inputLastName.Length == 0 || creationData.inputBirthDate.Length == 0) {
                    Console.WriteLine("\nKriv unos, nesmijes ostaviti prazna polja pri popunjavanju");
                    return;
                }

                DateOnly birthDate;
                if (DateOnly.TryParse(creationData.inputBirthDate, out birthDate)) { }
                else
                {
                    Console.WriteLine("\nGreska, kriv format datuma");
                    return;
                }

                if (IsValidDate(birthDate))
                {
                    if (IsVerified("kreirati"))
                    {
                        members.Add(membersNextId, (creationData.inputFirstName.Trim(), creationData.inputLastName.Trim(), birthDate, new List<int>()));
                        membersNextId++;
                        Console.WriteLine("\nKorisnik uspješno kreiran");
                        return;
                     
                    }
                    Console.WriteLine("\nKreiranje zaustavljeno");
                  
                }
            }


            void ListAllMembersSorted()
            {
                List<(int id, string firstName, string lastName, int borrowCount)> memberList = new List<(int id, string firstName, string lastName, int borrowCount)>();
                foreach (var (key, value) in members)
                {
                    var temporaryTouple = (key, value.firstName, value.lastName, value.borrowList.Count);
                    memberList.Add(temporaryTouple);
                }
                memberList.Sort((a, b) => string.Compare(a.lastName, b.lastName));

                PrintMemberList(memberList);

            }


            void ListMembersUnder18()
            {
                List<(int id, string firstName, string lastName, int borrowCount)> memberList = new List<(int id, string firstName, string lastName, int borrowCount)>();
                foreach (var (key, value) in members)
                {
                    int daysFromBirth = DateOnly.FromDateTime(DateTime.Today).DayNumber - value.birthDate.DayNumber;
                    int age = daysFromBirth / 365;
                    if (age <= 18)
                    {
                        var temporaryTouple = (key, value.firstName, value.lastName, value.borrowList.Count);
                        memberList.Add(temporaryTouple);
                    }
                }
                PrintMemberList(memberList);
            }


            void ListMembersWithNotReturned()
            {
                List<(int id, string firstName, string lastName, int borrowCount)> memberList = new List<(int id, string firstName, string lastName, int borrowCount)>();
                foreach (var (key, value) in members)
                {
                    foreach (var borrowedBook in value.borrowList)
                    {
                        if (borrowList[borrowedBook].returnDate == null)
                        {
                            var temporaryTouple = (key, value.firstName, value.lastName, value.borrowList.Count);
                            memberList.Add(temporaryTouple);
                            break;
                        }
                    }
                }

                PrintMemberList(memberList);
            }




            void OverviewMembers()
            {
                while (true)
                {
                    string template = """


                        PREGLED ČLANOVA

                        1 - Ispis članova abecedno po prezimenu

                        2 - Ispis članova mlađih od 18 godina

                        3 - Ispis svih članova koji trenutno imaju barem jednu knjigu nevraćenu

                        0 - Povratak na Members izbornik

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            ListAllMembersSorted();
                            break;

                        case ("2"):
                            ListMembersUnder18();
                            break;

                        case ("3"):
                            ListMembersWithNotReturned();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;

                    }
                }
            }






            void DeleteMemberById()
            {
                Console.Write("\nUnesi Id korisnika kojeg želis izbrisati ili Q za izać: ");
                string input = Console.ReadLine();

                if (input == "Q")
                {
                    return;
                }

                (bool isTrue, int selectedId) = DoesUserExistById(input);

                if (isTrue)
                {
                    if (HasNotReturned(selectedId))
                    {
                        Console.WriteLine("\nNemozes izbrisat korisnika koji ima nevracenu knjigu");
                        return;
                    }

                    Console.WriteLine($"\n{members[selectedId].firstName} {members[selectedId].lastName}");
                    if (IsVerified("obrisat"))
                    {
                        members.Remove(selectedId);
                        Console.WriteLine("\nKorisnik uspjesno izbrisan");
                        return;
                    }
                    Console.WriteLine("\nBrisanje zaustavljeno");
                }
            }

            void DeleteMemberByName()
            {
                Console.WriteLine("\n");

                Console.Write("Unesi Ime korisnika kojeg želiš izbrisat ili Q za izac: ");
                string inputFirstName = Console.ReadLine();

                if (inputFirstName == "Q" || inputFirstName == "q")
                {
                    return;
                }

                Console.Write("\nUnesi Prezime korisnika kojeg želiš izbrisat ili Q za izac: ");
                string inputLastName = Console.ReadLine();

                if (inputLastName == "Q" || inputLastName == "q")
                {
                    return;
                }

                int selectedId = ReturnUserId(inputFirstName, inputLastName);

                if (selectedId != 0)
                {
                    if (HasNotReturned(selectedId)) {
                        Console.WriteLine("\nNemozes izbrisat korisnika koji ima nevracenu knjigu");
                        return;
                    }

                    if (IsVerified("izbrisat"))
                    {
                        members.Remove(selectedId);
                        Console.WriteLine("Korisnik uspjesno izbrisan");
                        return;
                    }
                    Console.WriteLine("Brisanje zaustavljeno");
                    return;

                }
                Console.WriteLine("\nNeuspjesno brisanje, Korisnik kojeg ste unijeli ne postoji");

            }






            void DeleteMember()
            {
                while (true)
                {
                    string template = """



                        BRISANJE ČLANA

                        1 - Brisanje po  Id-u

                        2 - Brisanje po Imenu i Prezimenu
                         
                        0 - Povratak na Members izbornik
                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            DeleteMemberById();
                            break;

                        case ("2"):
                            DeleteMemberByName();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }

                }
            }








            void EditmemberFirstName(int id)
            {
                Console.Write("Promjeni ime: ");
                string inputFirstName = Console.ReadLine();

                if (inputFirstName.Length == 0)
                {
                    Console.WriteLine("Kriv unos, Nemozes unijet praznu rijec");
                    return;
                }

                if (IsVerified("uredit"))
                {
                    members[id] = (inputFirstName.Trim(), members[id].lastName, members[id].birthDate, members[id].borrowList);
                    Console.WriteLine("\nUređivanje uspjesno");
                    return;
                }
                Console.WriteLine("\nUredivanje prekinuto");

            }


            void EditmemberLastName(int id)
            {
                Console.Write("Promjeni prezime: ");
                string inputLastName = Console.ReadLine();

                if (inputLastName.Length == 0)
                {
                    Console.WriteLine("\nKriv unos, Nemozes unijet praznu rijec");
                    return;
                }

                if (IsVerified("uredit"))
                {
                    members[id] = (members[id].firstName, inputLastName.Trim(), members[id].birthDate, members[id].borrowList);
                    Console.WriteLine("\nUređivanje uspjesno");
                    return;
                }
                Console.WriteLine("\nUredivanje prekinuto");

            }

            void EditmemberBirthDate(int id)
            {
                Console.Write("Promjeni datum rođenja: ");
                string inputBirthDate = Console.ReadLine();

                DateOnly birthDate;
                if (!DateOnly.TryParse(inputBirthDate, out birthDate))
                {
                    Console.WriteLine("\nKriv unos datuma");
                    return;
                }

                if (IsValidDate(birthDate)) {
                    if (IsVerified("uredit"))
                    {
                        members[id] = (members[id].firstName, members[id].lastName, birthDate, members[id].borrowList);
                        Console.WriteLine("\nUređivanje uspjesno");
                        return;
                    }
                    Console.WriteLine("\nUredivanje prekinuto");
                }
            }




            void EditMember(int id)
            {
                while (true)
                {
                    string template = $"""


                            Uređivanje korisnika: {members[id].firstName} {members[id].lastName}

                            1 - Uredi ime

                            2 - Uredi prezime

                            3 - Uredi datum rođenja

                            0 - Povratak na Edit Menu

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            EditmemberFirstName(id);
                            break;

                        case ("2"):
                            EditmemberLastName(id);
                            break;

                        case ("3"):
                            EditmemberBirthDate(id);
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;

                    }
                }

            }

            void EditMemberById()
            {
                Console.Write("\nUnesi Id korisnika kojeg zelis uredit: ");
                string inputId = Console.ReadLine();

                (bool isTrue, int selectedId) = DoesUserExistById(inputId);
                if (isTrue)
                {
                    EditMember(selectedId);
                }
            }



            void PrintMembersMenu()
            {
                while (true)
                {
                    string template = """

                        MEMBER MENU

                        1 - Unos novog člana

                        2 - Brisanje člana

                        3 - Uređivanje člana

                        4 - Pregled svih članova

                        0 - Povratak na glavni izbornik
                         
                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            AddNewMember();
                            break;

                        case ("2"):
                            DeleteMember();
                            break;

                        case ("3"):
                            EditMemberById();
                            break;

                        case ("4"):
                            OverviewMembers();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;

                    }
                    Console.WriteLine("\n\n\n");
                }
            }


            void PrintAllBorrows()
            {
                var borrows = CreateBorrowTuple();
                PrintBorrowList(borrows);
            }

            void PrintSortedByDateBorrows()
            {
                var borrows = SortedTupleByDate();
                PrintBorrowList(borrows);
            }

            void PrintSortedByFeeBorrows()
            {
                var borrows = SortedTupleByOverdue();
                PrintBorrowList(borrows);
            }

            void PrintUnreturnedBorrows()
            {
                var borrows = CreateBorrowTuple();
                borrows.RemoveAll(borrow => borrow.returnDate != null);
                PrintBorrowList(borrows);
            }

            void PrintBorrowsByTitleOrAuthor()
            {
                var borrows = CreateBorrowTuple();

                Console.Write("Unesi ime autora ili naslov knjige za filtraciju posudbi: ");
                string input = Console.ReadLine();

                borrows.RemoveAll(borrow => borrow.bookTitle.ToLower() != input.Trim().ToLower() && borrow.author.ToLower() != input.Trim().ToLower());

                PrintBorrowList(borrows);
            }

            void PrintBorrowsByDate()
            {
                var borrowMonths = ReturnBorrowMonths();
                borrowMonths.Sort();
                foreach (var month in borrowMonths)
                {
                    Console.WriteLine($"{month}\n");
                    int borrowCount = 0;
                    double lateFeeSum = 0;
                    foreach (var (key, value) in borrowList)
                    {
                        if (value.borrowDate.ToString("yyyy-MM") == month)
                        {
                            borrowCount++;
                            lateFeeSum = lateFeeSum + value.lateFee;
                            PrintBorrow(key, value.bookTitle, value.author, value.borrowDate, value.returnDate, value.overdueDays, value.lateFee);
                        }
                    }
                    Console.WriteLine($"Ukupno posudbi: {borrowCount} ");
                    Console.WriteLine($"Ukupna zakasnina: {lateFeeSum}\n\n");

                }
            }




            void ListBorrowMenu()
            {
                while (true)
                {
                    string template = """

                        LIST BORROWS

                        1 - Sve posudbe

                        2 - Posudbe Sortirane Po Datumu

                        3 - Posudbe sortirane po zakasnini

                        4 - Nevracene posudbe

                        5 - Pretraga po naslovu ili autoru

                        6 - Grupirane posudbe

                        0 - Povratak na Borrow Menu

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            PrintAllBorrows();
                            break;

                        case ("2"):
                            PrintSortedByDateBorrows();
                            break;

                        case ("3"):
                            PrintSortedByFeeBorrows();
                            break;

                        case ("4"):
                            PrintUnreturnedBorrows();
                            break;

                        case ("5"):
                            PrintBorrowsByTitleOrAuthor();
                            break;

                        case ("6"):
                            PrintBorrowsByDate();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n\n\n");

                }
            }


            void CreateNewBorrow()
            {
                Console.Write("Unesi Id korisnika koji posuđuje: ");
                string input = Console.ReadLine();

                (bool isTrue, int selectedId) = DoesUserExistById(input);
                if (isTrue)
                {
                    Console.WriteLine($"\n{members[selectedId].firstName} {members[selectedId].lastName}\n");
                    Console.Write("Unesi naslov knjige ili Q za izac: ");
                    string inputBookTitle = Console.ReadLine();

                    if (inputBookTitle.ToLower() == "q")
                    {
                        Console.WriteLine("\nKreiranje nove posudbe zaustavljeno");
                        return;
                    }

                    Console.Write("Unesi autora knjige ili Q za izac: ");
                    string inputBookAuthor = Console.ReadLine();

                    if (inputBookAuthor.ToLower() == "q")
                    {
                        Console.WriteLine("\nKreiranjenove posudbe zaustavljeno");
                        return;
                    }

                    if (inputBookTitle.Length != 0 && inputBookAuthor.Length != 0)
                    {
                        if (IsVerifiedBorrow("kreirat"))
                        {
                            borrowList.Add(borrowListNextId, (inputBookTitle.Trim(), inputBookAuthor.Trim(), DateOnly.FromDateTime(DateTime.Today), null, 0, 0));
                            members[selectedId].borrowList.Add(borrowListNextId);
                            borrowListNextId++;

                            Console.WriteLine($"\nPosudba uspjesno kreirana, treba biti vracena do: {DateOnly.FromDateTime(DateTime.Today.AddDays(21))}\n");
                            return;
                        }
                        Console.WriteLine("\nPosudba zaustavljena");
                        return;

                    }
                    Console.WriteLine("Kriv unos, nemogu polja bit prazna");
                }
            }


            void DeleteById()
            {
                Console.Write("Unesi Id posudbe koju želiš izbrisat ili Q za izac: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    Console.WriteLine("Brisanje posudbe prekinuto");
                    return;
                }

                (bool isTrue, int selectedId) = DoesBorrowExistById(input);
                if (isTrue)
                {
                    Console.WriteLine($"\n{borrowList[selectedId].bookTitle}\n");
                    if (IsVerifiedBorrow("izbrisat")) {

                        RemoveBorrowIdFromMember(selectedId);
                        borrowList.Remove(selectedId);

                        Console.WriteLine("Brisanje posudbe uspjesno obavljeno");
                        return;
                    }
                    Console.WriteLine("Brisanje posudbe prekinuto");
                    return;
                }
            }

            void DeleteReturnedBorrows()
            {
                Console.Write("Unesi Id korisnika kojem želiš izbrisat vracene ili Q za izac: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    Console.WriteLine("Brisanje posudbe prekinuto");
                    return;
                }

                (bool isTrue, int selectedId) = DoesUserExistById(input);
                if (isTrue)
                {
                    Console.WriteLine($"\n{members[selectedId].firstName} {members[selectedId].lastName}\n");

                    if (IsVerifiedBorrow("izbrisat"))//malo uredit ovu funkciju prevelika je
                    {
                        int flag = 0;
                        for (int i = members[selectedId].borrowList.Count - 1; i >= 0; i--)
                        {
                            int borrowId = members[selectedId].borrowList[i];

                            if (borrowList[borrowId].returnDate != null)
                            {
                                members[selectedId].borrowList.RemoveAt(i);
                                borrowList.Remove(borrowId);
                                flag = 1;
                            }
                        }
                        string izlaz = flag == 0 ? "\nNema se šta za obrisat\n" : "\nBrisanje uspjesno obavljeno\n";
                        Console.WriteLine(izlaz);
                        return;
                    }
                    Console.WriteLine("Brisanje zaustavljeno");
                }

            }

            void DeleteBorrowsByFee()
            {
                (bool isTrue, double lateFee) = ReturnLateFee();
                if (isTrue)
                {
                    if (IsVerifiedBorrow("izbrisat"))
                    {
                        int flag = 0;
                        foreach (var (key, value) in members)
                        {
                            for (int i = value.borrowList.Count - 1; i >= 0; i--)
                            {
                                if (borrowList[value.borrowList[i]].lateFee > lateFee)
                                {
                                    borrowList.Remove(value.borrowList[i]);
                                    value.borrowList.RemoveAt(i);
                                    flag++;
                                }
                            }
                        }
                        string izlaz = flag == 0 ? "\nNema se šta za obrisat\n" : "\nBrisanje uspjesno obavljeno\n";
                        Console.WriteLine(izlaz);
                        return;
                    }
                    Console.WriteLine("Brisanje zaustavljeno");
                }
            }


            void DeleteBorrow()
            {
                while (true)
                {
                    string template = """

                        DELETE BORROW

                        1 - Izbrisi po Id-u

                        2 - Izbrisi sve vraćene posudbe odabranog člana

                        3 - Izbrisi sve posudbe sa zakasninom od unesenog iznosa

                        0 - Natrag na Borrow menu

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            DeleteById();
                            break;

                        case ("2"):
                            DeleteReturnedBorrows();
                            break;

                        case ("3"):
                            DeleteBorrowsByFee();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n\n\n");
                }
            }

            void ReturnBorrow()
            {
                (bool hasContinue, string input) = ReturnInputId("vratit");
                if (hasContinue)
                {
                    (bool isTrue, int selectedId) = DoesBorrowExistById(input);
                    if (isTrue)
                    {
                        if (borrowList[selectedId].returnDate == null)
                        {
                            Console.WriteLine($"\n {borrowList[selectedId].bookTitle}\n");
                            Console.Write("Unesi datum vracanja: ");
                            string inputDate = Console.ReadLine();

                            DateOnly returnDate;
                            if (DateOnly.TryParse(inputDate, out returnDate))
                            {
                                if (IsValidDate(returnDate, borrowList[selectedId].borrowDate))
                                {
                                    EditBorrow(selectedId, borrowList[selectedId].bookTitle, borrowList[selectedId].author,
                                        borrowList[selectedId].borrowDate, returnDate, borrowList[selectedId].overdueDays, borrowList[selectedId].lateFee);
                                    
                                }
                                return;
                            }
                            Console.WriteLine("\nGreska, kriv format datuma");
                            return;
                        }
                        Console.WriteLine("Greska, knjiga je vec vracena");
                    }
                }
            }







            void EditAuthor(int id)
            {
                Console.Write("Promjeni ime autora: ");
                string inputAuthor=Console.ReadLine();
                
                if(inputAuthor.Length != 0)
                {
                    if (IsVerifiedBorrow("uredit")) 
                    {
                        var tuple = (borrowList[id].bookTitle, inputAuthor, borrowList[id].borrowDate, borrowList[id].returnDate,
                            borrowList[id].overdueDays, borrowList[id].lateFee);
                        borrowList[id] = tuple;
                        Console.WriteLine("\nUređivanje uspjesno\n");
                        return;
                    }
                    Console.WriteLine("\nUređivanje prekinuto");
                    return;
                }
                Console.WriteLine("\nKriv unos, nesmije biti prazan strign");
            }


            void EditTitle(int id)
            {
                Console.Write("Promjeni ime naslova: ");
                string inputAuthor = Console.ReadLine();

                if (inputAuthor.Length != 0)
                {
                    if (IsVerifiedBorrow("uredit"))
                    {
                        var tuple = (inputAuthor, borrowList[id].author, borrowList[id].borrowDate, borrowList[id].returnDate,
                            borrowList[id].overdueDays, borrowList[id].lateFee);
                        borrowList[id] = tuple;
                        Console.WriteLine("\nUređivanje uspjesno\n");
                        return;
                    }
                    Console.WriteLine("\nUređivanje prekinuto");
                    return;
                }
                Console.WriteLine("\nKriv unos, nesmije biti prazan strign");
            }



            void EditBorrowDate(int id)
            {
                Console.Write("Promjeni datum posudbe: ");
                string inputDate = Console.ReadLine();

                if(DateOnly.TryParse(inputDate,out DateOnly borrowDate)){
                    if (IsValidDate(borrowDate)) {
                        if (borrowList[id].returnDate != null)
                        {
                            if(borrowDate > borrowList[id].returnDate)
                            {
                                Console.WriteLine("\nGreska, nemoze biti datum posudbe noviji od datuma vracanja");
                                return;
                            }
                        }
                        if (IsVerifiedBorrow("uredit"))
                        {
                            var tuple = (borrowList[id].bookTitle, borrowList[id].author, borrowDate, borrowList[id].returnDate,
                                borrowList[id].overdueDays, borrowList[id].lateFee);
                            borrowList[id] = tuple;
                            Console.WriteLine("\nUređivanje uspjesno\n");
                            return;

                        }
                        Console.WriteLine("\nUređivanje prekinuto\n");
                        return;
                    }
                    return;
                }
                Console.WriteLine("\nKriv format datuma\n");
            }




            void EditReturnDate(int id)
            {
                Console.Write("Promjeni datum vracanja ili upisi (prazno) ako zelis maknut datum vracanja: ");
                string inputDate = Console.ReadLine();

                if (DateOnly.TryParse(inputDate, out DateOnly returnDate))
                {
                    if (IsValidDate(returnDate, borrowList[id].borrowDate))
                    {
                        if (IsVerifiedBorrow("uredit"))
                        {
                            var tuple = (borrowList[id].bookTitle, borrowList[id].author, borrowList[id].borrowDate, returnDate,
                                borrowList[id].overdueDays, borrowList[id].lateFee);
                            borrowList[id] = tuple;
                            Console.WriteLine("\nUređivanje uspjesno\n");
                            return;

                        }
                        Console.WriteLine("\nUređivanje prekinuto\n");
                        return;
                    }
                    return;
                }
                else
                {
                    if(inputDate.ToLower() == "prazno")
                    {
                        if (IsVerified("uredit"))
                        {
                            DateOnly? date = null;
                            var tuple = (borrowList[id].bookTitle, borrowList[id].author, borrowList[id].borrowDate, date,
                                    borrowList[id].overdueDays, borrowList[id].lateFee);
                            borrowList[id] = tuple;
                            Console.WriteLine("\nUređivanje uspjesno\n");
                            return;
                        }
                        Console.WriteLine("\nUređivanje prekinuto\n");
                        return;
                    }
                }
                Console.WriteLine("\nKriv format datuma\n");
            }






            void EditBorrowById()
            {
                (bool hasContinue, string inputId) = ReturnInputId("uredit");
                if (hasContinue)
                {
                    (bool isTrue, int selectedId) = DoesBorrowExistById(inputId);
                    if (isTrue)
                    {
                        EditBorrowMenu(selectedId);
                    }
                }
            }



            void EditBorrowMenu(int id)
            {
                while (true)
                {
                    string template = $"""

                        Uredivanje posudbe: {borrowList[id].bookTitle}   {borrowList[id].author} 

                        1 - Uredi naslov

                        2 - Uredi autora

                        3 - Uredi datum posudbe

                        4 - Uredi datum vracanja

                        0 - Povratak na Borrow Menu

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            EditTitle(id);
                            break;

                        case ("2"):
                            EditAuthor(id);
                            break;

                        case ("3"):
                            EditBorrowDate(id);
                            UpdateBorrowData();
                            break;

                        case ("4"):
                            EditReturnDate(id);
                            UpdateBorrowData();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n\n\n");
                } 

            }


            void PrintReportMenu(int id)
            {
                while (true)
                {
                    string template = $"""

                        REPORT MENU: {members[id].firstName}  {members[id].lastName}

                        1 - Ukupan Broj posudbi i broj trenutno nevracenih knjiga

                        2 - Ukupan broj zakasnina

                        3 - Prosjecno trajanje posudbe

                        4 - Posudba s najvecim kasnjenjem

                        5 - Pregled posudbi u rasponu od - do

                        0 - Povratak na glavni izbornik

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            (int bookkSum, int unreturnedSum) = CalculateUserTotalBorrows(id);
                            Console.WriteLine($"Ukupan broj posudbi je: {bookkSum}, broj nevracenih knjiga je: {unreturnedSum}");
                            break;

                        case ("2"):
                            Console.WriteLine($"Ukupan zbroj zakasnina je: {CalculateUserTotalLateFee(id)}");
                            break;

                        case ("3"):
                            Console.WriteLine($"Prosjenco trajanje posudbe po danima je: {CalculateAverageBorrowLenght(id)}");
                            break;

                        case ("4"):
                            Console.WriteLine($"Posudba s najvecim kasnjenjem je: {CalculateBiggestOverdue(id)}");
                            break;

                        case ("5"):
                            PrintBorrowsByDateRange(id);
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n\n\n");
                }

            }

            void FindMemberId()
            {
                Console.Write("Unesi Id korisnika za kojeg želiš izvještaj: ");
                string inputId = Console.ReadLine();

                (bool isTrue, int selectedId) = DoesUserExistById(inputId);
                if (isTrue) {
                    PrintReportMenu(selectedId);
                }
            }



            void PrintBorrowMenu()
            {
                while (true)
                {
                    string template = """

                        BORROW MENU

                        1 - Unos nove posudbe

                        2 - Vraćanje knjige

                        3 - Brisanje posudbe

                        4 - Uređivanje postoječe posudbe

                        5 - Pregled svih posudbi

                        6 - Izvještaji i analize

                        0 - Povratak na glavni izbornik

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            CreateNewBorrow();
                            break;

                        case ("2"):
                            ReturnBorrow();
                            UpdateBorrowData();

                            break;

                        case ("3"):
                            DeleteBorrow();
                            break;

                        case ("4"):
                            EditBorrowById();
                            break;

                        case ("5"):
                            ListBorrowMenu();
                            break;

                        case ("6"):
                            FindMemberId();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n\n\n");
                }

            }



            void FindBiggestFeeMember()
            {
                int biggestFeeId = 1;
                double biggestFee = -1;
                foreach( var (key,value) in members)
                {
                    double tempTotalFee = CalculateUserTotalLateFee(key);
                    if (tempTotalFee > biggestFee)
                    {
                        biggestFeeId = key;
                        biggestFee = tempTotalFee;
                    }
                }
                if(members.Count == 0)
                {
                    Console.WriteLine("Prazna lista članova, nemoze se izracunat");
                    return;
                }
                Console.WriteLine($"\nČlan sa največom zakasninom je: {members[biggestFeeId].firstName} {members[biggestFeeId].lastName}");
                Console.WriteLine($"Zakasnina je {biggestFee}");
            }

            void FindMostBorrowsMember()
            {
                int mostBorrowsId = 1;
                double  mostBorrows= 0;
                int flag = 0;
                foreach (var (key, value) in members)
                {
                    double tempborrws = value.borrowList.Count;
                    if (tempborrws > mostBorrows)
                    {
                        mostBorrowsId = key;
                        mostBorrows = tempborrws;
                        flag = 1;
                    }
                }
                if (members.Count == 0)
                {
                    Console.WriteLine("Prazna lista članova, nemoze se izracunat\n");
                    return;
                }

                if (flag == 0)
                {
                    Console.WriteLine("Nijedan ćlan  nema posudbu");
                    return;
                }
                Console.WriteLine($"\nČlan sa najvise posudbi je: {members[mostBorrowsId].firstName} {members[mostBorrowsId].lastName}");
                Console.WriteLine($"Broj posudbi je {mostBorrows}");
            }

            void PrintMostBorrowedBook()
            {
                if(borrowList.Count == 0)
                {
                    Console.WriteLine("Lista posudbi je prazna");
                    return;
                }

                int mostBorrowedCount = -1;
                string mostBorrowedTitle = "";
                foreach(var (key,value) in borrowList)
                {
                    int tempBorrowedCount = 0;
                    foreach(var (key1,value1) in borrowList)
                    {
                        if(value.bookTitle == value1.bookTitle)
                        {
                            tempBorrowedCount++;
                        }
                    }
                    if(tempBorrowedCount > mostBorrowedCount)
                    {
                        mostBorrowedCount = tempBorrowedCount;
                        mostBorrowedTitle = value.bookTitle;
                    }
                }
                Console.WriteLine($"\nNajposuđivanija knjiga je: {mostBorrowedTitle}, broj posudbi: {mostBorrowedCount}");
            }


            void CalculateTotalLateFee()
            {
                double sum = 0;
                foreach(var (key,value) in members)
                {
                    sum = sum + CalculateUserTotalLateFee(key);
                }

                Console.WriteLine($"\nUkupan iznos zakasnina svih članova je: {sum}\n");
            }




            void PrintStatisticMenu()
            {
                while (true)
                {
                    string template = """

                        STATISTIC MENU


                        1 - Član s najvećom ukupnom zakasninom

                        2 - Član s najviše posudbi

                        3 - Najposuđivaniji naslov

                        4 - Ukupan iznos zakasnina svih članova

                        5 - Graf broja posudbi po mjesecu

                        0 - Povratak na glavni izbornik
                        
                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            FindBiggestFeeMember();
                            break;

                        case ("2"):
                            FindMostBorrowsMember();
                            break;

                        case ("3"):
                            PrintMostBorrowedBook();
                            break;

                        case ("4"):
                            CalculateTotalLateFee();
                            break;

                        case ("5"):

                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n\n\n");


                }
            }




            void PrintMainMenu()
            {
                UpdateBorrowData();
                while (true)
                {
                     string template = """
                        APLIKACIJA ZA EVIDENCIJU POSUDBI

                        1 - Članovi

                        2 - Posudbe

                        3 - Statistika

                        0 - Izlaz iz aplikacije

                        """;

                    string input = PrintTemplate(template);

                    switch (input)
                    {
                        case ("1"):
                            PrintMembersMenu();
                            break;

                        case ("2"):
                            PrintBorrowMenu();
                            break;

                        case ("3"):
                            PrintStatisticMenu();
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n\n\n");
                }
            }

            PrintMainMenu();
        }
    }
}
