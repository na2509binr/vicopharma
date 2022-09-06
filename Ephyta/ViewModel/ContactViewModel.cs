using Ephyta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ephyta.ViewModel
{
    public class ListFeedbackViewModel
    {
        public PagedList.IPagedList<Feedback> Feedbacks { get; set; }
        public string Name { get; set; }
    }
    public class ListContactViewModel
    {
        public PagedList.IPagedList<Contact> Contacts { get; set; }
        public string Name { get; set; }
    }
    public class ListRegisterViewModel
    {
        public PagedList.IPagedList<Register> Registers { get; set; }
        public string Phone { get; set; }
    }
}