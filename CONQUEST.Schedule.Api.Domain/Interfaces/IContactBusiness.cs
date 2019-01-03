using CONQUEST.Schedule.Api.Domain.Models;
using System;
using System.Collections.Generic;

namespace CONQUEST.Schedule.Api.Domain.Interfaces
{
    public interface IContactBusiness : IDisposable
    {
        List<Contact> Get();
        Contact GetById(string Code);
        bool Insert(Contact contact);
        bool Update(Contact contact);
        bool Delete(string Code);
    }
}
