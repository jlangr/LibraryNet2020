Todo:

- Rider debugging breakpoints change to "not" symbol after debugging starts
- Console logging: w/ XUnit, can only use a console helper:
     https://xunit.github.io/docs/capturing-output.html
- How to use tuples. Tried installing System.ValueTuple (see https://stackoverflow.com/questions/38382971/predefined-type-system-valuetuple´2´-is-not-defined-or-imported) but seemed to still have a problem.
  Appear to be on .NET 4.5 (not 4.7+). How to change that in Rider?
  .NET.core 2.0.x? Maybe in Rider. Need to be on 3.1.

-- installed .NET Core 3.1 
https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-3.1.9-macos-x64-installer



Installed .NET SDK

clean
=====
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator --version 3.1.4

Microsoft.EntityFrameworkCore.Design package (nuget 3.1.9)
Microsoft.EntityFrameworkCore.InMemory (nuget 3.1.9)
Microsoft.EntityFrameworkCore.SQlite (nuget 3.1.9)
Microsoft.VisualStudio.Web.CodeGeneration.Design (nuget 3.1.4)
Microsoft.EntityFrameworkCore.SqlServer (nuget 3.1.9)

dotnet tool install --global dotnet-aspnet-codegenerator --version 3.1.4

dotnet aspnet-codegenerator controller -name BranchesController -m Branch -dc BranchContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

# repeat for Holdings, Materials, Branches


# make sure the solution is closed (or maybe just the executable)
dotnet ef migrations add Initial -v
dotnet ef database update

dotnet ef migrations add RestOfTables


TODO: look at their code. OO? functional? Linq w/ code or regular linq?
TODO: navigation on view
TODO: show edit error (add duplciate branch) on Holdings page

---
https://andrewlock.net/should-you-unit-test-controllers-in-aspnetcore/
---

console logging from unit test:
https://www.jetbrains.com/help/rider/Xunit.XunitTestWithConsoleOutput.html
        private readonly ITestOutputHelper output; -- pass in through test constructor


