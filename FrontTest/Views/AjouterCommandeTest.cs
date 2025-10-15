using Bunit;
using Bunit.TestDoubles;
using Front.Models;
using Front.Service;
using Front.Views;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FrontTest.Views
{
    public class AjouterCommandeTests : TestContext
    {
        [Fact]
        public void AjouterCommande_Should_Display_Form_With_All_Required_Fields()
        {
            // Arrange
            var fakeCommandeService = new FakeCommandeService();
            var fakeUtilisateurService = new FakeUtilisateurService();

            Services.AddSingleton<IService<Commande, int, string>>(fakeCommandeService);
            Services.AddSingleton<IService<Utilisateur, int, string>>(fakeUtilisateurService);

            // Act
            var cut = RenderComponent<AjouterCommande>();

            // Assert
            cut.WaitForElement("h4");

            Assert.Contains("Nouvelle Commande", cut.Markup);

            var nomArticleInput = cut.Find("input#nomArticle");
            Assert.NotNull(nomArticleInput);

            var montantInput = cut.Find("input#montant");
            Assert.NotNull(montantInput);

            var utilisateurSelect = cut.Find("select#utilisateur");
            Assert.NotNull(utilisateurSelect);

            var submitButton = cut.Find("button[type='submit']");
            Assert.NotNull(submitButton);
            Assert.Contains("Créer la commande", submitButton.TextContent);

            var retourButton = cut.Find("button[type='button']");
            Assert.NotNull(retourButton);
            Assert.Contains("Retour", retourButton.TextContent);
        }
    }

    public class FakeCommandeService : IService<Commande, int, string>
    {
        public Task AddAsync(Commande entity) => Task.CompletedTask;
        public Task DeleteAsync(int id) => Task.CompletedTask;
        public Task<IEnumerable<Commande>> GetAllAsync() => Task.FromResult(Enumerable.Empty<Commande>());
        public Task<Commande> GetByIdAsync(int id) => Task.FromResult(new Commande());
        public Task<Commande> GetByKeyAsync(string key) => Task.FromResult(new Commande());
        public Task UpdateAsync(int id, Commande entity) => Task.CompletedTask;
    }

    public class FakeUtilisateurService : IService<Utilisateur, int, string>
    {
        public Task AddAsync(Utilisateur entity) => Task.CompletedTask;
        public Task DeleteAsync(int id) => Task.CompletedTask;

        public Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur { IdUtilisateur = 1, Nom = "Dupont", Prenom = "Jean", Ville = "Paris" },
                new Utilisateur { IdUtilisateur = 2, Nom = "Martin", Prenom = "Marie", Ville = "Lyon" }
            };
            return Task.FromResult(utilisateurs.AsEnumerable());
        }

        public Task<Utilisateur> GetByIdAsync(int id) => Task.FromResult(new Utilisateur());
        public Task<Utilisateur> GetByKeyAsync(string key) => Task.FromResult(new Utilisateur());
        public Task UpdateAsync(int id, Utilisateur entity) => Task.CompletedTask;
    }
}