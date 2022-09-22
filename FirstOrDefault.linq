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
	
	
	//Using firstORDefault()
	//Used to check when record exixted in a BLL service Method
	
	//let us find the first albums by Deep Purple.
	
	var artistparam = "Deep Purple";
	var resultsFOD = Albums
						.Where(a => a.Artist.Name.Equals(artistparam))
						.Select(a => a)
						.OrderBy(a => a.ReleaseYear)
						//.First()
						.FirstOrDefault()
						//.Dump()
						;
						
	if(resultsFOD != null)
	{
		resultsFOD.Dump();
	}
	else
	{
		Console.WriteLine($"No albums found for artist{artistparam}");
	}
}

//First() gives the first instance.
//firstordefault() allows you to continue your program when there is no instance
//FirstOrDefault is when you have sero or more items. While singleOrDefault is when you have a single item.