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



//strongly type data set can't be done in expression and statement environment. Rather we use program environment.
//first thing you see is something that looks like console app without any of the overhead of visual studio. Thus;

void Main()
{
	//Find songs by partial name
	// display the album title, song and artist name.
	//order by song
//	
//	var songCollections = Tracks
//							.Where(t => t.Name.Contains("dance"))
//							.Select(t => new
//							{
//								Album =t.Album.Title,
//								Song = t.Name,
//								Artist = t.Album.Artist.Name
//								
//							}
//							);
//		songCollections.Dump();
//
////I added s to songCollection to make a difference so that the program will run.
//	var songCollection = Tracks
//						.Where(t => t.Name.Contains("dance"))
//						.Select(t => new SongList
//						{
//							Album = t.Album.Title,
//							Song = t.Name,
//							Artist = t.Album.Artist.Name
//
//						}
//						);
//	songCollection.Dump();

//let us pretend Main() is a webpage and assume that a value was enetered and we also assume a post button was pressed and assume that Main() is the post event.

	string inputValue = "dance";
	List<SongList> songCollection = SongsByPartialName(inputValue);
	songCollection.Dump();

}

// you can define other methods, fields, classess and namespace here

//Now let us create a strongly-typed data set.

//C# really enjoys strongly typed data fields.
//wheather these fields are primitive data type such as int, double etc, or developer defined data types...(ie class)

//Let us create a class



public class SongList{
	public string Album{get; set;}
	public string Song { get; set; }
	public string Artist { get; set; }
}

//TO MAKE IT STRONGLY TYPED< WE add NEW ABOVE WITH SongList


//Imagine the following method exists in a service in your BLL
//this method receives the web page parameter value for the query.
//This method will need to return a collection.

List<SongList> SongsByPartialName(string partialSongName)
{
	
	//let us pretend the query is in the list
	var songCollection = Tracks  // note var is handled at execution time but we need something that can be handled during compilation time. Such as IEnummerable (handles data from inside the memory) and IQueryable(handles data coming from inside SQL).
					.Where(t => t.Name.Contains(partialSongName))
					.OrderBy(t => t.Name)
					.Select(t => new SongList
					{
						Album = t.Album.Title,
						Song = t.Name,
						Artist = t.Album.Artist.Name

					}
					);
	return songCollection.ToList();
}