﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Requisition.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          03-15-2022 19:54
// Last Updated On:     03-15-2022 21:12
// *****************************************/

#endregion

#region Using

using ProfSvc_AppTrack.Pages.Admin;
using System.Xml.Linq;

using ProfSvc_Classes;

using ChangeEventArgs = Microsoft.AspNetCore.Components.ChangeEventArgs;
using SelectEventArgs = Syncfusion.Blazor.Navigations.SelectEventArgs;

#endregion

namespace ProfSvc_AppTrack.Pages;

public partial class Requisition
{
    private readonly List<IntValues> _showRecords =
        new() {new(10, "10 rows"), new(25, "25 rows"), new(50, "50 rows"), new(75, "75 rows"), new(100, "100 rows")};

    private int _currentPage = 1;
    private int _selectedTab;

    private List<IntValues> _states;

    private readonly List<IntValues> _statesCopy = new();

    public static int PageCount
    {
        get;
        set;
    }

    private AutoCompleteButton AutoCompleteControl
    {
        get;
        set;
    }

    private static SfGrid<Requisitions> Grid
    {
        get;
        set;
    }

    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    private static RequisitionSearch SearchModel
    {
        get;
    } = new();

    private bool VisibleNewCandidate
    {
        get;
        set;
    }

    public string SortField
    {
        get;
        set;
    }

    public SortDirection SortDirectionProperty
    {
        get;
        set;
    }

    public static int StartRecord
    {
        get;
        set;
    }

    public static int EndRecord
    {
        get;
        set;
    }

    public static int Count
    {
        get;
        set;
    }

    private void AddNewCandidate()
    {
        VisibleNewCandidate = true;
    }

    private async Task AdvancedSearch(MouseEventArgs arg)
    {
        await Task.Delay(1);
    }

    private async Task ChangeItemCount(ChangeEventArgs<int, IntValues> obj)
    {
        _currentPage = 1;
        SearchModel.Page = _currentPage;
        SearchModel.ItemCount = obj.Value;

        await LocalStorageBlazored.SetItemAsync("RequisitionGrid", SearchModel);
        Grid.Refresh();
        StateHasChanged();
    }

    private bool FirstRender
    {
        get;
        set;
    } = true;

    private async Task DataHandler(object obj)
    {
        DotNetObjectReference<Requisition> _dotNetReference = DotNetObjectReference.Create(this); // create dotnet ref
        await _runtime.InvokeAsync<string>("detail", _dotNetReference);
        //  send the dotnet ref to JS side
        FirstRender = false;
        //Count = Count;
        await Grid.SelectRowAsync(0);
    }

    private Requisitions _target;

    private SfSpinner Spinner
    {
        get;
        set;
    } = new();

    private RequisitionDetails _requisitionDetailsObject = new();
    private RequisitionDetails _requisitionDetailsObjectClone = new();

    private async Task DetailDataBind(DetailDataBoundEventArgs<Requisitions> requisition)
    {
        //VisibleProperty = true;

        if (_target != null)
        {
            if (_target != requisition.Data) // return when target is equal to args.data
            {
                await Grid.ExpandCollapseDetailRowAsync(_target);
            }
        }

        _target = requisition.Data;

        //_requisitionDetailsObject.ClearData();
        await Task.Delay(1);
        await Spinner.ShowAsync();
        //await Task.Delay(1000);
        RestClient _restClient = new($"{Start.ApiHost}");
        RestRequest request = new("Requisition/GetRequisitionDetails");
        request.AddQueryParameter("requisitionID", _target.ID);

        Dictionary<string, object> _restResponse = await _restClient.GetAsync<Dictionary<string, object>>(request);

        if (_restResponse != null)
        {
            _requisitionDetailsObject = JsonConvert.DeserializeObject<RequisitionDetails>(_restResponse["Requisition"]?.ToString() ?? string.Empty);
            //_candidateSkillsObject = General.DeserializeObject<List<CandidateSkills>>(_restResponse["Skills"]);
            //_candidateEducationObject = General.DeserializeObject<List<CandidateEducation>>(_restResponse["Education"]);
            //_candidateExperienceObject = General.DeserializeObject<List<CandidateExperience>>(_restResponse["Experience"]);
            //_candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_restResponse["Activity"]);
            //_candidateNotesObject = General.DeserializeObject<List<CandidateNotes>>(_restResponse["Notes"]);
            //_candidateRatingObject = General.DeserializeObject<List<CandidateRating>>(_restResponse["Rating"]);
            //_candidateMPCObject = General.DeserializeObject<List<CandidateMPC>>(_restResponse["MPC"]);
            //_candidateDocumentsObject = General.DeserializeObject<List<CandidateDocument>>(_restResponse["Document"]);
            //RatingMPC = JsonConvert.DeserializeObject<CandidateRatingMPC>(_restResponse["RatingMPC"]?.ToString() ?? string.Empty);
            //GetMPCDate();
            //GetMPCNote();
            //GetRatingDate();
            //GetRatingNote();
            //SetupAddress();
            //SetCommunication();
            //SetEligibility();
            //SetJobOption();
            //SetTaxTerm();
            //SetExperience();
        }

        //_selectedTab = _candidateActivityObject.Count > 0 ? 7 : 0;

        await Task.Delay(1);
        await Spinner.HideAsync();
    }

    private async Task FilterGrid(ChangeEventArgs<string, KeyValues> arg)
    {
        await Task.Delay(1);
    }

