﻿namespace GraphML.Interfaces
{
  public interface IContactDatastore : IOwnedDatastore<Contact>
  {
    Contact ByEmail(string email);
  }
}