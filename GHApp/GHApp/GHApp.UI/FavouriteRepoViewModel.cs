using GHApp.Contracts.Dto;

namespace GHApp.UI
{
    public class FavouriteRepoViewModel : ViewModelBase
    {
        private Repo _repo;

        public Repo Repo
        {
            get { return _repo; }
            set
            {
                _repo = value;
                OnPropertyChanged();
            }
        }

        private int _newCommitsCount;

        public int NewCommitsCount
        {
            get { return _newCommitsCount; }
            set
            {
                _newCommitsCount = value;
                OnPropertyChanged();
            }
        }
    }
}