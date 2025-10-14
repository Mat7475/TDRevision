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

namespace TDRevisionTests.Controllers.MarqueTests
{
    [TestClass]
    [TestCategory("integration")]
    public class MarquesControllerTests
    {
        private CommandeController _controller;
        private AppDbContext _context;
        private IDataRepository<Commande, int, string> _repo;
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

            _controller = new CommandeController(_repo, _mapper);


            _context.Commandes.RemoveRange();
            await _context.SaveChangesAsync();

            _commandecommun = new Commande()
            {
                NomArticle = "ArticleCommun"
            };
            await _repo.AddAsync(_commandecommun);
        }
        [TestMethod]
        public async Task GetmarqueTest()
        {
            var result = await _controller.GetCommande(_commandecommun.IdCommande);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(_commandecommun.NomArticle, result.Value.NomArticle);
        }

        [TestMethod]
        public async Task NotFoundGetmarqueTest()
        {
            var result = await _controller.GetCommande(0);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetmarquesTest()
        {
            var result = await _controller.GetCommandes();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value.Any());
            Assert.IsTrue(result.Value.Any(m => m.NomArticle == _commandecommun.NomArticle));
        }

        [TestMethod]
        public async Task GetmarqueByStringTest()
        {
            var result = await _controller.GetCommandeByString(_commandecommun.NomArticle);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(_commandecommun.NomArticle, result.Value.NomArticle);
        }

        [TestMethod]
        public async Task NotFoundGetmarqueByStringTest()
        {
            var result = await _controller.GetCommandeByString("NonExistentMarque");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostMarqueTest_Entity()
        {
            var marque = new Commande
            {
                NomArticle = "CommandePost"
            };

            var actionResult = await _controller.Postcommande(marque);

            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)actionResult.Result;

            var createdMarque = (Commande)created.Value;
            Assert.AreEqual(marque.NomArticle, createdMarque.NomArticle);
        }


        [TestMethod]
        public async Task DeletemarqueTest()
        {
            var result = await _controller.Deletecommande(_commandecommun.IdCommande);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var deletedMarque = await _repo.GetByIdAsync(_commandecommun.IdCommande);
            Assert.IsNull(deletedMarque);
        }

        [TestMethod]
        public async Task NotFoundDeletemarqueTest()
        {
            var result = await _controller.Deletecommande(0);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PutmarqueTest()
        {
            var marque = new Commande()
            {
                IdCommande = _commandecommun.IdCommande,
                NomArticle = "CommandeUpdated"
            };

            var result = await _controller.Putcommande(_commandecommun.IdCommande, marque);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var fetchedMarque = await _repo.GetByIdAsync(_commandecommun.IdCommande);
            Assert.AreEqual(marque.NomArticle, fetchedMarque.NomArticle);
        }

        [TestMethod]
        public async Task NotFoundPutmarqueTest()
        {
            var marque = new Commande()
            {
                IdCommande = 0,
                NomArticle = "CommandeNonExistant"
            };

            var result = await _controller.Putcommande(0, marque);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task BadRequestPutmarqueTest()
        {
            var marque = new Commande()
            {
                IdCommande = _commandecommun.IdCommande + 1,
                NomArticle = "CommandeMismatched"
            };

            var result = await _controller.Putcommande(_commandecommun.IdCommande, marque);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task BadRequestPostmarqueTest()
        {
            // Arrange : création d'une marque invalide (Nom requis manquant)
            var marque = new Commande
            {
                NomArticle = null
            };

            // Simuler la validation du ModelState
            _controller.ModelState.AddModelError("Nom", "Required");

            // Act : appeler le POST
            var actionResult = await _controller.Postcommande(marque);

            // Assert : vérifier qu'on obtient bien un BadRequest
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
        }
    }
}
