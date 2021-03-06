﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class DomainTests : TestBase
    {
        [TestMethod]
        public void GetContacts_ReturnContactInfoForAllUsers()
        {
            DomainContactsResult contacts = _api.Domains.GetContacts(_domainName);

            // TODO: use different names for each type of contact to distinguish between them in testing
            Assert.AreEqual(contacts.Admin.FirstName, TestUserFirstName);
            Assert.AreEqual(contacts.Admin.LastName, TestUserLastName);
            Assert.AreEqual(contacts.AuxBilling.FirstName, TestUserFirstName);
            Assert.AreEqual(contacts.AuxBilling.LastName, TestUserLastName);
            Assert.AreEqual(contacts.Registrant.FirstName, TestUserFirstName);
            Assert.AreEqual(contacts.Registrant.LastName, TestUserLastName);
            Assert.AreEqual(contacts.Tech.FirstName, TestUserFirstName);
            Assert.AreEqual(contacts.Tech.LastName, TestUserLastName);
        }

        [TestMethod]
        public void GetInfo_ReturnsInformationOnExistingDomain()
        {
            DomainInfoResult info = _api.Domains.GetInfo(_domainName);
            Assert.IsTrue(info.ID > 0);
        }

        [TestMethod]
        public void GetList_ShouldContainTheTestDomain()
        {
            DomainListResult result = _api.Domains.GetList();
            Assert.IsTrue(result.Domains.Length > 0);
            Assert.IsTrue(result.Domains.Any(d => string.Equals(d.Name, _domainName)));
        }

        [TestMethod, Ignore("Needs work - can only renew a domain so many times")]
        public void Test_renew()
        {
            var result = _api.Domains.Renew(_domainName, 1);

            Assert.AreEqual(result.DomainName, _domainName);
            Assert.IsTrue(result.DomainID > 0);
            Assert.AreEqual(result.Renew, true);
            Assert.IsTrue(result.OrderID > 0);
            Assert.IsTrue(result.TransactionID > 0);
            Assert.IsTrue(result.ChargedAmount > 0);
        }

        [TestMethod, Ignore("Needs work - can only reactivate an expired domain")]
        public void Test_reactivate()
        {
            var result = _api.Domains.Reactivate(_domainName);

            Assert.AreEqual(result.DomainName, _domainName);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsTrue(result.OrderID > 0);
            Assert.IsTrue(result.TransactionID > 0);
            Assert.IsTrue(result.ChargedAmount > 0);
        }

        [TestMethod, Ignore("Needs work - should unset the registrar lock in order to validate the method sets it")]
        public void Test_set_registrar_lock()
        {
            _api.Domains.SetRegistrarLock(_domainName);
        }

        [TestMethod, Ignore("Need work - should set the value of the lock before executing the test to make sure it gets the proper value")]
        public void Test_get_registrar_lock()
        {
            bool isLocked = _api.Domains.GetRegistrarLock(_domainName);
            Assert.IsTrue(isLocked);
        }

        [TestMethod, Ignore("Needs work - should validate that the value of the contact was set")]
        public void Test_set_Contacts()
        {
            var contact = new ContactInformation()
            {
                Address1 = "1 never never land",
                City = "New York",
                Country = "US",
                EmailAddress = "noreply@example.com",
                FirstName = "Billy",
                LastName = "Bob",
                Phone = "+011.5555555555",
                PostalCode = "l5Z5Z5",
                StateProvince = "California"
            };

            _api.Domains.SetContacts(new DomainContactsRequest()
            {
                DomainName = _domainName,
                Admin = contact,
                AuxBilling = contact,
                Registrant = contact,
                Tech = contact
            });
        }
    }
}
