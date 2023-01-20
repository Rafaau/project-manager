namespace ProjectManager.Web.ApiModels;

public class AppointmentRequest
{
  public string Name { get; set; }
  public string Description { get; set; }
  public DateTime Date { get; set; }
  public UserSimplified[] Users { get; set; }
}

public class AppointmentComplex
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public DateTime Date { get; set; }
  public UserSimplified[] Users { get; set; }
}
