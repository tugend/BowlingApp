Projektet er skrevet i .NET Core 2 og med Resharper støtte, i.e. autogenereret toString og Equals mm.

Jeg brugte cirka 1 dag og en aften på at hygge mig med projektet.
Opgaven er løst med følgende bemærkninger. 

Tiden løb lidt fra mig til sidst, men jeg vil mene opgaven er løst.

- Koden kan blive pænere og mere læsbar, flere kommentarer burde være fjernet.
- Der kan indsættes et par invariant checks, i.e. Debug.Assert flere steder for at gøre koden mere robust.
- To tests er ikke implementeret (og fejler lige nu)
- Nuværende score pr. frame for strikes og spares
  bliver ikke genberegnet i Runneren når efterfølgende slag giver bonus point, 
  de bliver dog regnet rigtigt ud i efterfølgende totaler. 
- Det er op til diskussion, men jeg ville nok mene at interfacene til 'calculatorne' er 
  premature abstraktioner når der ikke er behov for variabilitetspunkter.
- Håndtering af flere spillere er ikke implementeret.

Kørsel
1) Kør fra VS, RunnerCli projektet er sat til at køre det givne eksempel igennem til og med tredje sidste input.
2) Kør run_scenario.bat der er sat til samme som (1)
3) Kør fra kommando linien, optional input er en liste af integers fra 0-10 som vil blive indlæst som slag.
dotnet run --project ./CliRunner.csproj 1 4 4 5 6 4 5 5 10 0 1 7 3 6 4 10 2



