<Query Kind="Statements">
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

//Any and All
//these filter tests return a true or false condition
//they work at the complete collection level

Genres.Count().Dump();

//Let us show genres that have tracks which are not on any playlist.
Genres
	.Where(g => g.Tracks.Any(tr => tr.PlaylistTracks.Count() == 0))
	.Select(g => g)
	.Dump()
	;
	
	//Show genres that have all their tracks appearing at least once on a playlist
Genres
	.Where(g => g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0))
	.Select(g =>g)
	.Dump()
	;
	
	//there maybe times that using a !Any() -> All(!relationship)
	// and !All -> Any(!relationship)
	
	
	//Using All and Any in comparing 2 collections
	//if your collection is NOT a complex record, there is a Linq method called .Except that can be used to solve your query.
	
	//Compare the track collection of 2 people using All and Any
	
	//reoberto Almedia and Michelle Brooks
	
	
	var almeida = PlaylistTracks
					.Where(x => x.Playlist.UserName.Contains("AlmeidaR"))
					.Select(x => new
					{
						song = x.Track.Name,
						genre = x.Track.Genre.Name,
						id = x.TrackId,
						artist = x.Track.Album.Artist.Name
					})
					.Distinct()
					.OrderBy(x => x.song);
					//.Dump()
					;

var brooks = PlaylistTracks
				.Where(x => x.Playlist.UserName.Contains("BrooksM"))
				.Select(x => new
				{
					song = x.Track.Name,
					genre = x.Track.Genre.Name,
					id = x.TrackId,
					artist = x.Track.Album.Artist.Name
				})
				.Distinct()
				.OrderBy(x => x.song)
				;
				//.Dump();88
				
//List the tracks both Roberto and Michelle like.

//Here we compare two data sets together.

//Data in list A that is also in list B.
//Assume listA is roberto and listB is Michelle
//ListA is what is what you need to report from 
//ListB is what you wish to compare

//What songs does roberto like but not Michelle?

//Using Any
var c1 = almeida
			.Where(rob => !brooks.Any(mic => mic.id == rob.id))
			.OrderBy(rob => rob.song);
			//.Dump();

//Using All

var c2 = almeida
			.Where(rob => brooks.All(mic => mic.id != rob.id))
			.OrderBy(rob => rob.song);
			//.Dump();


var c3 = brooks
			.Where(mic => almeida.All(rob => rob.id != mic.id))
			.OrderBy(mic => mic.song)
			.Dump()
			;
//What songs does not Michelle and Roberto like?

var c4 = brooks
			.Where(mic => almeida.Any(rob => rob.id == mic.id))
			.OrderBy(mic => mic.song)
			.Dump()
			;