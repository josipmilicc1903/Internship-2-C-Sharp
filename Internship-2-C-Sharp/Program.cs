namespace Internship_2_C_Sharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            Dictionary<int, (string firstName, string latName, DateOnly birthDate,
                List<int> borrowList)> members = new Dictionary<int, (string firstName, string latName, DateOnly birthDate,
                List<int> borrows)>();

            Dictionary<int, (string bookTitle, string author, DateOnly borroDate,
                DateOnly? returnDate, int overdueDays, int lateFee)> borrowList = new Dictionary<int, (string bookTitle, string author, DateOnly borroDate,
                DateOnly? returnDate, int overdueDays, int lateFee)>();

            //members.Add(1, ("Josip", "Milic", DateOnly.FromDateTime(DateTime.Now), new List<int> { 1, 2, 3 }));
            //borrowList.Add(1, ("Zlocin", "Fjodor", DateOnly.FromDateTime(DateTime.Now), null, 0, 0));

        }
    }
}
