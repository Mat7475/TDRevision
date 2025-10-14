using Front.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TDRevision.BlazorApp.Services;
using Front.Models;
using TDRevision.Models.DTO;

namespace Front.BlazorApp.ViewModels
{
    public class CommandeListViewModel : INotifyPropertyChanged
    {
        private readonly ICommandeService _commandeService;
        private List<CommandeDTO> _commandes = new();
        private Commande? _selectedCommande;
        private bool _isLoading;
        private bool _showDetailModal;
        private string _errorMessage = string.Empty;

        public CommandeListViewModel(ICommandeService commandeService)
        {
            _commandeService = commandeService;
        }

        public List<CommandeDTO> Commandes
        {
            get => _commandes;
            set
            {
                _commandes = value;
                OnPropertyChanged();
            }
        }

        public Commande? SelectedCommande
        {
            get => _selectedCommande;
            set
            {
                _selectedCommande = value;
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

        public bool ShowDetailModal
        {
            get => _showDetailModal;
            set
            {
                _showDetailModal = value;
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
                Commandes = await _commandeService.GetAllCommandesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des commandes: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task ShowCommandeDetailAsync(int commandeId)
        {
            try
            {
                IsLoading = true;
                SelectedCommande = await _commandeService.GetCommandeByIdAsync(commandeId);
                ShowDetailModal = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement du détail: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void CloseDetailModal()
        {
            ShowDetailModal = false;
            SelectedCommande = null;
        }

        public async Task DeleteCommandeAsync(int commandeId)
        {
            try
            {
                IsLoading = true;
                await _commandeService.DeleteCommandeAsync(commandeId);
                await LoadCommandesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression: {ex.Message}";
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}