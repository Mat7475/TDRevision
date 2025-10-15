using Front.Models;
using Front.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Front.ViewModels
{
    public class CommandesViewModel : INotifyPropertyChanged
    {
        private readonly IService<Commande, int, string> _commandeService;
        private ObservableCollection<Commande> _commandes;
        private bool _isLoading;
        private string _errorMessage;

        public CommandesViewModel(IService<Commande, int, string> commandeService)
        {
            _commandeService = commandeService;
            _commandes = new ObservableCollection<Commande>();
        }

        public ObservableCollection<Commande> Commandes
        {
            get => _commandes;
            set
            {
                _commandes = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadCommandesAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var commandes = await _commandeService.GetAllAsync();
                Commandes = new ObservableCollection<Commande>(commandes);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des commandes : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task DeleteCommandeAsync(int id)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _commandeService.DeleteAsync(id);
                await LoadCommandesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}