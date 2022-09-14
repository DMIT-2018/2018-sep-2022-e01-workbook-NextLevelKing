<Query Kind="Expression">
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

Albums

// query syntax to list all records in an entiry (table, collection, bracket)

from arowoncollection in Albums
select arowoncollection

//method syntax to list all records in an entity//

Albums
   .Select (arowoncollection => arowoncollection)
   
   //Where clause
   //filter method
   //the conditions are setup as you would in c#
   //beware that Linqpad may NOT like some C# syntax (DateTime)
   //beware that linq is conerted to SQL which may not like 
   //certain C# syntax because SQL couldnot convert
   
   
   //syntax
   //notice that the method syntax makes use of the lambda expression
   //Lambdas are common when performing the linq method syntax.
   //.Where(lambda expression)
   //.Where(x => condition [logical operator condition2....])
   //Example
   //.Where(x => Bytes > 350000)
   
   Tracks
   	.Where(x => x.Bytes > 10000000)
	
	from x in Tracks
	where x.Bytes > 700000000
	select x
	
	
	
	
	//Find all the albums of the artist Queen.
	//concerns: the artist name is in another table
		//in an sql Select you would be using an inner join
		// in Linq you DO NOT need to specify your inner join
		// instead use the "navigational properties" of your entity to generate the relationship
	
	Albums
		.Where(a => a.Artist.Name.Contains("Queen"))