﻿using System.Collections.Generic;
using System.Linq;
using DbLocalizationProvider.Internal;
using DbLocalizationProvider.Sync;
using Xunit;

namespace DbLocalizationProvider.Tests.EnumTests
{
    public class LocalizedEnumTests
    {
        public LocalizedEnumTests()
        {
            var types = new[] { typeof(DocumentEntity) };
            var sut = new TypeDiscoveryHelper();

            Assert.NotEmpty(types);

            _properties = types.SelectMany(t => sut.ScanResources(t));
        }

        private readonly IEnumerable<DiscoveredResource> _properties;

        [Fact]
        public void DiscoverEnumValue_NameAsTranslation()
        {
            var sut = new TypeDiscoveryHelper();
            var properties = sut.ScanResources(typeof(SampleStatus));

            var openStatus = properties.First(p => p.Key == "DbLocalizationProvider.Tests.EnumTests.SampleStatus.Open");

            Assert.Equal("Open", openStatus.Translation);
        }

        [Fact]
        public void DiscoverEnumWithPrefixKey()
        {
            var sut = new TypeDiscoveryHelper();
            var properties = sut.ScanResources(typeof(SampleStatusWithPrefix));

            var openStatus = properties.First(p => p.Key == "ThisIsPrefix.Open");

            Assert.Equal("Open", openStatus.Translation);
        }

        [Fact]
        public void EnumType_CheckDiscovered_Found()
        {
            var enumProperty = _properties.FirstOrDefault(p => p.Key == "DbLocalizationProvider.Tests.EnumTests.DocumentEntity.Status");
            Assert.NotNull(enumProperty);
        }

        [Fact]
        public void Test_EnumExpression_AsProperty()
        {
            var doc = new DocumentEntity
            {
                Status = SampleStatus.New
            };

            Assert.Equal("DbLocalizationProvider.Tests.EnumTests.DocumentEntity.Status", ExpressionHelper.GetFullMemberName(() => doc.Status));
        }

        [Fact]
        public void Test_EnumExpression_Directly()
        {
            Assert.Equal("DbLocalizationProvider.Tests.EnumTests.SampleStatus.Open", ExpressionHelper.GetFullMemberName(() => SampleStatus.Open));
        }

        [Fact]
        public void Test_MemberAccessExpression()
        {
            var doc = new DocumentEntity();
            Assert.Equal("DbLocalizationProvider.Tests.EnumTests.DocumentEntity", ExpressionHelper.GetFullMemberName(() => doc));
        }
    }
}
