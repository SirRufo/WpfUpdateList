using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using WpfUpdateList.Common.Models;

namespace WpfUpdateList.ViewModels
{
    public class DetailsViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> recordIsValid;

        public Details Record { get; } = new();
        public ObservableCollection<Details> Records { get; } = new();
        [Reactive] public Details? SelectedRecord { get; set; }
        public bool RecordIsValid => recordIsValid.Value;
        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        public DetailsViewModel()
        {
            // copy selected record content to edit record
            this.WhenAnyValue(e => e.SelectedRecord)
                .WhereNotNull()
                .Subscribe(e => Record.CopyFrom(e));

            recordIsValid = Record
                .WhenAny(
                    e => e.Name,
                    e => e.Company,
                    (name, company) => (Name: name.Value, Company: company.Value))
                .Select(e => !string.IsNullOrEmpty(e.Name) && !string.IsNullOrEmpty(e.Company))
                .ToProperty(this, e => e.RecordIsValid);

            var canSubmit = this.WhenAnyValue(e => e.RecordIsValid);
            var canUpdate = this.WhenAny(e => e.RecordIsValid, e => e.SelectedRecord, (valid, record) => valid.Value && record.Value is not null);
            var canRemove = this.WhenAny(e => e.SelectedRecord, e => e.Value is not null);

            SubmitCommand = ReactiveCommand.Create(DoSubmit, canSubmit);
            UpdateCommand = ReactiveCommand.Create(DoUpdate, canUpdate);
            RemoveCommand = ReactiveCommand.Create(DoRemove, canRemove);
        }

        private void DoSubmit()
        {
            var temp = new Details();
            Record.CopyTo(temp);
            Records.Add(temp);
            SelectedRecord = temp;
        }

        private void DoUpdate() => Record.CopyTo(SelectedRecord!);

        private void DoRemove()
        {
            var selectedIndex = Records.IndexOf(SelectedRecord!);
            Records.Remove(SelectedRecord!);
            if (Records.Any())
            {
                if (Records.Count == selectedIndex)
                    SelectedRecord = Records.Last();
                else
                    SelectedRecord = Records[selectedIndex];
            }
        }
    }
}
