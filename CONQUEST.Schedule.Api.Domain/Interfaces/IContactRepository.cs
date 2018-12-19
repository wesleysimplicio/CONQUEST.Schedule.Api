using CONQUEST.Schedule.Api.Domain.Models;
using System;
using System.Collections.Generic;

namespace CONQUEST.Schedule.Api.Domain.Interfaces
{
    public interface IContactRepository : IDisposable
    {
        List<Contact> Get();
        Contact GetById(string Id);
    }
}
