namespace DatabaseCreation.Models
{
    public class Worker : IUserOwned
    {
        public int Id { get; set; }
        public string Worker_Name { get; set; }
        public WorkerType Worker_Type { get; set; }
        public string Address { get; set; }
        public DateOnly Hire_Date { get; set; }
        public DateOnly Last_Update { get; set; }
        public List<Phone> Phones { get; set; } = new List<Phone>();
        public List<AdvanceAndDeduction> AdvanceAndDeductions { get; set; } = new List<AdvanceAndDeduction>();
        public string UserId { get; set; }
    }
    public enum WorkerType
    {
        Stitcher,
        Cutter,
        Ironer,
        Girls,

    }
}

