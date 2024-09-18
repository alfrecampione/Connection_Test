namespace API;
class Program
{
    static void Main()
    {
        string[] endpoints = ["v2/schedule", "v2/schedule_expanded"];
        while (true)
        {
            Console.WriteLine("Select one of the following endpoints by writing the number of the desired option:");
            for (int i = 0; i < endpoints.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {endpoints[i]}");
            }
            Console.Write("option: ");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                var connection = new Connection(apiUrl: "http://146.190.130.247:5011/donbest", token: "reeEQitM0rEsVOdhd7Ed", endpoints: endpoints);
                Console.WriteLine(connection.Get(option));
            }
            else
            {
                Console.WriteLine("Invalid option, please write the number of the desired option");
                Console.Clear();
            }
        }
    }
}