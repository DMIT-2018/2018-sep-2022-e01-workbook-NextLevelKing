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

//Grouping
//We need a pk or fk in our queries but if it is not built-in already, we use this technique grouping.

//we can group on an entire entity, we can group on two fields

//Don's comments on grouping
//When you create a group, it builds two(2) components
//a) Key component (this is the deciding criteria value(s) defining the group
//		reference this component using the groupname.Key and optionally .propertyname ie groupname.Key.[.propertyname]
//		1 property use -  value for key: groupname.Key
//  	n number of properties use - value for key: groupname.Key.propertyname

//	(property <- >, field <- >, attribute <- >, value are all talking about the same thing.

// b) data of the group( raw instances of the collection ie raw record.

//Ways to group
//a) by a single field, attribute, property) use groupname.Key
//b) by a set of columns (annonymous data set) use groupname.Key.property
//C) by using an entity (ie entity name/property) - use groupname.Key.Property - the result will be a data set.

//Concept processing
//start with a "pile" of data (original collection prior to grouping)
//specify the grouping property or property(ies)
//the result of the group operation will be to "place the data into smaller piles".
//		the piles are dependent on the grouping property(ies) value(s).
//		The grouping property(ies) become the key
//		the individual instances are the data in the smaller piles.
//		the entire individual instance of the original collection is placed in these smaller piles.
//		then you can manipulate each of the smaller piles using your linq commands.

//Grouping is different then Ordering. Odering is the final re-sequencing of a collection for display.
//Grouping re-organizes a collection into seprate, usually smaller collections for further processing(ie aggregates)

//Grouping is an excellent way to organize your data especially if 
//		you need to process data on a property that is not a relative key 
//		such as a foreign key which forms a natural group using the navigational properties.

//Example - Display albums by release year
//This request does NOT need grouping
//This request is an Ordering of output :OrderBy
//the ordering only affects display

Albums
	.OrderBy(a => a.ReleaseYear)

//Now let us do this: Display albums grouped by release year
//This is an explicit request to breakup the display into desired "piles"

Albums
	.GroupBy(x => x.ReleaseYear)

	//processing on the groups created by the Group command
	//Display the number of albums produced each year
	//list only the year that has more than 10 albums


Albums
	.GroupBy(x => x.ReleaseYear)
	//.Where(egP => egP.Count() > 10)  - filters against each group pile
	.Select(eachgroupPile => new
	{
		Year = eachgroupPile.Key,
		NumberOfAlbums = eachgroupPile.Count()
	})
	.Where(x => x.NumberOfAlbums > 10) // you are filtering against the output of the .Select() command

	//Use a multiple series of properties to form the group
	//include a nested query to report the small pile group

//Display albums grouped by Releaselabel, ReleaseYear. Display
//ReleaseYear and number of albums. List only the years with 2 or 
// more albums released. for each album, display the title, artist, number of Tracks on the album and release year.



Albums
	.GroupBy(a => new { a.ReleaseYear, a.ReleaseLabel})
	.Where(egP => egP.Count() > 2) 
	.Select(eachgroupPile => new
	{
		Label = eachgroupPile.Key.ReleaseLabel,
		Year = eachgroupPile.Key.ReleaseYear,
		NumberOfAlbums = eachgroupPile.Count(),
		AlbumItems = eachgroupPile
						.Select(egpInstance => new
						{
							title = egpInstance.Title,
							artist = egpInstance.Artist.Name,
							trackcountA = egpInstance.Tracks.Count(),
							trackcountB = egpInstance.Tracks.Select(x => x).Count(),
							YearOfAlbum = egpInstance.ReleaseYear
						})
						.ToList()
	})





































	