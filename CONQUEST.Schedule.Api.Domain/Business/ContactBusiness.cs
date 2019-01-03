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

        public Contact GetById(string Code)
        {
            return this._contactRepository.GetById(Code);
        }

        public bool Insert(Contact contact)
        {
            this._contactRepository.Insert(contact);
            return true;
        }

        public bool Update(Contact contact)
        {

            if (string.IsNullOrWhiteSpace(contact.Name)
                || string.IsNullOrWhiteSpace(contact.Email)
                || string.IsNullOrWhiteSpace(contact.Telephone))
            {
                return false;
            }

            return this._contactRepository.Update(contact) > 0;
        }

        public bool Delete(string Code)
        {
            if (string.IsNullOrWhiteSpace(Code))
                return false;

            var dbUser = this._contactRepository.GetById(Code);
            if (dbUser == null)
            {
                throw new Exception("Este contato não existe");
            }
            
            return this._contactRepository.Delete(Code) > 0;
        }

        public void Dispose()
        {
        }
    }


}
