using System.Text.Json;

namespace ProjectOpgaveListe
{
    class Job
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }
    }
    class Program
    {
        static string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "MySavedData.json"
        );

        static List<Job> jobs;
        static List<Job> activeJobs;

        static Program()
        {
            jobs = LoadJobs();
            activeJobs = jobs.Where(j => j.IsCompleted == false).ToList();
        }
        static void Main(string[] args)
        {
            bool running = true;
            do
            {
                Console.WriteLine("Hello, please choose what you'd like to do: ");
                Console.WriteLine("[X] Close program");
                Console.WriteLine("[1] Add To-Do job");
                Console.WriteLine("[2] View Jobs");
                Console.WriteLine("[3] Complete jobs");

                string choice = Console.ReadLine()!.ToUpper();

                switch (choice) {
                    case "X":
                        running = false;
                        break;
                    case "1":
                        AddJob();
                        break;
                    case "2":
                        ViewJobs();
                        break;
                    case "3":
                        CompleteJob();
                        break;
                    default: break;
                }

            } while (running);
        }
        public static void AddJob()
        {
            if (activeJobs.Count < 5)
            {
                Console.Clear();
                Job job = new Job();
                job.Id = jobs.Count + 1;
                job.CreatedDate = DateTime.Now;
                Console.Write("Add job name: ");
                job.Name = Console.ReadLine();
                job.IsCompleted = false;
                Console.Clear();
                Console.Write("Thank you, now please describe the job: ");
                job.Description = Console.ReadLine();
                jobs.Add(job);
                activeJobs.Add(job);
                SaveToFile(jobs);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("There may be no more than 5 jobs awaiting at once, please wait for other jobs to be completed before adding more. ");
            }
            

        }
        public static void ViewJobs()
        {
            Console.Clear();
            if (activeJobs.Count == 0)
            {
                Console.WriteLine("No jobs found.");
                Console.ReadKey();
                return;
            }
            foreach (Job job in activeJobs)
            {
                Console.WriteLine($"Id: {job.Id}");
                Console.WriteLine($"Name: {job.Name}");
                Console.WriteLine($"Description: {job.Description}");
                Console.WriteLine($"Created: {job.CreatedDate}");
                Console.WriteLine("----------------------------------------------------");
            }
            Console.ReadLine();
            Console.Clear();
        }
        public static void CompleteJob()
        {
            Console.Clear();
            Console.WriteLine("Please enter id of the Job you would like to mark as completed: ");
            var input = Console.ReadLine();
            var id = int.Parse(input!);
            Job? job = jobs.Where(j => j.Id == id).FirstOrDefault();
            if (job != null && job.IsCompleted != true)
            {
                Console.WriteLine("Please confirm job for completion: ");
                Console.WriteLine();
                Console.WriteLine($"Id: {job.Id}");
                Console.WriteLine($"Name: {job.Name}");
                Console.WriteLine($"Description: {job.Description}");
                Console.WriteLine($"Created: {job.CreatedDate}");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine();
                bool confirmed = false;
                Console.WriteLine("Is this the job you want to mark as completed? (y/n)");
                input = Console.ReadLine()!.ToUpper();
                if (input == "Y")
                {
                    confirmed = true;
                }
                else
                {
                    return;
                }

                job.IsCompleted = true;
                activeJobs.Remove(job);
                SaveToFile(jobs);
                Console.WriteLine("Job is now marked as completed: ");
                Console.ReadLine();
            }
            else if (job == null)
            {
                Console.WriteLine($"No job found with id {id}. ");
                Console.WriteLine("Hej");
            }
            else if (job != null && job.IsCompleted == true)
            {
                Console.WriteLine("");
            }
                Console.Clear();
        }
        public static void SaveToFile(List<Job> newJobs)
        {
            var json = JsonSerializer.Serialize(newJobs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static List<Job> LoadJobs()
        {
            if (File.Exists(filePath))
            {
                var existingJson = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Job>>(existingJson) ?? new List<Job>();
            }
            else
            {
                return new List<Job>();
            }
        }
    }
}
