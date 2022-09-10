<Query Kind="Statements">
  <Connection>
    <ID>54bf9502-9daf-4093-88e8-7177c12aaaaa</ID>
    <NamingService>2</NamingService>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\ChinookDemoDb.sqlite</AttachFileName>
    <DisplayName>Demo database (SQLite)</DisplayName>
    <DriverData>
      <PreserveNumeric1>true</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.Sqlite</EFProvider>
      <MapSQLiteDateTimes>true</MapSQLiteDateTimes>
      <MapSQLiteBooleans>true</MapSQLiteBooleans>
    </DriverData>
  </Connection>
</Query>

//Statement ide(means development environment.)
// This environment expects the use of C# statements grammar.
// the results of a query is not automatically displayed as in the Expression environment.
//to display the results, you need to .Dump() the variable holding the data result.

//IMPORTANT!! - .Dump() is a linqpad method. It is NOT a C# method. ie, it can only be used in linqpad.
//Within the statement environment, one can run ALL the queries in ONE execution.

var qsyntaxlist = from arowoncollection in Albums
					select arowoncollection;
					
	qsyntaxlist.Dump();


//Method syntax

var msyntaxlist = Albums // table was brought back, and passed into the select method, which was executed and passed into the dump method.
   .Select (arowoncollection => arowoncollection)
   .Dump();
   
   //msyntaxlist.Dump();
   
   //no where clause
   
   var QueenAlbums = Albums  // It brought back all the albums from table Album, the albums were sent into the filter which brought back some rows. those rows where then dumped.
						.Where(a => a.Artist.Name.Contains("Queen"))
						.Dump()
						;