using Front.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using TDRevision.BlazorApp.Services;
using TDRevision.Models;

namespace TDRevision.BlazorApp.ViewModels
{
    public class CommandeCreateViewModel : INotifyPropertyChanged
    {
        private readonly ICommandeService _commandeService;
        private string? _nomArticle;
        private string? _montant;
        private int _idUtilisateur;
        private bool _isSubmitting;
        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;

        public CommandeCreateViewModel(ICommandeService commandeService)
        {
            _commandeService = commandeService;
        }

        [Required(ErrorMessage = "Le nom de l'article est requis")]
        public string? NomArticle
        {
            get => _nomArticle;
            set
            {
                _nomArticle = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Le montant est requis")]
        public string? Montant
        {
            get => _montant;
            set
            {
                _montant = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "L'ID utilisateur est requis")]
        [Range(1, int.MaxValue, ErrorMessage = "L'ID utilisateur doit être supérieur à 0")]
        public int IdUtilisateur
        {
            get => _idUtilisateur;
            set
            {
                _idUtilisateur = value;
                OnPropertyChanged();
            }
        }

        public bool IsSubmitting
        {
            get => _isSubmitting;
            set
            {
                _isSubmitting = value;
                OnPropertyChanged();
            }
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                _successMessage = value;
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

        public async Task<bool> CreateCommandeAsync()
        {
            try
            {
                IsSubmitting = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                var commande = new Commande
                {
                    NomArticle = NomArticle,
                    Montant = Montant,
                    IdUtilisateur = IdUtilisateur
                };

                await _commandeService.CreateCommandeAsync(commande);
                SuccessMessage = "Commande créée avec succès !";
                ResetForm();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la création: {ex.Message}";
                return false;
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        public void ResetForm()
        {
            NomArticle = string.Empty;
            Montant = string.Empty;
            IdUtilisateur = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}