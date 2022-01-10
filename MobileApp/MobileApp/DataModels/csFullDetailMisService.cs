using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.DataModels
{
    class csFullDetailMisService
    { 
        private static string id, ItemName, Quantity, DateOfRequest, Purpose, Status, Options, Note, RentDate, EndRentDate, Deadline;

        public void setData(string id, string ItemName, string Quantity, string DateOfRequest, string Purpose,
            string Status, string Options, string Note, string RentDate, string EndRentDate, string Deadline)
        {
            csFullDetailMisService.id = id;
            csFullDetailMisService.ItemName = ItemName;
            csFullDetailMisService.Quantity = Quantity;
            csFullDetailMisService.DateOfRequest = DateOfRequest;
            csFullDetailMisService.Purpose = Purpose;
            csFullDetailMisService.Status = Status;
            csFullDetailMisService.Options = Options;
            csFullDetailMisService.Note = Note;
            csFullDetailMisService.RentDate = RentDate;
            csFullDetailMisService.EndRentDate = EndRentDate;
            csFullDetailMisService.Deadline = Deadline;
        }

        public String getid() { return id; }
        public String getItemName() { return ItemName; }
        public String getQuantity() { return Quantity; }
        public String getDateOfRequest() { return DateOfRequest; }
        public String getPurpose() { return Purpose; }
        public String getStatus() { return Status; }
        public String getOptions() { return Options; }
        public String getNote() { return Note; }
        public String getRentDate() { return RentDate; }
        public String getEndRentDate() { return EndRentDate; }
        public String getDeadline() { return Deadline; }
    }
}
