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
                List<int> borrows)>();

            Dictionary<int, (string bookTitle, string author, DateOnly borrowDate,
                DateOnly? returnDate, int overdueDays, double lateFee)> borrowList = new Dictionary<int, (string bookTitle, string author, DateOnly borrowDate,
                DateOnly? returnDate, int overdueDays, double lateFee)>();





            members.Add(1, ("Josip", "Milic", new DateOnly(2004,3,19), new List<int> { 1, 3 }));
            members.Add(2, ("Marko", "Matic", new DateOnly(2002, 10, 11), new List<int> { 2, 4 }));
            members.Add(3, ("Ivan", "Ivanovic", new DateOnly(2010, 1, 21), new List<int> { 5, 6 }));

            borrowList.Add(1, ("Crime and Punishment", "Fjodor Dostojevski", DateOnly.FromDateTime(DateTime.Today.AddDays(-21)), DateOnly.FromDateTime(DateTime.Today), 0, 0));
            borrowList.Add(2, ("The Metamorphosis", "Franz Kafka", DateOnly.FromDateTime(DateTime.Today.AddDays(-10)), null, 0, 0));
            borrowList.Add(3, ("1984", "George Orwell", DateOnly.FromDateTime(DateTime.Today.AddDays(-32)), null, 0, 0));
            borrowList.Add(4, ("The Stranger", "Albert Camus", DateOnly.FromDateTime(DateTime.Today.AddDays(-10)), DateOnly.FromDateTime(DateTime.Today), 0, 0));
            borrowList.Add(5, ("The Trial", "Franz Kafka", DateOnly.FromDateTime(DateTime.Today.AddDays(-100)), DateOnly.FromDateTime(DateTime.Today.AddDays(-3)), 76, 38));
            borrowList.Add(6, ("The Brothers Karamazov", "Fjodor Dostojevski", DateOnly.FromDateTime(DateTime.Today.AddDays(-22)), null, 0, 0));


            void UpdateDataForOverdueBorrows()
            {
                foreach(var (key,value) in borrowList)
                {
                    DateOnly todayDate = DateOnly.FromDateTime(DateTime.Today);
                    if(value.returnDate == null)
                    {
                        int dayDifference = todayDate.DayNumber - value.borrowDate.DayNumber;
                        if(dayDifference > 21)
                        {
                            int overdueDays = dayDifference - 21;
                            double lateFee = overdueDays * 0.5;
                            var touple = (value.bookTitle, value.author, value.borrowDate, value.returnDate, overdueDays, lateFee);
                            borrowList[key] = touple;

                        }
                    }
                }
            }











            void AddNewMember()
            {
                Console.Write("Unesi ime: ");
                string firstNameInput = Console.ReadLine();
                Console.WriteLine("\n");

                Console.Write("Unesi prezime: ");
                string lastNameInput = Console.ReadLine();
                Console.WriteLine("\n");

                Console.Write("Unesi datum rođenja: ");
                string dateInput = Console.ReadLine();
                Console.WriteLine("\n");

                if(firstNameInput.Length == 0|| lastNameInput.Length == 0|| dateInput.Length == 0){
                    Console.WriteLine("Kriv unos, nesmijes ostaviti prazna polja pri popunjavanju");
                    return;
                }
                DateOnly birthDate;
                if (DateOnly.TryParse(dateInput, out birthDate)) { }
                else
                {
                    Console.WriteLine("Greska, kriv format datuma");
                    return;
                }

                members.Add(membersNextId, (firstNameInput, lastNameInput, birthDate, new List<int>()));
                membersNextId++;
                Console.WriteLine("Korisnik uspješno kreiran");
            }



            void ListAllMembersSorted()
            {
                List<(int id,string firstName, string lastName, int borrowCount)> memberList = new List<(int id,string firstName, string lastName, int borrowCount)>();
                foreach(var (key,value) in members)
                {
                    var temporaryTouple=(key,value.firstName, value.lastName, value.borrowList.Count);
                    memberList.Add(temporaryTouple);
                }
                memberList.Sort((a, b) => string.Compare(a.lastName, b.lastName));

                Console.WriteLine("\n");
                foreach(var member in memberList)
                {
                    Console.WriteLine($"Id: {member.id}, Prezime: {member.lastName}, Ime: {member.firstName}, Broj posudbi: {member.borrowCount}");
                }
                Console.WriteLine("\n");

            }



            void OverviewMembers()
            {
                while (true)
                {
                    string template = """

                        PREGLED ČLANOVA

                        1 - Ispis članova abecedno po prezimenu

                        2 - Ispis članova mlađih od 18 godina

                        3 - Ispis svih članova koji trenutno imaju barem jednu knjigu

                        """;

                    Console.WriteLine(template);
                    Console.Write("Odabir: ");
                    string input = Console.ReadLine();
                    Console.WriteLine("");

                    switch (input)
                    {
                        case ("1"):
                            ListAllMembersSorted();
                            break;

                        case ("2"):
                            break;

                        case ("3"):
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;

                    }
                }
            }

            



            void PrintMembersMenu()
            {
                while (true)
                {
                    string template = """

                        1 - Unos novog člana

                        2 - Brisanje člana

                        3 - Uređivanje člana

                        4 - Pregled svih članova

                        0 - Povratak na glavni izbornik
                         
                        """;

                    Console.WriteLine(template);
                    Console.Write("Odabir: ");
                    string input = Console.ReadLine();
                    Console.WriteLine("");

                    switch (input)
                    {
                        case ("1"):
                            AddNewMember();
                            break;

                        case ("2"):
                            break;

                        case ("3"):
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
                    Console.WriteLine("\n");
                    Console.WriteLine("\n");
                    Console.WriteLine("\n");
                }
            }



            void PrintMainMenu()
            {
                UpdateDataForOverdueBorrows();
                while (true)
                {
                    string template = """
                        APLIKACIJA ZA EVIDENCIJU POSUDBI

                        1 - Članovi

                        2 - Posudbe

                        0 - Izlaz iz aplikacije

                        """;

                    Console.WriteLine(template);
                    Console.Write("Odabir: ");
                    string input = Console.ReadLine();
                    Console.WriteLine("");

                    switch (input)
                    {
                        case ("1"):
                            PrintMembersMenu();
                            break;

                        case ("2"):
                            break;

                        case ("0"):
                            return;

                        default:
                            Console.WriteLine("Unesen krivi znak");
                            break;
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("\n");
                    Console.WriteLine("\n");
                }
            }

            PrintMainMenu();
        }
    }
}
