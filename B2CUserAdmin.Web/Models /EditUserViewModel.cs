using Microsoft.Graph.Models;

namespace B2CUserAdmin.Models;

public class EditUserViewModel
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string GivenName { get; set; }
    public string Surname { get; set; }
    public bool? AccountEnabled { get; set; }
    public string Mail { get; set; }
    public string MobilePhone { get; set; }
    public string FaxNumber { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string Department { get; set; }
    public string EmployeeId { get; set; }
    public string EmployeeType { get; set; }
    public string OfficeLocation { get; set; }
    public List<IdentityUserFlowAttribute> CustomAttributes { get; set; }
    public Dictionary<string, string> AdditionalData { get; set; }

}
