# Integra

Package|Last version
-|-
Integra.DbExecutor|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Integra.DbExecutor.svg)](https://www.nuget.org/packages/Integra.DbExecutor/)

Abstractions for integration tests:

## Builds

Branch|Build status
-|-
master|[![Build status](https://ci.appveyor.com/api/projects/status/c5queapwprgm9spl/branch/master?svg=true)](https://ci.appveyor.com/project/Valeriy1991/integra-pug9r/branch/master)
dev|[![Build status](https://ci.appveyor.com/api/projects/status/d6o65bhfr2u5nben/branch/dev?svg=true)](https://ci.appveyor.com/project/Valeriy1991/integra/branch/dev)

AppVeyor Nuget project feed: 
https://ci.appveyor.com/nuget/integra-ksjcb4t7lhvd

## Dependencies

Project|Dependency
-|-
Integra.DbExecutor|[DbConn.DbExecutor.Dapper](https://github.com/Valeriy1991/DbExecutor)

## How to use

1. Install this package to your project with integration tests:
```
Install-Package Integra.DbExecutor
```
2. Create integration test class and inherit it from `DbIntegrationTest` class and be sure that you override the `Init` method and add attribute from you favorite test framework:
```csharp
using NUnit.Framework

[TestFixture]
[Category("Integration test")]
public class CreateUserCommandTest : DbIntegrationTest
{
    private CreateUserCommand CreateTestedCommand(IDbExecutor dbExecutor)
    {
        return new CreateUserCommand(dbExecutor)
    }
    
    [SetUp] // NUnit
    //[TestInitialize] // MS Test
    public override void Init()
    {
        base.Init();
    }

    [Test]
    public void Execute__CreateNewUserSuccess()
    {
        var connectionStringToDb = "data-source:...";
        using (var dbExecutor = CreateTransactionalDbExecutor(connectionStringToDb))
        {
            // Arrange
            var command = CreateTestedCommand(dbExecutor);
            var user = new User();
            // Act
            var createResult = command.Execute(user);
            // Assert
            Assert.IsTrue(createResult.Success());
        }
    }
}
```
3. **Or** you can create your own base integration class and inherit it from `DbIntegrationTest` class:
```csharp
using NUnit.Framework;
//...
public abstract class IntegrationTest : DbIntegrationTest
{   
    // Set attribute for init method from you favorite test framework
    [SetUp] // NUnit
    //[TestInitialize] // MS Test
    public override void Init()
    {
        base.Init();
    }

    protected string ConnectionString => "data-source:...";

    protected IDbExecutor CreateDbExecutor()
    {
        return CreateDbExecutor(ConnectionString);
    }

    protected IDbExecutor CreateTransactionalDbExecutor()
    {
        return CreateTransactionalDbExecutor(ConnectionString);
    }
}
```
3. Then create integration test class and inherit it from your own base integration test:
```csharp
using NUnit.Framework

[TestFixture]
[Category("Integration test")]
public class CreateUserCommandTest : IntegrationTest
{
    private CreateUserCommand CreateTestedCommand(IDbExecutor dbExecutor)
    {
        return new CreateUserCommand(dbExecutor)
    }

    [Test]
    public void Execute__CreateNewUserSuccess()
    {
        using (var dbExecutor = CreateTransactionalDbExecutor())
        {
            // Arrange
            var command = CreateTestedCommand(dbExecutor);
            var user = new User();
            // Act
            var createResult = command.Execute(user);
            // Assert
            Assert.IsTrue(createResult.Success());
        }
    }
}
```