<Query Kind="Program">
  <Connection>
    <ID>93984414-8093-400e-9209-d2d73e55aba9</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>.</Server>
    <Database>Chinook</Database>
    <DisplayName>Chinook-Entity</DisplayName>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	#region CQRS Queries

	//Main is going to represent the web page post method
	//pretend we have some fields already
	
	//coded and tested the FetchTracksBy query
	try
	{
		string searcharg = "Deep";
		string searchby = "Artist";
		List<TrackSelection> tracklist = TracksServices_TracksFetchBy(searcharg, searchby);
		//tracklist.Dump();
		
		
		//coded and tested the FetchPlaylist query
		string playlistname = "hansenb1";
		string username = "HansenB"; // this is a user name which will come from O/S via security
		List<PlaylistTrackInfo> playlist = PlaylistTrack_FetchByPlaylist(playlistname, username);
		//playlist.Dump();
		
		
		//coded and tested the Add_Track trx
		//the command method will receive no collection but will receive individual arguments.
		// arguments will be trackid, playlistname and username.
		
		//test tracks
		//793 A castle full of Rascals
		//822 A Twist in the tail
		//543 Burn
		//756 Child in Time
		
		//on the web page, the post method would have already have access to the 
		// BindProperty variables containing the input values.
		
		
		//Example
		
		//playlistname = "hansebtest1";
		//int trackid = 756;
		
		// the post will call the service method to process the data
		//PlaylistTrack_AddTrack(playlistname, username, trackid); -tested
		
		//on the web page, the post method would have already  have access to the 
		//BindProperty variables containing the input values.
		
		//Test data for remove
		playlistname = "hansenbtest";
		List<PlaylistTrackTRX> trackListInfo = new List<PlaylistTrackTRX>();
		trackListInfo.Add(new PlaylistTrackTRX()
							{
								SelectedTrack = true,
								TrackId = 793,
								TrackNumber = 1,
								TrackInput = 0
							});

		trackListInfo.Add(new PlaylistTrackTRX()
		{
			SelectedTrack = true,
			TrackId = 543,
			TrackNumber = 1,
			TrackInput = 0
		})


		trackListInfo.Add(new PlaylistTrackTRX()
		{
			SelectedTrack = true,
			TrackId = 822,
			TrackNumber = 1,
			TrackInput = 0
		})


 		//call the service method for remove to process data
		
		PlaylistTrack_RemovePlaylist(playlistname, username, trackListInfo);
		//once the service method is complete, the webpage would refresh
		playlist = PlaylistTrack_FetchByPlaylist(playlistname, username);
		playlist.Dump();
	}
	
	catch(ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch(Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}


	#endregion
}
	
	#region TrackServices class
	
	public List<PlaylistTrackInfo> PlaylistTrack_FetchByPlaylist(string playlistname, string username)
	{
		if (string.IsNullOrWhiteSpace(playlistname))
		{
			throw new ArgumentNullException("No playlist name submitted");
		}
	
		if (string.IsNullOrWhiteSpace(username))
		{
			throw new ArgumentNullException("No user name submitted");
		}
		
		IEnumerable<PlaylistTrackInfo> results = PlaylistTracks
												.Where(x => (x.Playlist.Name.Equals(playlistname)) &&
														x.Playlist.UserName.Equals(username)) 
												.Select(x => new PlaylistTrackInfo
												{
													TrackId = x.TrackId,
													SongName = x.Track.Name,
													TrackNumber = x.TrackNumber,
													Milliseconds = x.Track.Milliseconds

												})
												.OrderBy(x => x.TrackNumber);
		return results.ToList();
	}

public List<TrackSelection> TracksServices_TracksFetchBy(string searcharg, string searchby)
{
	if (string.IsNullOrWhiteSpace(searcharg))
	{
		throw new ArgumentNullException("No search value submitted");
	}

	if (string.IsNullOrWhiteSpace(searchby))
	{
		throw new ArgumentNullException("No search style submitted");
	}

	IEnumerable<TrackSelection> results = Tracks
											.Where(x => (x.Album.Artist.Name.Contains(searcharg) &&
													searchby.Equals("Artist")) || (x.Album.Title.Contains(searcharg) &&
													searchby.Equals("Album")))
											.Select(x => new TrackSelection
											{
												TrackId = x.TrackId,
												SongName = x.Name,
												AlbumTitle = x.Album.Title,
												ArtistName = x.Album.Artist.Name,
												Milliseconds = x.Milliseconds,
												Price = x.UnitPrice
											});
	return results.ToList();
}


#region Command TRX methods

