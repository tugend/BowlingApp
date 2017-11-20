Projektet er skrevet i .NET Core 2 og med Resharper st�tte, i.e. autogenereret toString og Equals mm.

Jeg brugte cirka 1 dag og en aften p� at hygge mig med projektet.
Opgaven er l�st med f�lgende bem�rkninger. 

Tiden l�b lidt fra mig til sidst, men jeg vil mene opgaven er l�st.

- Koden kan blive p�nere og mere l�sbar, flere kommentarer burde v�re fjernet.
- Der kan inds�ttes et par invariant checks, i.e. Debug.Assert flere steder for at g�re koden mere robust.
- To tests er ikke implementeret (og fejler lige nu)
- Nuv�rende score pr. frame for strikes og spares
  bliver ikke genberegnet i Runneren n�r efterf�lgende slag giver bonus point, 
  de bliver dog regnet rigtigt ud i efterf�lgende totaler. 
- Det er op til diskussion, men jeg ville nok mene at interfacene til 'calculatorne' er 
  premature abstraktioner n�r der ikke er behov for variabilitetspunkter.
- H�ndtering af flere spillere er ikke implementeret.

K�rsel
1) K�r fra VS, RunnerCli projektet er sat til at k�re det givne eksempel igennem til og med tredje sidste input.
2) K�r run_scenario.bat der er sat til samme som (1)
3) K�r fra kommando linien, optional input er en liste af integers fra 0-10 som vil blive indl�st som slag.
dotnet run --project ./CliRunner.csproj 1 4 4 5 6 4 5 5 10 0 1 7 3 6 4 10 2



