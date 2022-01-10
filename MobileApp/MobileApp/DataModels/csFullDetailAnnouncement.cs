using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.DataModels
{
    class csFullDetailAnnouncement
    {
        private static string id, AlertLevel, Subject, Date, Details;


        public void setData( string id, string AlertLevel, string Subject, string Date, string Details)
        {
            csFullDetailAnnouncement.id = id;
            csFullDetailAnnouncement.AlertLevel = AlertLevel;
            csFullDetailAnnouncement.Subject = Subject;
            csFullDetailAnnouncement.Date = Date;
            csFullDetailAnnouncement.Details = Details;
        }

        public String getid()
        {
            return id;
        }

        public String getAlertLevel()
        {
            return AlertLevel;
        }

        public String getSubject()
        {
            return Subject;
        }

        public String getDate()
        {
            return Date;
        }

        public String getDetails()
        {
            return Details;
        }
    }
}
