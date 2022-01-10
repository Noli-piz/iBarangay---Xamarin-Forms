using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.DataModels
{
    class csFullDetailRequest
    {
        private static string id, Types, DateOfRequest, Purpose, Status, Options,Note;

        public void setData(string id, string Types, string DateOfRequest, string Purpose, 
            string Status, string Options, string Note)
        {
            csFullDetailRequest.id = id;
            csFullDetailRequest.Types = Types;
            csFullDetailRequest.DateOfRequest = DateOfRequest;
            csFullDetailRequest.Purpose = Purpose;
            csFullDetailRequest.Status = Status;
            csFullDetailRequest.Options = Options;
            csFullDetailRequest.Note = Note;
        }

        public String getid() { return id; }
        public String getType() { return Types; }
        public String getDateOfRequest() { return DateOfRequest; }
        public String getPurpose() { return Purpose; }
        public String getStatus() { return Status; }
        public String getOptions() { return Options; }
        public String getNote() { return Note; }
    }
}
