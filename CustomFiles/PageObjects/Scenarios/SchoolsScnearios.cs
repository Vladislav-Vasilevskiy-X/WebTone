using Core.SeleniumUtils;
using Core.SeleniumUtils.Core.Objects;
using CustomFiles.PageObjects.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace CustomFiles.PageObjects.Scenarios
{
    public class SchoolsScnearios : Scenario
    {
        public bool WriteSchoolsListToJsonFile(IList<School> schools)
        {
            schools.ToList().ForEach(s =>
            {
                s.Address = s.Address.RemoveEscapeCodes().Trim();
                s.ContactPhone = s.ContactPhone.RemoveEscapeCodes().Trim();
                s.Name = s.Name.RemoveEscapeCodes().Trim().Replace(@"\", string.Empty);
            });

            string output = new JavaScriptSerializer().Serialize(schools);
            string fileName = "Shools_for_" + DateTime.Now.ToString("dd_MM_yy_hh_mm_ss");
            string path = Directory.GetCurrentDirectory() + @"\\" + fileName + ".json";

            try
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(output);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
