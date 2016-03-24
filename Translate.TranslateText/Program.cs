/*
Copyright 2011 Google Inc

Licensed under the Apache License, Version 2.0(the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Google.Apis.Translate.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;

namespace Translate.TranslateText
{
    /// <summary>
    /// This example uses the Translate API to translate a user
    /// entered phrase from English to French or a language of the user's choice.
    ///
    /// Uses your DeveloperKey for authentication.
    /// </summary>
    internal class Program
    {
        #region User Input

        /// <summary>User input for this example.</summary>
        [Description("input")]
        public class TranslateInput
        {
            [Description("text to translate")]
            public string SourceText = "Who ate my candy?";

            [Description("target language")]
            public string TargetLanguage = "fr";
        }

        /// <summary>
        /// Creates a new instance of T and fills all public fields by requesting input from the user.
        /// </summary>
        /// <typeparam name="T">Class with a default constructor</typeparam>
        /// <returns>Instance of T with filled in public fields</returns>
        public static T CreateClassFromUserinput<T>()
        {
            var type = typeof(T);

            // Create an instance of T
            T settings = Activator.CreateInstance<T>();

            Console.WriteLine("Please enter values for the {0}:", GetDescriptiveName(type));

            // Fill in parameters
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                object value = field.GetValue(settings);

                // Let the user input a value
                RequestUserInput(GetDescriptiveName(field), ref value, field.FieldType);

                field.SetValue(settings, value);
            }

            Console.WriteLine();
            return settings;
        }

        /// <summary>
        /// Tries to return a descriptive name for the specified member info. It uses the DescriptionAttribute if
        /// available.
        /// </summary>
        /// <returns>Description from DescriptionAttriute or name of the MemberInfo</returns>
        public static string GetDescriptiveName(MemberInfo info)
        {
            // If available, return the description set in the DescriptionAttribute.
            foreach (DescriptionAttribute attribute in info.GetCustomAttributes(typeof(DescriptionAttribute), true))
            {
                return attribute.Description;
            }

            // Otherwise: return the name of the member.
            return info.Name;
        }

        /// <summary>Requests an user input for the specified value.</summary>
        /// <param name="name">Name to display.</param>
        /// <param name="value">Default value, and target value.</param>
        /// <param name="valueType">Type of the target value.</param>
        private static void RequestUserInput(string name, ref object value, Type valueType)
        {
            do
            {
                Console.Write("\t{0}: ", name);
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    // No change required, use default value.
                    return;
                }

                try
                {
                    value = Convert.ChangeType(input, valueType);
                    return;
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine("Please enter a valid value!");
                }
            } while (true); // Run this loop until the user gives a valid input.
        }

        #endregion User Input

        [STAThread]
        private static void Main(string[] args)
        {
            Console.WriteLine("Translate Sample");
            Console.WriteLine("================");

            try
            {
                //new Program().Run().Wait();
                new Program().CreateCSV().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task CreateCSV()
        {
            var key = "AIzaSyAPf4L8kiYR7Mv4aAWAyCF_hPDQ3ADcT3I";
            // Ask for the user input.
            //TranslateInput input = CreateClassFromUserinput<TranslateInput>();
            // Create the service.
            var service = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = key,
                ApplicationName = "Translate API Sample"
            });

            var file = @"C:\Temp\GoogleTranslate.csv";
            List<string> inputString = new List<string>() { "Settings", "Shift", "Update MOH's from MS Project", "Add Action", "View Actions", "ERRORS", "View and Set Resource Constraints", "Show Search", "Principales/Lances", "Filter for MS Project", "[ - ] Collapse All MOH", "Dept.View", "Tasks", "Process Critical", "Person", "Action", "Date", "Due Date", "Cancel", "OK", "Help/Overview Guide ?", "Shared Res View", "Type", "Resource Levelling Tool", "Alcoa - Resource Levelling Tool", "SHOW ALL", "FILTERS", "OUTSTANDINGS", "NEW", "ACTIONS (Selected Actions)", "Export Actions", "Mark as Complete", "Email", "Print Changes", "Delete Highlighted", "Set Resource Hrs and Qtys", "Manage Companies", "Please Wait", "Loading", "Expand All", "Collapse All", "Group by Resource then Department (default)", "Group by Departement the Resource", "Resource", "Manage Exceptions", "Department", "Start Date", "End Date", "Day", "Day Hrs", "Day Qty", "Night Hrs", "Night Qty", "Crew Information", "Crew", "Change Date", "Changed By", "Emp. No.", "Employee Name", "Reports", "Department Levelling", "Plan Errors", "Assign Dept", "Start", "Actions (Selected Errors)", "Print Errors", "Select", "Errors", "Department Code", "Use Averaging Method", "TimeFrame", "Vertical View", "Horizontal View", "Location View (I or K)", "12 Mth MOH Schedule", "All Data", "Process Critical (K or J)", "Top 20 Mat.", "List Changes", "Align. Req'd", "PRA Req'd", "Impact Views", "(Un)Check All", "Algn. Rqd.", "Approval Log", "Manage && Create/Edit", "Actions", "Add", "View", "Long Term Filters", "Current Year", "Next Full Year", "Approvals", "Align.", "All", "Extreme", "Asset not in Matrix", "Work Order", "Scheduled Dates", "Schedule Alignment Required", "Asset Number", "Job Type", "End", "Align. Code", "Risk Code", "Job Churn", "Alignment Tool", "Add / Edit Comment", "Date:", "Work Order #:", "Asset Number:", "Asset Activity:", "Select Color:", "Comment:", "Delete", "Last Modified By: n/a", "Last Modified Date: n/a", "Settings", "Alignment Constraints", "My Groups", "Horizontal Groups", "Process Risk Assessment & Action Plan - Risk Impacts Template", "Users/Roles", "(1) Individual Jobs", "(2) Asset vs Asset Clash", "(3) Asset Scheduling", "(4) Asset vs Asset Days Between", "Read Only Tabular View:", "Find", "Clear", "Else click button(s) to view/edit the graphical Matrix:", "Risk Desc.", "Impact Code", "Impact Desc", "Asset Number X", "Asset Number Y", "Read Only Tabular View:", "Add New Constraint", "Edit Selected", "Delete Selected", "Note: Please contact Yujia to add/Edit/Delete constraints", "Restriction Start", "Restriction End", "> Days Apart", "Filter Name", "Filter String", "Created By User Id", "Created Date", "Created By User Logon", "Created By", "User Logon", "User Full Name", "User Email", "Default Site Name", "Default Area Name", "ECT Group Leader", "OC Dept. Supervisor", "PC Supervisor", "Add New Group", "Ordinal", "Risk Impact Name", "Alignment Schedule Id", "Owning Department", "WKs To Go", "Iso Week Year", "Bldg", "WO Description", "Scheduled Date Start", "Scheduled Date End", "Asset Activity", "Job Type", "MOH Status", "In Matrix", "Work Order", "Scheduled Dates", "Schedule Alignment Required", "Process Risk Assessment & Action Plan", "Action", "WO Description", "Asset Number", "Align. Code", "Risk Code", "Currently With", "Revision", "Return to the Alignment Tool (Close) X", "Print", "Process Risk Assessments & Action Plans Management", "FILTERS - Status", "NOT STARTED", "DRAFT", "Pending Approval", "Pending Reviewal", "Approved", "FILTERS - Currently With", "ECT Group Leader (100)", "OC Dept. Supervisor (100)", "PC Supervisor (100)", "Current Schedule", "Archive (Includes all created Process Risk Assessments && Action Plans)", "Alignment Approval Id", " Alignment Approval Status Id", "Align Code", "Approved By", "Reviewed By", "Work Order No", "Work Order Desc", "Schd Start Date", "Schd End Date", "Last Modified Date", "Current Plan", "Archive", "Work Order Description", "Building", "Date Due", "OUTSTANDING", "OVERDUE", "From Process Risk Assessment", "ACTIONS (Selected Action)", "Date Entered", "Risk Impact", "Describe Risk", "Action Required", "When", "Complete", "Area", "Group Name", "Group Description", "Created Date", "Please Select View:", "Please Select Timeframe:", "Label (1-3 Characters only):", "Revision #", "Submit for Approvals", "View Printable Version (A4 PDF)", "View Previous", "Make Standard", "Header Information", "Work Order", "Asset", "Description", "Site Area", "Activity", "Status:", "Log:", "Add Comment", "Reject (Reset to Draft)", "Date", "Log Text", "By", "Approvals:", "Not Required", "OC Dept. Supervisor (R1+)", "OC Dept. Supervisor on behalf of OC/Dept. Manager (R2+)", "PC Supervisor (R3+)", "PC Supervisor on behalf of Prod and M&&R Manager (R4+)", "PC Supervisor (Review)", "Previous", "Next", "Work Scope Executive Summary", "(Describe key work scope elements in space below)", "Start Date", "Completion Date", "Duration", "Upload Sched/Key Milestone", "Browse...", "Delete", "Key comments related to Schedule/Key Milestones", "Click each Risk Impact as required to add it to the Action Plan:", "(Add other Risk Impact)", "ACTIONS (Selected Action)", "Edit", "Mark as Complete", "Email", "Risk Impact", "Describe Risk", "Describe Action Required to Eliminate/Mitigate Risk", "When", "Complete", "Scheduled Start Date:", "Scheduled Completion Date:", "Column", "Operator", "Value", "Recent changes/updates have been made to this REX Tool as detailed below to assist you in understanding any system or processes changes:", "Change Log", "Upcoming Changes", "Quick Reference Guide", "Note: Green Rows Indicate Proposed Change Matches EAM Schedule.", "Area", "Apply", "Site/Area", "Building", "Asset Number", "WO No.", "Job Type", "Work Order Description", "Schd Start Date", "Schd End Date", "Schd Start Date (EAM)", "Schd End Date (EAM)", "Stock #", "Total Cost", "Auto Request", "WO Schedule Date", "Viewed && Approved", "Approval Log", "Inventory Items WO Status Released", "Inventory Items WO Status Un-Released/Draft", "Item Description", "Qty Required", "Qty Issued", "Total Cost ($)", "Approved By", "Group Desc.", "Grouping:", "Level", "Sub Level", "Filter Name", "Filter Criterea", "Copy Current", "Wizard", "(Filter Criterea Automatically Copied from Main Screen)", "Risk Impact", "Ordinal", "Recent changes/updates have been made to this REX Tool as detailed below to assist you in understanding any system or processes changes:" };
            using (var stream = File.CreateText(file))
            {
                //stream.WriteLine("Hello Portuguese111!,line break\n 222, line break\n 333, line break\n");
                //stream.WriteLine("aaa!,line break\n bbb, line break\n ccc, line break\n");
                foreach (string inputText in inputString.ToArray())
                {
                    var response = await service.Translations.List(inputText, "pt").ExecuteAsync();
                    var translations = new List<string>();
                    foreach (TranslationsResource translation in response.Translations)
                    {
                        stream.WriteLine(string.Format("{0},{1}", inputText, translation.TranslatedText));
                    }
                }
            }
        }

        private async Task Run()
        {
            var key = GetApiKey();

            // Ask for the user input.
            TranslateInput input = CreateClassFromUserinput<TranslateInput>();

            // Create the service.
            var service = new TranslateService(new BaseClientService.Initializer()
                {
                    ApiKey = key,
                    ApplicationName = "Translate API Sample"
                });

            // Execute the first translation request.
            Console.WriteLine("Translating to '" + input.TargetLanguage + "' ...");

            string[] srcText = new[] { "Hello world!", input.SourceText };
            var response = await service.Translations.List(srcText, input.TargetLanguage).ExecuteAsync();
            var translations = new List<string>();

            foreach (TranslationsResource translation in response.Translations)
            {
                translations.Add(translation.TranslatedText);
                Console.WriteLine("translation :" + translation.TranslatedText);
            }

            // Translate the text (back) to English.
            Console.WriteLine("Translating to English ...");

            response = service.Translations.List(translations, "en").Execute();
            foreach (TranslationsResource translation in response.Translations)
            {
                Console.WriteLine("translation :" + translation.TranslatedText);
            }
        }

        private static string GetApiKey()
        {
            Console.WriteLine("Enter API Key");
            return "AIzaSyAPf4L8kiYR7Mv4aAWAyCF_hPDQ3ADcT3I";
        }
    }
}