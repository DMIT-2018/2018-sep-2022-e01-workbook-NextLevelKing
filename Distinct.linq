<Query Kind="Program">
  <Connection>
    <ID>a3e3431a-be32-4564-8fcd-e08389e36724</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	//Distinct
	//It removes duplicate reported lines.
	//Get a list of customer countries
	
	var resultDistinct = Customers
							.OrderBy(c => c.Country)
							.Select(c => c.Country)
							.Distinct()
							
							.Dump();
}

// You can define other methods, fields, classes and namespaces here