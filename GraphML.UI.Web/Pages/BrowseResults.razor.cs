﻿using System;
using System.Threading.Tasks;
using GraphML.Analysis;
using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseResults
  {
    #region Parameters

    [Parameter]
    public string OrganisationName { get; set; }

    [Parameter]
    public string OrganisationId { get; set; }

    [Parameter]
    public string RepositoryManagerName { get; set; }

    [Parameter]
    public string RepositoryManagerId { get; set; }

    [Parameter]
    public string RepositoryName { get; set; }

    [Parameter]
    public string RepositoryId { get; set; }

    [Parameter]
    public string GraphName { get; set; }

    [Parameter]
    public string GraphId { get; set; }

    #endregion

    private RequestBase[] _requests;

    private bool _deleteDialogIsOpen;
    private RequestBase _deleteResult;

    private void ConfirmDeleteResult(RequestBase item)
    {
      _deleteResult = item;
      _deleteDialogIsOpen = true;
    }

    private async Task DeleteResult()
    {
      _deleteDialogIsOpen = false;
      await _resultServer.Delete(_deleteResult.CorrelationId);
      StateHasChanged();
    }

    private void OnShowResult(Guid correlationId)
    {
      _navMgr.NavigateTo($"/ShowResult/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{correlationId}");
    }
  }
}
