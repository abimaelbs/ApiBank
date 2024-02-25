using ApiBank.Web.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiBank.Test.Models
{
    public class ContaTest
    {
        private readonly string _name;
        private readonly DateTime? _date;
        private readonly string _type;

        // Setup
        public ContaTest()
        {
            var faker = new Faker("pt_BR");

            _name = faker.Random.Word();
            _date = faker.Date.Recent();
            _type = faker.Random.Word();
        }

        [Fact]
        public void DeveCadastrarConta()
        {
            // Action
            var conta = new Conta(_name, _date, _type);

            // Assert
            Assert.Equal(_name, conta.Name);
            Assert.Equal(_date, conta.Date);
            Assert.Equal(_type, conta.Type);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveContaTerNomeNuloOuVazio(string nome)
        {
            Assert.Throws<Exception>(() => new Conta(nome, _date, _type));
        }

        [Theory, MemberData(nameof(GetData))]
        public void NaoDeveContaTerDataNulaOuDataMininaMaxima(DateTime? data)
        {
            Assert.Throws<Exception>(() => new Conta(_name, data, _type));
        }

        public static IEnumerable<object[]> GetData => new List<object[]>
        {
            new object[] { null },
            new object[] { DateTime.MinValue },
            new object[] { DateTime.MaxValue }
        };

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveContaTerTipoNuloOuVazio(string tipo)
        {
            Assert.Throws<Exception>(() => new Conta(_name, _date, tipo));
        }
    }    
}
