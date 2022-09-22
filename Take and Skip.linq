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
	//.Take() and .Skip() - other versions take conditions  - take while and skip while
	//in CPSC1517, when you wanted to use the supplied pagination.
	//the query method was to supply only the needed record for the display, not the entire collection.
	//a) the query was executed, returning a collection of size x
	//b) obtained the total count (x) of returned record
	//c) calculated the number of records to skip(pagenumber -1)* pagesize
	//d) on the returned method statement, we used 
	//return variablename.Skip(rowsSkipped).Take(pagesize).ToList()
}

// You can define other methods, fields, classes and namespaces here