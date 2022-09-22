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
	//Conversions
	//collection we will look at are Iqueryable and IEnumerable and List
	
	//Display all albums and their tracks. Display the albums title
	//artist name and album tracks. For each track show the song name
	//and play time, Show only albums with 25 or more tracks.
	
	//
	//IEnumerable<AlbumTracks> albumlist = Albums
	//										.Where(a => a.Tracks.Count() >=25)
	//										.Select( a => new AlbumTracks
	//										{
	//											Title = a.Title,
	//											Artist = a.Artist.Name,
	//											Songs = a.Tracks
	//														.Select(tr => new SongItem
	//														{
	//															Songs = tr.Name,
	//															Playtime = tr.Milliseconds/1000.0
	//														})
	//														.ToList()
	//										})
	//										.Dump();

	List<AlbumTracks> albumlist = Albums
										.Where(a => a.Tracks.Count() >= 25)
										.Select(a => new AlbumTracks
										{
											Title = a.Title,
											Artist = a.Artist.Name,
											Songs = a.Tracks
														.Select(tr => new SongItem
														{
															Songs = tr.Name,
															Playtime = tr.Milliseconds / 1000.0
														})
														.ToList()
										})
										.ToList()
										.Dump();


}

// You can define other methods, fields, classes and namespaces here

public class SongItem
{
	public string Songs { get; set; }
	public double Playtime{ get; set; }
	
}

//public class AlbumTracks
//{
//	public string Title { get; set; }
//	public string Artist { get; set; }
//	public IEnumerable<SongItem> Songs{get; set;}
//}

public class AlbumTracks
{
	public string Title { get; set; }
	public string Artist { get; set; }
	public List<SongItem> Songs { get; set; }
}

