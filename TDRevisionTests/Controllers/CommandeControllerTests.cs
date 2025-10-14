using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TDRevision.Controllers;
using TDRevision.Mapper;
using TDRevision.Models;
using TDRevision.Models.DataManager;
using TDRevision.Models.EntityFramework;
using TDRevision.Models.Repository;

namespace TDRevisionTests.Controllers.CommandeTests
{
    [TestClass]
    [TestCategory("integration")]
    public class CommandesControllerTests
    {
        private CommandeController _controller;
        private AppDbContext _context;
        private CommandeManager _manager;
        private IMapper _mapper;
        private Commande _commandecommun;

        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappeurProfile>();
            });
            _mapper = config.CreateMapper();

            _manager = new CommandeManager(_context);
            _controller = new CommandeController(_manager, _mapper);

            _context.Commandes.RemoveRange();
            await _context.SaveChangesAsync();


            var utilisateur = new Utilisateur
            {
                Nom = "TestNom",
                Prenom = "TestPrenom",
                CodePostal = 73000,
                Ville = "Chambéry"
            };
            await _context.Utilisateurs.AddAsync(utilisateur);
            await _context.SaveChangesAsync();

            _commandecommun = new Commande()
            {
                NomArticle = "ArticleCommun",
                IdUtilisateur = utilisateur.IdUtilisateur, 
                Montant = "100"
            };
            await _manager.AddAsync(_commandecommun);
        }
        [TestMethod]
        public async Task GetCommandeTest()
        {
            var result = await _controller.GetCommande(_commandecommun.IdCommande);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(_commandecommun.NomArticle, result.Value.NomArticle);
        }

        [TestMethod]
        public async Task NotFoundGetCommandeTest()
        {
            var result = await _controller.GetCommande(0);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetCommandesTest()
        {
            var result = await _controller.GetCommandes();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value.Any());
            Assert.IsTrue(result.Value.Any(m => m.NomArticle == _commandecommun.NomArticle));
        }

        [TestMethod]
        public async Task GetCommandeByStringTest()
        {
            var result = await _controller.GetCommandeByString(_commandecommun.NomArticle);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(_commandecommun.NomArticle, result.Value.NomArticle);
        }

        [TestMethod]
        public async Task NotFoundGetCommandeByStringTest()
        {
            var result = await _controller.GetCommandeByString("NonExistentCommande");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostCommandeTest_Entity()
        {
            var Commande = new Commande
            {
                NomArticle = "CommandePost"
            };

            var actionResult = await _controller.Postcommande(Commande);

            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)actionResult.Result;

            var createdCommande = (Commande)created.Value;
            Assert.AreEqual(Commande.NomArticle, createdCommande.NomArticle);
        }


        [TestMethod]
        public async Task DeleteCommandeTest()
        {
            var result = await _controller.Deletecommande(_commandecommun.IdCommande);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var deletedCommande = await _manager.GetByIdAsync(_commandecommun.IdCommande);
            Assert.IsNull(deletedCommande);
        }

        [TestMethod]
        public async Task NotFoundDeleteCommandeTest()
        {
            var result = await _controller.Deletecommande(0);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PutCommandeTest()
        {
            var Commande = new Commande()
            {
                IdCommande = _commandecommun.IdCommande,
                NomArticle = "CommandeUpdated",
                IdUtilisateur = 1,  // AJOUT OBLIGATOIRE
                Montant = "150"  // Optionnel mais recommandé
            };

            var result = await _controller.Putcommande(_commandecommun.IdCommande, Commande);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var fetchedCommande = await _manager.GetByIdAsync(_commandecommun.IdCommande);
            Assert.AreEqual(Commande.NomArticle, fetchedCommande.NomArticle);
        }

        [TestMethod]
        public async Task NotFoundPutCommandeTest()
        {
            var Commande = new Commande()
            {
                IdCommande = 0,
                NomArticle = "CommandeNonExistant"
            };

            var result = await _controller.Putcommande(0, Commande);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task BadRequestPutCommandeTest()
        {
            var Commande = new Commande()
            {
                IdCommande = _commandecommun.IdCommande + 1,
                NomArticle = "CommandeMismatched"
            };

            var result = await _controller.Putcommande(_commandecommun.IdCommande, Commande);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task BadRequestPostCommandeTest()
        {
            // Arrange : création d'une Commande invalide (Nom requis manquant)
            var Commande = new Commande
            {
                NomArticle = null
            };

            // Simuler la validation du ModelState
            _controller.ModelState.AddModelError("Nom", "Required");

            // Act : appeler le POST
            var actionResult = await _controller.Postcommande(Commande);

            // Assert : vérifier qu'on obtient bien un BadRequest
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
        }
    }
}
