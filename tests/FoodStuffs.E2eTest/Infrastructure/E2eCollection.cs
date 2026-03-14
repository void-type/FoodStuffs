using Xunit;

namespace FoodStuffs.E2eTest.Infrastructure;

[CollectionDefinition("E2e")]
public class E2eCollection : ICollectionFixture<E2eFixture> { }