    private async Task FirstClick()
    {
        if (_currentPage < 1)
        {
            _currentPage = 1;
        }

        _currentPage = 1;
        SearchModel.Page = _currentPage;
        await LocalStorageBlazored.SetItemAsync("RequisitionGrid", SearchModel);
        //_ = new StorageCompression(SessionStorage).SetRequisitionGrid();
        Grid.Refresh();
    }

    private async Task LastClick()
    {
        if (_currentPage < 1)
        {
            _currentPage = 1;
        }

        _currentPage = PageCount.ToInt32();
        SearchModel.Page = _currentPage;
        await LocalStorageBlazored.SetItemAsync("RequisitionGrid", SearchModel);
        //_ = new StorageCompression(SessionStorage).SetRequisitionGrid();
        Grid.Refresh();
    }

    private async Task NextClick()
    {
        if (_currentPage < 1)
        {
            _currentPage = 1;
        }

        _currentPage = SearchModel.Page >= PageCount.ToInt32() ? PageCount.ToInt32() : SearchModel.Page + 1;
        SearchModel.Page = _currentPage;
        await LocalStorageBlazored.SetItemAsync("RequisitionGrid", SearchModel);
        //_ = new StorageCompression(SessionStorage).SetRequisitionGrid();
        Grid.Refresh();
    }

    private async Task OnActionBegin(ActionEventArgs<Requisitions> arg)
    {
        await Task.Delay(1);
    }

    private async Task OnActionComplete(ActionEventArgs<Requisitions> arg)
    {
        await Task.Delay(1);
    }

    private async Task PageNumberChanged(ChangeEventArgs obj)
    {
        decimal _currentValue = obj.Value.ToDecimal();
        if (_currentValue < 1)
        {
            _currentPage = 1;
        }
        else if (_currentValue > PageCount)
        {
            _currentPage = PageCount.ToInt32();
        }

        SearchModel.Page = _currentPage;
        await LocalStorageBlazored.SetItemAsync("RequisitionGrid", SearchModel);
        //_ = new StorageCompression(SessionStorage).SetRequisitionGrid();
        Grid.Refresh();
    }

    private async Task PreviousClick()
    {
        if (_currentPage < 1)
        {
            _currentPage = 1;
        }

        _currentPage = SearchModel.Page <= 1 ? 1 : SearchModel.Page - 1;
        SearchModel.Page = _currentPage;
        await LocalStorageBlazored.SetItemAsync("RequisitionGrid", SearchModel);
        //_ = new StorageCompression(SessionStorage).SetRequisitionGrid();
        Grid.Refresh();
    }

    private static void RefreshGrid() => Grid.Refresh();

    public class AdminRequisitionDropDownAdaptor : DataAdaptor
    {
        #region Methods

        public override Task<object> ReadAsync(DataManagerRequest dm, string key = null) => General.GetAutocompleteAsync("SearchCandidate", "@Candidate", dm);

        #endregion
    }

    public class AdminRequisitionAdaptor : DataAdaptor
    {
        #region Methods

        /// <summary>Performs data Read operation synchronously.</summary>
        public override Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            Task<object> _requisitionReturn = General.GetRequisitionReadAdaptor(SearchModel, dm);
            //Count = ((DataResult)_candidateReturn.Result).Count;
            Grid.SelectRowAsync(0);
            return _requisitionReturn;
        }

        #endregion
    }

    private void SetAlphabet(string alphabet)
    {
        return;
    }

    private async Task AllAlphabet(MouseEventArgs arg)
    {
        await Task.Delay(1);
    }

    private async Task ClearFilter(MouseEventArgs arg)
    {
        await Task.Delay(1);
    }

    private async Task TabSelected(SelectEventArgs arg)
    {
        await Task.Delay(1);
    }

    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(1);
        LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
        IMemoryCache _memoryCache = Start.MemCache;
        while (_states == null)
        {
            _memoryCache.TryGetValue("States", out _states);
        }

        _statesCopy.Clear();
        _statesCopy.Add(new(0, "All"));
        _statesCopy.AddRange(_states);
        /*while (_eligibility == null)
        {
            _memoryCache.TryGetValue("Eligibility", out _eligibility);
        }

        _eligibilityCopy.Clear();
        _eligibilityCopy.Add(new(0, "All"));
        _eligibilityCopy.AddRange(_eligibility);

        _memoryCache.TryGetValue("Experience", out _experience);
        _memoryCache.TryGetValue("TaxTerms", out _taxTerms);
        while (_jobOptions == null)
        {
            _memoryCache.TryGetValue("JobOptions", out _jobOptions);
        }

        _jobOptionsCopy.Clear();
        _jobOptionsCopy.Add(new("%", "All"));
        _jobOptionsCopy.AddRange(_jobOptions);

        _memoryCache.TryGetValue("StatusCodes", out _statusCodes);
        _memoryCache.TryGetValue("Workflow", out _workflows);
        _memoryCache.TryGetValue("Communication", out _communication);
        _memoryCache.TryGetValue("DocumentTypes", out _documentTypes);

        string _cookyString = await LocalStorageBlazored.GetItemAsync<string>("CandidateGrid");
        if (!_cookyString.NullOrWhiteSpace())
        {
            SearchModel = JsonConvert.DeserializeObject<CandidateSearch>(_cookyString);
        }
        else
        {
            await LocalStorageBlazored.SetItemAsync("CandidateGrid", SearchModel);
        }*/

        await base.OnInitializedAsync();
    }
}