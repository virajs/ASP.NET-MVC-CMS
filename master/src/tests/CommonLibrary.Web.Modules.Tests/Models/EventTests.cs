using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using System.Linq;
using System.Data;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Modules.Events;
using ComLib.Web.Modules.Categorys;

namespace CommonLibrary.Tests
{   
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void CanHandleFreeCostText()
        {
            Event ev = new Event();
            AreEqual(() => ev.Costs = "0", () => CostType.Free, () => ev.Cost);
            AreEqual(() => ev.Costs = "free", () => CostType.Free, () => ev.Cost);
            AreEqual(() => ev.Costs = "none", () => CostType.Free, () => ev.Cost);
            AreEqual(() => ev.Costs = "zero", () => CostType.Free, () => ev.Cost);
            AreEqual(() => ev.Costs = "na", () => CostType.Free, () => ev.Cost);
            AreEqual(() => ev.Costs = "N/A", () => CostType.Free, () => ev.Cost);
            AreEqual(() => ev.Costs = "NOTHING", () => CostType.Free, () => ev.Cost);
            AreEqual(() => ev.Costs = "$0", () => CostType.Free, () => ev.Cost);
        }


        [Test]
        public void CanHandleDontKnowCostText()
        {
            Event ev = new Event();
            AreEqual(() => ev.Costs = "do not know", () => CostType.DoNotKnow, () => ev.Cost);
            AreEqual(() => ev.Costs = "UNKNOWN", () => CostType.DoNotKnow, () => ev.Cost);
            AreEqual(() => ev.Costs = "don't know", () => CostType.DoNotKnow, () => ev.Cost);
        }


        [Test]
        public void CanHandleVariesCostText()
        {
            Event ev = new Event();
            AreEqual(() => ev.Costs = "varies", () => CostType.Varies, () => ev.Cost);
            AreEqual(() => ev.Costs = "differs", () => CostType.Varies, () => ev.Cost);
            AreEqual(() => ev.Costs = "DEPENDS", () => CostType.Varies, () => ev.Cost);
        }


        [Test]
        public void CanMassageDataOnBeforeSave()
        {
            IList<Category> categories = new List<Category>();
            categories.Add(new Category() { Id = 1, Name = "Art",       Group = "Event" });
            categories.Add(new Category() { Id = 2, Name = "Painting",  Group = "Event", ParentTitle = "Art" });
            categories.Add(new Category() { Id = 3, Name = "Oil",       Group = "Event", ParentTitle = "Art" });
            categories.Add(new Category() { Id = 4, Name = "Water",     Group = "Event", ParentTitle = "Art" });
            categories.Add(new Category() { Id = 5, Name = "Sports",    Group = "Event", });
            categories.Add(new Category() { Id = 6, Name = "Baseball",  Group = "Event", ParentTitle = "Sports" });
            categories.Add(new Category() { Id = 7, Name = "Football",  Group = "Event", ParentTitle = "Sports" });
            Category.Init(new RepositoryInMemory<Category>(), false);
            Category.Create(categories, true, c => c.Name, c => c.ParentId);

            Event.Init(new RepositoryInMemory<Event>("Id,CreateUser,Title,StartDate"), false);
            Event ev = new Event() { Title = "Learn", Description = "testing", CategoryName = "C#", Content = "testing", Times = "9am-10am", StartDate = DateTime.Today, EndDate = DateTime.Today };
            Event.Create(ev);

        }


        [Test]
        public void CanHandleAgeRangeText()
        {
            Event ev = new Event();
            ev.Ages = "";
            Assert.IsFalse(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 0);
            Assert.AreEqual(ev.AgeTo, 0);


            ev.Ages = "babies";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 0);
            Assert.AreEqual(ev.AgeTo, 4);
            Assert.AreEqual(ev.Ages, "babies");

            ev.Ages = "kids";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 5);
            Assert.AreEqual(ev.AgeTo, 12);
            Assert.AreEqual(ev.Ages, "kids");

            ev.Ages = "teens";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 13);
            Assert.AreEqual(ev.AgeTo, 17);
            Assert.AreEqual(ev.Ages, "teens");

            ev.Ages = "youngadults";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 18);
            Assert.AreEqual(ev.AgeTo, 22);
            Assert.AreEqual(ev.Ages, "youngadults");

            ev.Ages = "adults";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 23);
            Assert.AreEqual(ev.AgeTo, 64);
            Assert.AreEqual(ev.Ages, "adults");

            ev.Ages = "seniors";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 65);
            Assert.AreEqual(ev.AgeTo, 100);
            Assert.AreEqual(ev.Ages, "seniors");

            ev.Ages = "18 and up";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 18);
            Assert.AreEqual(ev.AgeTo, 100);

            ev.Ages = "ANY";
            Assert.IsFalse(ev.IsAgeApplicable);

            ev.Ages = "21 and over";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 21);
            Assert.AreEqual(ev.AgeTo, 100);

            ev.Ages = "10 and under";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 0);
            Assert.AreEqual(ev.AgeTo, 10);
            
            ev.Ages = "13 and below";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 0);
            Assert.AreEqual(ev.AgeTo, 13);

            ev.Ages = "14 to 18";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 14);
            Assert.AreEqual(ev.AgeTo, 18);

            ev.Ages = "14 - 18";
            Assert.IsTrue(ev.IsAgeApplicable);
            Assert.AreEqual(ev.AgeFrom, 14);
            Assert.AreEqual(ev.AgeTo, 18);
        }        


        private void AreEqual( Action action, Func<object> expected, Func<object> actual)
        {
            action();
            object expectedval = expected();
            object actualval = actual();
            Assert.AreEqual(expectedval, actualval);
        }



       
    }
}