void PlaylistTrack_AddTrack(string playlistname, string username, int trackid){

	//locals
	Tracks trackexists = null;
	
	Playlists playlistExists = null;
	PlaylistTracks playlisttrackexists = null;
	int tracknumber = 0;

	if (string.IsNullOrWhiteSpace(playlistname))
	{
		throw new ArgumentNullException("No playlist name submitted");
	}

	if (string.IsNullOrWhiteSpace(username))
	{
		throw new ArgumentNullException("No user name submitted");
	}
	
	trackexists = Tracks	
					.Where(x => x.TrackId == trackid)
					.Select(x => x)
					.FirstOrDefault();


	if (trackexists == null)
	{
		throw new ArgumentNullException("Selected track no longer on file. Refresh track table");
	}
	
	
	//remember B/R playlist name must be unique within a user
	playlistExists = Playlists
						.Where(x => x.Name.Equals(playlistname) && x.UserName.Equals(username))
						.Select(x => x)
						.FirstOrDefault();
						
	if(playlistExists == null)
	{
		playlistExists = new Playlists()   // instantiating at the same time. we reused variables
		{
			Name = playlistname,
			UserName = username
		};
		Playlists.Add(playlistExists);
		tracknumber = 1;
	}
	else
	{
		//B/R a track may only exist once on a playlist
		
		playlisttrackexists = PlaylistTracks
									.Where(x => (x.Playlist.Name.Equals(playlistname) 
									&& x.Playlist.UserName.Equals(username) 
									&& x.TrackId == trackid))
									.Select(x => x)
									.FirstOrDefault();
		if(playlisttrackexists == null)
		{
			//if empty, generate the next track number if not 
			tracknumber = PlaylistTracks
								.Where(x => (x.Playlist.Name.Equals(playlistname)
									&& x.Playlist.UserName.Equals(username)
									))
									.Count();

			tracknumber++;
		}
		else
		{
			var songname = Tracks
								.Where(x => x.TrackId == trackid)
								.Select(x => x.Name)
								.SingleOrDefault();
			throw new ArgumentNullException($"Selected track ({songname}) already exist on the playlist.");
		}
	}
	
	//for processing to stage the new track to the playlist
	
	playlisttrackexists = new PlaylistTracks();
	
	// then load the data to the new instance of plalist track
	
	playlisttrackexists.TrackNumber = tracknumber;
	playlisttrackexists.TrackId = trackid;
	
	
	/******************************
	?? What about the second part of the primary key: PlaylistID
	
	If the playlist exists, then we know the id:
	playlistexists.playlistId;
	
	In the situation of a NEW playlist, even though we have created the playlist instance(see above), it is ONLY staged!!!
	
	This means that the actual SQL record has NOT yet been created. This means that the identity value for the new playlist DOES NOT yet exists. The value on the playlist instance(playlistexists)is zero. Thus, we have a serious problem.
	
	Solution
	
	It is built into entity framework software and it is based on using the navigational property in Playlists pointing to a "child".
	
	Staging a typical Add is the past was to reference the entity and use the entity.Add(xxxx)
	_context.PlaylistTrackAdd(xxxx) [remember, the _context. is context instance in the VS. ie when we move to VS, we have to add _context to our references].
	
	If you use this statement, the playlistid would be zero(0), causing your transaction to ABORT.
	
	INSTEAD, do the staging, using the syntax of "parent.navigationalproperty.Add(xxxx)"
	
	playlistexists will be filled with either senario A) a new  staged instance or senario B) a copy of the existing playlist instance.
	
	******************************/
	
	playlistExists.PlaylistTracks.Add(playlisttrackexists); //this line forces the parent table to create a primary key(PlaylistId) that will automatically be linked to the new playlisttrackexists
	
	/******************************
	Staging is complete
	Commit the work (transaction)
	committting the work needs a .SaveChanges()
	a transaction needs only one  .SaveChanges()
	
	IF the SaveChanges() fails, then all staged work being handled by the SaveChanges is rollback.
	
	*******************************/
	
	SaveChanges();

}


public void PlaylistTrack_RemovePlaylist(string playlistname, string username, List<PlaylistTrackTRX>trackListInfo)
{
	//local variables
	Playlists playlistexists = null;
	
	
	if (string.IsNullOrWhiteSpace(playlistname))
	{
		throw new ArgumentNullException("No playlist name submitted");
	}

	if (string.IsNullOrWhiteSpace(username))
	{
		throw new ArgumentNullException("No user name submitted");
	}
	
	var count = trackListInfo.Count();
	if (count == 0)
	{
		throw new ArgumentNullException("No list of tracks were submitted");
	}


	playlistexists = Playlists
						.Where(x => x.Name.Equals(playlistname) && x.UserName.Equals(username))
						.Select(x => x)
						.FirstOrDefault();
	if (playlistexists == null)
	{
		throw new ArgumentNullException($"Play list {playlistname} does not exist for this user.");
	}
	
	

}

#endregion

#region Queries and command models

public class TrackSelection

{	public int TrackId { get; set; }
	public string SongName { get; set; }
	public string AlbumTitle { get; set; }
	public string ArtistName { get; set; }
	public int Milliseconds { get; set; }
	public decimal Price { get; set; }
}

public class PlaylistTrackInfo
{

	public int TrackId { get; set; }
	public int TrackNumber { get; set; }
	public string SongName { get; set; }
	public int Milliseconds { get; set; }
}

public class PlaylistTrackTRX
{
	public bool SelectedTrack { get; set; }
	public int TrackId { get; set; }
	public int TrackNumber { get; set; }
	public int TrackInput { get; set; }
}


#endregion



//this is a general method to drill down intoan exception to obtain the innerException where your 
//actual error is detailed.

private Exception GetInnerException(Exception ex)
{
	while(ex.InnerException != null)
	
		ex = ex.InnerException;
		return ex;
	
}