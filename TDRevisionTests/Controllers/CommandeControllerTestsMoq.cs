using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDRevision.Controllers;
using TDRevision.Mapper;
using TDRevision.Models;
using TDRevision.Models.DataManager;
using TDRevision.Models.DTO;
using TDRevision.Models.EntityFramework;
using TDRevision.Models.Repository;

namespace TD1Tests.Controllers.CommandeTests
{
    [TestClass]
    [TestCategory("unit")]
    public class CommandesControllerMoqTests
    {
        private Mock<IDataRepository<Commande,int,string>> _mockRepo;
        private CommandeController _controller;
        private Commande _CommandeCommun;
        private IMapper _mapper;

        [TestInitialize]
        public void Initialize()
        {
            // Création du mock du repository
            _mockRepo = new Mock<IDataRepository<Commande, int, string>>();

            // Création du Commande de référence
            _CommandeCommun = new Commande
            {
                IdCommande = 1,
                NomArticle = "CommandeDefault",
            };

            // Configuration AutoMapper avec ton profil existant
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappeurProfile>(); 
            });
            _mapper = config.CreateMapper();

            // Injection dans le controller
            _controller = new CommandeController(
                _mockRepo.Object,
                _mapper   
            );
        }
        [TestMethod]
        public async Task GetCommandeTest()
        {

            _mockRepo.Setup(repo => repo.GetByIdAsync(1))
                     .ReturnsAsync(_CommandeCommun);

            ActionResult<Commande> result = await _controller.GetCommande(1);

            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(Commande));
            Assert.AreEqual(_CommandeCommun.NomArticle, result.Value.NomArticle);
            Assert.AreEqual(_CommandeCommun.IdCommande, result.Value.IdCommande);
        }

        [TestMethod()]
        public void NotFoundGetCommandeTestMoq()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(0))
                     .ReturnsAsync((Commande)null);
            // Act
            ActionResult<Commande> result = _controller.GetCommande(0).GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAllCommandesTest_AutoMapper()
        {
            // Arrange
            var CommandesList = new List<Commande>
            {
                _CommandeCommun,
                new Commande
                {
                    IdCommande = 2,
                    NomArticle = "CommandeSecondaire",
                }
            };

            // Mock du repository → retourne la liste de Commandes
            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(CommandesList);

            // Act
            ActionResult<IEnumerable<CommandeDTO>> result = await _controller.GetCommandes();

            // Assert
            Assert.IsNotNull(result.Value);
            var resultList = result.Value.ToList();
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual("CommandeDefault", resultList[0].NomArticle);
            Assert.AreEqual("CommandeSecondaire", resultList[1].NomArticle);
        }


        [TestMethod()]
        public void GetCommandeByStringTestMoq()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByKeyAsync("CommandeDefault"))
                     .ReturnsAsync(_CommandeCommun);
            // Act
            ActionResult<Commande> result = _controller.GetCommandeByString("CommandeDefault").GetAwaiter().GetResult();
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Commande));
            Assert.AreEqual(_CommandeCommun.NomArticle, result.Value.NomArticle);
        }

        [TestMethod()]
        public void NotFoundGetCommandeByStringTestMoq()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByKeyAsync("NonExistentProduct"))
                     .ReturnsAsync((Commande)null);
            // Act
            ActionResult<Commande> result = _controller.GetCommandeByString("NonExistentProduct").GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void PostCommandeTestMoq()
        {
            // Arrange
            var newCommande = new Commande
            {
                IdCommande = 3,
                NomArticle = "NouveauCommande",
            };
            _mockRepo.Setup(repo => repo.AddAsync(newCommande))
                     .Returns(Task.CompletedTask)
                     .Verifiable();
            // Act
            var result = _controller.Postcommande(newCommande).GetAwaiter().GetResult();
            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetByID", createdAtActionResult.ActionName);
            Assert.AreEqual(newCommande.IdCommande, ((Commande)createdAtActionResult.Value).IdCommande);
            _mockRepo.Verify(repo => repo.AddAsync(newCommande), Times.Once);
        }

        [TestMethod()]
        public void PutCommandeTestMoq()
        {
            // Arrange
            var existingCommande = new Commande
            {
                IdCommande = 1,
                NomArticle = "CommandeDefault"
            };
            var updatedCommande = new Commande
            {
                IdCommande = 1,
                NomArticle = "CommandeMisAJour"
            };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1))
                         .ReturnsAsync(existingCommande);
            _mockRepo.Setup(repo => repo.UpdateAsync(existingCommande, updatedCommande))
                     .Returns(Task.CompletedTask)
                     .Verifiable();
            // Act
            var result = _controller.Putcommande(1, updatedCommande).GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockRepo.Verify(repo => repo.UpdateAsync(existingCommande, updatedCommande), Times.Once);
        }

        [TestMethod()]
        public void DeleteCommandeTestMoq()
        {
            // Arrange
            var CommandeToDelete = new Commande
            {
                IdCommande = 1,
                NomArticle = "CommandeDefault"
            };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1))
                      .ReturnsAsync(CommandeToDelete);
            _mockRepo.Setup(repo => repo.DeleteAsync(CommandeToDelete))
                     .Returns(Task.CompletedTask)
                     .Verifiable();
            // Act
            var result = _controller.Deletecommande(1).GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockRepo.Verify(repo => repo.DeleteAsync(CommandeToDelete), Times.Once);
        }

        [TestMethod()]
        public void NotFoundDeleteCommandeTestMoq()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(999))
                     .ReturnsAsync((Commande)null);
            // Act
            var result = _controller.Deletecommande(999).GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void BadRequestPutCommandeTestMoq()
        {
            // Arrange
            var updatedCommande = new Commande
            {
                IdCommande = 2, // ID différent de celui passé en paramètre
                NomArticle = "CommandeMisAJour"
            };
            // Act
            var result = _controller.Putcommande(1, updatedCommande).GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void BadRequestPostCommandeTestMoq()
        {
            // Arrange
            var newCommande = new Commande
            {
                IdCommande = 3,
                NomArticle = "NouveauCommande"
            };
            _controller.ModelState.AddModelError("NomArticle", "Le nom est requis");
            // Act
            var result = _controller.Postcommande(newCommande).GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod()]
        public void NotFoundPutCommandeTestMoq()
        {
            // Arrange
            var updatedCommande = new Commande
            {
                IdCommande = 1,
                NomArticle = "CommandeMisAJour"
            };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1))
             .ReturnsAsync((Commande)null);
            // Act
            var result = _controller.Putcommande(1, updatedCommande).GetAwaiter().GetResult();
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}