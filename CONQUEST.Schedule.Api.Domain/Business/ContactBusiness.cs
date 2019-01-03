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

        public bool Insert(Contact contact)
        {
            this._contactRepository.Insert(contact);
            return true;
        }

        public bool Update(Contact contact)
        {
            return this._contactRepository.Update(contact) > 0;
        }

        public void Dispose()
        {
        }
    }


}
