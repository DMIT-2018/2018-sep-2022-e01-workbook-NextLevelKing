<Query Kind="Expression">
  <Connection>
    <ID>54bf9502-9daf-4093-88e8-7177c12aaaaa</ID>
    <NamingService>2</NamingService>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\ChinookDemoDb.sqlite</AttachFileName>
    <DisplayName>Demo database (SQLite)</DisplayName>
    <DriverData>
      <PreserveNumeric1>true</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.Sqlite</EFProvider>
      <MapSQLiteDateTimes>true</MapSQLiteDateTimes>
      <MapSQLiteBooleans>true</MapSQLiteBooleans>
    </DriverData>
  </Connection>
</Query>

//Sorting

//There is a significant difference between a query sytax and method syntax.

//Query syntax is much like SQL
//so you will have 
//  orderby field {[ascending] | [descending]} [, field....]

// ascending is the default option

//method syntax is a series of individual methods

// .OrderBy(x => x.field)
//.OrderByDescending(x => x.field)

//for multiple filed, you satrt using/ or add 
//.ThenBy(x => x.field) each following field
//.ThenByDescending(x => x.field) each following field


//For example Let us find all the album tracks for band Queen. Order the track by track name alphabetically.

//query syntax method.

from x in Tracks
where x.Album.Artist.Name.Contains("Queen")
orderby x.AlbumId, x.Name //means first sort by albumid then the track name.
select x


//method syntax - select is optional here

Tracks
	.Where (x => x.Album.Artist.Name.Contains("Queen"))
	.OrderBy(x => x.AlbumId)
	.ThenBy(x => x.Name)
	
	// let us try sorting by Album Name
	
Tracks
	.Where (x => x.Album.Artist.Name.Contains("Queen"))
	.OrderBy(x => x.Album.Title)
	.ThenBy(x => x.Name)
	

//order of sorting and filtering can be interchanged.	
// Sorting before filtering and filtering before sorting will give more same result but time it takes varies.
Tracks
	.OrderBy(x => x.Album.Title)
	.ThenBy(x => x.Name)
	.Where (x => x.Album.Artist.Name.Contains("Queen"))
	
//ThenBy can only follow OrderBy.
Tracks
	.OrderBy(x => x.Album.Title)
	.Where (x => x.Album.Artist.Name.Contains("Queen"))
	.ThenBy(x => x.Name)