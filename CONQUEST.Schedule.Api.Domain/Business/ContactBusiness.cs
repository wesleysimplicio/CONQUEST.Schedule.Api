using CONQUEST.Schedule.Api.Domain.Interfaces;
using CONQUEST.Schedule.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CONQUEST.Schedule.Api.Domain.Business
{
    public class ContactBusiness : IContactBusiness
    {
        private readonly IContactRepository _contactRepository;

        public ContactBusiness(
           IContactRepository contactRepository)
        {
            this._contactRepository = contactRepository;
        }
        public List<Contact> Get()
        {
            return this._contactRepository.Get();
        }

        public Contact GetById(string Id)
        {
            return this._contactRepository.GetById(Id);
        }

        public void Dispose()
        {
        }
    }


}
