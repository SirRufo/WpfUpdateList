namespace WpfUpdateList.Common.Models
{
    public class Details : ReactiveObject
    {
        [Reactive] public string Name { get; set; } = string.Empty;
        [Reactive] public string Company { get; set; } = string.Empty;
        [Reactive] public string Region { get; set; } = string.Empty;
        [Reactive] public string HRA { get; set; } = string.Empty;
        [Reactive] public string Basic { get; set; } = string.Empty;
        [Reactive] public string PF { get; set; } = string.Empty;
        [Reactive] public string Salary { get; set; } = string.Empty;

        public void CopyTo(Details other)
        {
            other.Name = Name;
            other.Company = Company;
            other.Region = Region;
            other.HRA = HRA;
            other.Basic = Basic;
            other.PF = PF;
            other.Salary = Salary;
        }

        public void CopyFrom(Details? other)
        {
            other ??= new Details();
            other.CopyTo(this);
        }
    }
}
