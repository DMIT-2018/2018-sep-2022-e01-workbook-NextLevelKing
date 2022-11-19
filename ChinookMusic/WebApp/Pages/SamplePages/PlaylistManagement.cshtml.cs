using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional 
using ChinookSystem.ViewModels;
using WebApp.Helpers;
using ChinookSystem.BLL;
#endregion


namespace WebApp.Pages.SamplePages
{

    public class PlaylistManagementModel : PageModel
    {
        #region Private variables and DI constructor
        private readonly TrackServices _trackServices;
        private readonly PlaylistTrackServices _playlisttrackServices;


        public PlaylistManagementModel(TrackServices trackservices,
                                PlaylistTrackServices playlisttrackservices)
        {
            _trackServices = trackservices;
            _playlisttrackServices = playlisttrackservices;
        }
        #endregion

        #region Messaging and Error Handling
        [TempData]
        public string FeedBackMessage { get; set; }
        
        public string ErrorMessage { get; set; }

        //a get property that returns the result of the lamda action
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBackMessage);

        //used to display any collection of errors on web page
        //whether errors are generated locally or come from the class library service methods
        public List<string> ErrorDetails { get; set; } = new();

        //PageModel local error list for collection 
        public List<Exception> Errors { get; set; } = new();

        #endregion

        #region Paginator
        private const int PAGE_SIZE = 5;
        public Paginator Pager { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? currentpage { get; set; }   
        #endregion

        [BindProperty(SupportsGet = true)] //the support get allows us to send something with page redirect
        public string searchBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string searchArg { get; set; }

        [BindProperty(SupportsGet = true)]
        public string playlistname { get; set; }

        public List<TrackSelection> trackInfo { get; set; } //these variables are used by the query to display things, so they do not need bindproperty

        public List<PlaylistTrackInfo> qplaylistInfo { get; set; } //remember, bindproperty is two ways.  This property will be tied to the input field of the web page. in this case, this field is tied to the table data elements for the playlist. This is an outgoing property so we do not need to make it a bindproperty.

        [BindProperty]
        public List<PlaylistTrackTRX> cplaylistInfo { get; set; } = new();// input collection. This property is tied to the form input element located on each of the rows of the track table.
                                                                          // It will hold the track id one wishes to add to the playlist.This is an incoming property,
                                                                          // it will pull data coming in so it needs to be a bindproperty
                                                                          //the  =new() is required to retain values during the an error............see the page

        [BindProperty]
        public int addtrackid { get; set; }

        public const string USERNAME = "HansenB"; // we are using this until we do the security part.
        public void OnGet()  //this method is executed everytime the page is called for the first time or whenever
                             //a Get is method to the page SUCH as RedirectToPage().
        {
            GetTrackInfo();
            GetPlaylist();
        }

        public void GetTrackInfo()
        {
            if (!string.IsNullOrWhiteSpace(searchArg) &&
                            !string.IsNullOrWhiteSpace(searchBy))
            {
                int totalcount = 0;
                int pagenumber = currentpage.HasValue ? currentpage.Value : 1;
                PageState current = new(pagenumber, PAGE_SIZE);
                trackInfo = _trackServices.Track_FetchTracksBy(searchArg.Trim(),
                    searchBy.Trim(), pagenumber, PAGE_SIZE, out totalcount);
                Pager = new(totalcount, current);
            }
        }

        public void GetPlaylist()
        {
            if (!string.IsNullOrWhiteSpace(playlistname))
            {
                string username = USERNAME;
                qplaylistInfo = _playlisttrackServices.PlaylistTrack_FetchPlaylist(playlistname.Trim(), username);
            }
        }
        public IActionResult OnPostTrackSearch() 
        {
            //when we have IAction request, we must have a redirect to page or return page
            //we use redirect when we want to go back to on get but if we want
            //to redisplay the same page we are on say incase of error, we use Page()
            //redirecttopage only displays what we get from on get method.
            //onget wipes everything clean
            try
            {
                if (string.IsNullOrWhiteSpace(searchBy))
                {
                    Errors.Add(new Exception("Track search type not selected"));
                }
                if (string.IsNullOrWhiteSpace(searchArg))
                {
                    Errors.Add(new Exception("Track search string not entered"));
                }
                if (Errors.Any())
                {
                    throw new AggregateException(Errors);
                }
                //RedirecToPage() will cause an Get request to be issued (OnGet())
                return RedirectToPage(new
                {
                    searchBy = searchBy.Trim(),
                    searchArg = searchArg.Trim(),
                    playlistname = string.IsNullOrWhiteSpace(playlistname) ? " " : playlistname.Trim()
                });
            }
            catch (AggregateException ex)
            {
                ErrorMessage = "Unable to process search";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);
                    
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                return Page();
            }
        }

