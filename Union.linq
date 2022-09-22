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
	//Union()
	//Rules in linq are the same as SQL
	//result is the same as sql, combine separate collections into one.
	//the syntax is (queryA).Union(queryB)[.Union(query...)]------------------[ ] means optionally
	//rules:
	// No of columns the same
	//column datatypes must be the same
	//ordering should be done as a method after the last union.

	var resultsUnionA = (Albums
							.Where(x => x.Tracks.Count() == 0)
							.Select(x => new
							{
								title = x.Title,
								totalTracks = 0,
								totalCost = 0.00m,
								averageLength = 0.00d

							})
							)



						.Union(Albums
						.Where(x => x.Tracks.Count() > 0)
						.Select(x => new
						{
							title = x.Title,
							totalTracks = x.Tracks.Count(),
							totalCost = x.Tracks.Sum(tr => tr.UnitPrice),
							averageLength = x.Tracks.Average(tr => tr.Milliseconds)

						})
						)
							.OrderBy(x => x.totalTracks)
							.Dump()
							;
}

// You can define other methods, fields, classes and namespaces here