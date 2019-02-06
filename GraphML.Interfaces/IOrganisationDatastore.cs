﻿using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOrganisationDatastore : IDatastore<Organisation>
  {
    IEnumerable<Organisation> GetAll();
  }
}