        public IActionResult OnPostFetch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("Enter a playlist name to fetch.");
                }
                return RedirectToPage(new
                {
                    searchBy = string.IsNullOrWhiteSpace(searchBy) ? " " : searchBy.Trim(),
                    searchArg = string.IsNullOrWhiteSpace(searchArg) ? " " : searchArg.Trim(),
                    playlistname = playlistname.Trim()
                });
            }
            catch(Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                return Page();

            }
        }

        public IActionResult OnPostAddTrack()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("You need to have a playlist select first. Enter a playlist name and Fetch");
                }

                // Add the code to add a track via the service.
                //the data needed for your call has already been placed in your local propertiesby the use of [BindProperty] which is two way(output/input)
                //once security is installed, you would be able to obtain the user name from the operating system

                string username = USERNAME;
                //call your service sending in the expected data
                _playlisttrackServices.PlaylistTrack_AddTrack(playlistname, username, addtrackid);
                FeedBackMessage = "adding the track";
                return RedirectToPage(new
                {
                    searchby = searchBy,
                    searcharg = searchArg,
                    playlistname = playlistname
                });
            }
            catch (AggregateException ex)
            {
                              
                ErrorMessage = "Unable to process add track";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }

                //since the onGet() will no be called if there is a transaction error,
                //the catch MUST do the actions of the OnGet.
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            
        }

        public IActionResult OnPostRemove()
        {
            try
            {
               //Add the code to process the list of tracks via the service.
               if(string .IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("You need to have a playlist selected first. Enter a playlistname and press Fetch.");
                }

                int oneselection = cplaylistInfo
                                     .Where(x => x.SelectedTrack)
                                     .Count();
                if(oneselection == 0)
                {
                    throw new Exception("You need to first select one track to delete before pressing Remove.");
                }
                string username = USERNAME;
                //send data to the service
                _playlisttrackServices.PlaylistTrack_RemoveTracks(playlistname, username, cplaylistInfo);

                //success
                FeedBackMessage = "Tracks have been removed";
                return RedirectToPage(new
                {
                    searchBy = string.IsNullOrWhiteSpace(searchBy) ? " " : searchBy.Trim(),
                    searchArg = string.IsNullOrWhiteSpace(searchArg) ? " " : searchArg.Trim(),
                    playlistname = playlistname
                });
            }
            catch (AggregateException ex)
            {

                ErrorMessage = "Unable to process remove tracks";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }

        }


        public IActionResult OnPostReOrg()
        {
            try
            {
                //Add the code to process the list of tracks via the service.
                if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("You need to have a playlist selected first. Enter a playlistname and press Fetch.");
                }

               
                string username = USERNAME;
                //send data to the service
                _playlisttrackServices.PlaylistTrack_MoveTracks(playlistname, username, cplaylistInfo);

                //success
                FeedBackMessage = "Tracks have been re-organized";
                return RedirectToPage(new
                {
                    searchBy = string.IsNullOrWhiteSpace(searchBy) ? " " : searchBy.Trim(),
                    searchArg = string.IsNullOrWhiteSpace(searchArg) ? " " : searchArg.Trim(),
                    playlistname = playlistname
                });
            }
            catch (AggregateException ex)
            {

                ErrorMessage = "Unable to process re-organize tracks";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }

        }
        private Exception GetInnerException(Exception ex)
        {
            while(ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
    }
}
