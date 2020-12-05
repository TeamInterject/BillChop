using NUnit.Framework;
using BillChopBE.Extensions;

namespace BillChopBETests
{
    public class AutoMapperTests
    {
        [Test]
        public void AssertMapping() 
        {
            //Arrange
            //Act
            var config = ServiceCollectionAutoMapperExtensions.CreateMapperConfig();

            //Assert
            config.AssertConfigurationIsValid();
        }
    }
}