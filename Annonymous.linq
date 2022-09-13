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

//Using Navigational properties and Annonymous data set(collection)

//reference: Student Notes/Demos/eRestaurant/Ling: Query and Method syntax.

//Fina all albums released in the 90's (1990 - 1999)
//order the albums by ascending year and then alphabetically by album tittle.
//Display the year, title, Artist Name and Release label.

//Concerns
	//a) Not all properties of Album are to be displayed.
	//b) The order of the properties are to be displayed in a different sequence than the definition of the properties on the entity Album.
	 //c) The Artist name is not on the table but it is on the Artist Table.
	 
	 //Solution: Use an annonymous data collection.
	 //The annonymous data instance is defined withing the select by declared fields (aka properties).
	 //the order of the fields on the new defined instance will be done in specifying the properties of the annonymous data collection.
	 
Albums
	.Where(x => x.ReleaseYear > 1989 && x.ReleaseYear < 2000)
	.OrderBy(x => x.ReleaseYear)
	.ThenBy(x => x.Title)
	.Select(x => new
				{
					Year = x.ReleaseYear,
					Title = x.Title,
					Artist = x.Artist.Name,
					Label = x.ReleaseLabel
				}
				)
//Changing the sort order for trial

Albums
	.Where(x => x.ReleaseYear > 1989 && x.ReleaseYear < 2000)
	
	.Select(x => new
	{
		Year = x.ReleaseYear,
		Title = x.Title,
		Artist = x.Artist.Name,
		Label = x.ReleaseLabel
	}
				)
	//.OrderBy(x => x.ReleaseYear) //Year is in the annonymous data type collection. ReleaseYear is NOT.
	.OrderBy(x => x.Year) 
	.ThenBy(x => x.Title)//OrderBy is having issues because ReleaseYear changed to year. The outcome of the where was feed into the select, so the outcome of the select should be used to do the orderby.