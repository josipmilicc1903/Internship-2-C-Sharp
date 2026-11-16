namespace Internship_2_C_Sharp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int membersNextid = 4;
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

            borrowList.Add(1, ("Crime and Punishment", "Fjodor Dostojevski", DateOnly.FromDateTime(DateTime.Today.AddDays(-20)), null, 0, 0));
            borrowList.Add(2, ("The Metamorphosis", "Franz Kafka", DateOnly.FromDateTime(DateTime.Today.AddDays(-10)), null, 0, 0));
            borrowList.Add(3, ("1984", "George Orwell", DateOnly.FromDateTime(DateTime.Today.AddDays(-100)), null, 0, 0));
            borrowList.Add(4, ("The Stranger", "Albert Camus", DateOnly.FromDateTime(DateTime.Today.AddDays(-10)), DateOnly.FromDateTime(DateTime.Today), 0, 0));
            borrowList.Add(5, ("The Trial", "Franz Kafka", DateOnly.FromDateTime(DateTime.Today.AddDays(-100)), DateOnly.FromDateTime(DateTime.Today), 0, 0));
            borrowList.Add(6, ("The Brothers Karamazov", "Fjodor Dostojevski", DateOnly.FromDateTime(DateTime.Today.AddDays(-100)), null, 0, 0));

            foreach(var (key,value) in members)
            {
                foreach(var book in value.borrowList)
                {
                    Console.WriteLine(borrowList[book].bookTitle);
                }
            }

        }
    }
}
