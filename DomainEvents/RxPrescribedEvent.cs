namespace DomainEvents
{
	public class RxPrescribedEvent
	{
        public Patient patient { get; set; }
        public Medication medication { get; set; }
    }

	public class Patient
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

	public class Medication
    {
        public string drugName { get; set; }
    }
}
