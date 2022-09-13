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

//List all Albums by release label. Any Album with no label should be indicated as unknown.
//List Title, Label and Artist Name.
//Orderby release label

//Understand the problem
//collection albums
//selective data: annonymous data set
//label (nullable): either Unknown or label name.****

//Start with design
//collection is Albums
//Select (new{})
//fields: title
//		  label ???? ternary operator (condition(s) ? true value : fasle value)
//		  Artist.Name
//coding and testing

Albums
	.Select(x => new{
		Title = x.Title,
		Label = x.ReleaseLabel == null ? "Unknown": x.ReleaseLabel,
		Artist = x.Artist.Name
	}
	)
	.OrderBy(x => x.Label)
	
//List all albums showing the Title, Artist name, Year and decade of release using oldies, 70s, 80s,90s or mordern.
//Order by year
//Design
//collection - Albums
//fields: title,
//        Artist name
//        year
//        < 9170
//        Oldies
//else
// (< 1980 then 70s
//		else
//			(<1990 then 80s
//			else(< 2000 then 90s
//else modern




Albums
	.Select(x => new {
		Title = x.Title,
		Artist = x.Artist.Name,
		Year = x.ReleaseYear,
		Decade = x.ReleaseYear < 1970 ? "Oldies":
				 x.ReleaseYear < 1980 ? "70s" : 
				 x.ReleaseYear < 1990 ? "80s" :
				 x.ReleaseYear < 2000 ? "90s" : "Modern"
	})
	.OrderBy(x => x.Year)
