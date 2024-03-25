using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notes.ViewModels
{
    internal class NoteViewModel : ObservableObject, IQueryAttributable
    {
        private Models.Note _note;

        public string Text
        {
            get => _note.Text;
            set
            {
                if (_note.Text != value)
                {
                    _note.Text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Identifier => _note.Filename;

        public ObservableCollection<GroupMemberViewModel> Members { get; }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AddMemberCommand { get; private set; }
        public ICommand RemoveMemberCommand { get; private set; }

        public string NewMemberName { get; set; }

        public NoteViewModel()
        {
            _note = new Models.Note();
            Members = new ObservableCollection<GroupMemberViewModel>();
            InitializeCommands();
        }

        public NoteViewModel(Models.Note note)
        {
            _note = note;
            Members = new ObservableCollection<GroupMemberViewModel>(
                _note.Members.Select(member => new GroupMemberViewModel(member)));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
            AddMemberCommand = new RelayCommand(AddMember);
            RemoveMemberCommand = new RelayCommand(RemoveMember);
        }

        private void RemoveMember()
        {
            throw new NotImplementedException();
        }

        private async Task Save()
        {
            _note.Save();
            await Shell.Current.GoToAsync($"..?saved={_note.Filename}");
        }

        private async Task Delete()
        {
            _note.Delete();
            await Shell.Current.GoToAsync($"..?deleted={_note.Filename}");
        }

        public void AddMember()
        {
            if (!string.IsNullOrWhiteSpace(NewMemberName))
            {
                var newMember = new Models.GroupMember { Name = NewMemberName };
                Members.Add(new GroupMemberViewModel(newMember));
                NewMemberName = string.Empty; // Clear the NewMemberName after adding
            }
        }

        public void RemoveMember(object parameter)
        {
            if (parameter is GroupMemberViewModel memberToRemove)
            {
                Members.Remove(memberToRemove);
            }
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("load"))
            {
                _note = Models.Note.Load(query["load"].ToString());
                RefreshProperties();
            }
        }

        public void Reload()
        {
            _note = Models.Note.Load(_note.Filename);
            RefreshProperties();
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Text));
            // Refresh Members list if needed
        }
    }

    internal class GroupMemberViewModel : ObservableObject
    {
        private Models.GroupMember _member;

        public string Name
        {
            get => _member.Name;
            set
            {
                if (_member.Name != value)
                {
                    _member.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public GroupMemberViewModel(Models.GroupMember member)
        {
            _member = member;
        }
    }
}
