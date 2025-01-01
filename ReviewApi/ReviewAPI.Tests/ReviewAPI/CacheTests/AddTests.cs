using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using ReviewAPI.Services.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ReviewAPI.CacheTests
{
    public class AddTest
    {
        Mock<IMemoryCacheWrapper> _memoryCacheWrapperMock;
        Cache _cache;

        [SetUp]
        public void SetUp()
        {
            _memoryCacheWrapperMock = new Mock<IMemoryCacheWrapper>();
            _cache = new Cache(_memoryCacheWrapperMock.Object);
        }

        [Test]
        public void Add_NullValue_Throws_ArgumentintEmptyException()
        {
            var nullFake = new KeyValuePair<int, object>(1, null);

            Assert.Throws<ArgumentNullException>(() => _cache.Add(It.IsAny<CacheType>(), nullFake.Key, nullFake.Value));

            _memoryCacheWrapperMock
                .Verify(
                   mock => mock.Set(
                       It.IsAny<string>(),
                       It.IsAny<object>(),
                       It.IsAny<MemoryCacheEntryOptions>()
                   ),
                   Times.Never);
        }


        [Test]
        public void Add_ValidInput_CachesValue()
        {
            var correctFake = new KeyValuePair<int, object>(1, "correct value");

            _cache.Add(It.IsAny<CacheType>(), correctFake.Key, correctFake.Value);

            _memoryCacheWrapperMock.Verify(
                mock => mock.Set(
                    It.IsAny<CacheType>() + "_" + correctFake.Key.ToString(),
                    correctFake.Value,
                    It.IsAny<MemoryCacheEntryOptions>()
                ),
                Times.Once
            );
        }

    }
}
